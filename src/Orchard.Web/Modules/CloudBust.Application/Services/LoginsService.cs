using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CloudBust.Application.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Logging;
using Orchard.Security;
using Orchard.Services;
using Orchard.Users.Models;

namespace CloudBust.Application.Services {
    public class LoginsService : ILoginsService {
        private static readonly TimeSpan DelayToValidate = new TimeSpan(90, 0, 0, 0); // one week to keep hash alive
        private readonly IApplicationsService _applicationsService;
        private readonly IClock _clock;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<LoginsRecord> _loginsRepository;
        private readonly IOrchardServices _orchardServices;
        private readonly ISettingsService _settingsService;
        private readonly IRepository<UserProfilePartRecord> _userprofileRepository;

        public LoginsService(
            IOrchardServices orchardServices,
            IClock clock,
            IApplicationsService applicationsService,
            IEncryptionService encryptionService,
            IRepository<UserProfilePartRecord> userprofileRepository,
            IRepository<LoginsRecord> loginsRepository,
            ISettingsService settingsService
        ) {
            _orchardServices = orchardServices;
            _clock = clock;
            _applicationsService = applicationsService;
            _encryptionService = encryptionService;
            _userprofileRepository = userprofileRepository;
            _loginsRepository = loginsRepository;
            _settingsService = settingsService;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public IUser CheckUser() {
            var user = _orchardServices.WorkContext.CurrentUser;

            if (user != null) {
                var aid = GetSessionAppId(user);
                var app = _applicationsService.GetApplication(aid);
                if (app != null && app.AppKey == _settingsService.GetWebApplicationKey()) return user;
            }

            return null;
        }

        public void CleanupHashes(UserProfilePart profilePart, ApplicationRecord applicationRecord) {
            try {
                var logins = from login in _loginsRepository.Table where login.ApplicationRecord.Id == applicationRecord.Id && login.UserProfilePartRecord.Id == profilePart.Id select login;
                foreach (var login in logins) {
                    var allowed = _clock.UtcNow.Subtract(DelayToValidate);
                    if (login.UpdatedUtc != null && DateTime.Compare(login.UpdatedUtc.Value, allowed) > 0)
                        _loginsRepository.Delete(login);
                }
            }
            catch { }
        }

        public void ClearSessionAppId() {
            _orchardServices.WorkContext.HttpContext.Session.Remove("doticca_aid");
        }

        public string CreateHash(UserProfilePart profilePart, ApplicationRecord applicationRecord) {
            var profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null) return null;

            // first delete all hashes for this user and application
            CleanupHashes(profilePart, applicationRecord);

            var utcNow = _clock.UtcNow;

            var r = new LoginsRecord();
            r.Hash = CreateHash(profileRecord.Id, applicationRecord.Id, DelayToValidate);
            r.UserProfilePartRecord = profileRecord;
            r.ApplicationRecord = applicationRecord;
            r.UpdatedUtc = utcNow;

            _loginsRepository.Create(r);

            return r.Hash;
        }

        public void DeleteHash(string Hash) {
            try {
                var logins = from login in _loginsRepository.Table where login.Hash == Hash select login;
                foreach (var login in logins) _loginsRepository.Delete(login);
            }
            catch { }
        }

        public string GetHash(UserProfilePart profilePart, ApplicationRecord applicationRecord) {
            var profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null) return null;
            try {
                var logins = from login in _loginsRepository.Table where login.ApplicationRecord.Id == applicationRecord.Id && login.UserProfilePartRecord.Id == profilePart.Id select login;

                var first = logins.FirstOrDefault();
                if (first != null)
                    return first.Hash;
            }
            catch {
                return null;
            }

            return null;
        }

        public DateTime GetServerTime() {
            return _clock.UtcNow;
        }

        public int GetSessionAppId(IUser user) {
            if (user == null)
                return 0;

            try {
                var appid = _orchardServices.WorkContext.HttpContext.Session["doticca_aid"].ToString();

                if (string.IsNullOrWhiteSpace(appid)) {
                    var app = _settingsService.GetWebApplication();
                    if (app != null) {
                        SetSessionAppId(app.Id);
                        return app.Id;
                    }

                    return 0;
                }

                int aid;
                if (!int.TryParse(appid, out aid))
                    return 0;

                return aid;
            }
            catch {
                try {
                    var app = _settingsService.GetWebApplication();
                    if (app != null) {
                        SetSessionAppId(app.Id);
                        return app.Id;
                    }
                }
                catch {
                    return 0;
                }

                return 0;
            }
        }

        public IUser GetUser(int Id) {
            return _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.Id == Id).List().FirstOrDefault();
        }

        public IUser GetUser(string username) {
            var normalizedusername = username.ToLowerInvariant();
            return _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.NormalizedUserName == normalizedusername || u.Email == username).List().FirstOrDefault();
        }

        public LoginsRecord LoginWithHash(string Hash) {
            return null;
        }

        public void SetSessionAppId(int applicationId) {
            try {
                _orchardServices.WorkContext.HttpContext.Session["doticca_aid"] = applicationId;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }

        public IUser ValidateHash(string hash, string apiKey, out string reason) {
            reason = string.Empty;
            LoginsRecord loginrecord = null;
            try {
                var logins = from login in _loginsRepository.Table where login.Hash == hash select login;
                if (!logins.Any()) {
                    reason = "Hash not Found.";
                    return null;
                }

                loginrecord = logins.FirstOrDefault();
                if (loginrecord == null) {
                    reason = "Hash not Found.";
                    return null;
                }

                var hasharray = Convert.FromBase64String(loginrecord.Hash);
                var data = _encryptionService.Decode(hasharray);
                var xml = Encoding.UTF8.GetString(data);
                var element = XElement.Parse(xml);
                var appid = element.Attribute("ai")?.Value;
                var userid = element.Attribute("ui")?.Value;
                var validateByUtc = DateTime.Parse(element.Attribute("utc")?.Value, CultureInfo.InvariantCulture);
                if (_clock.UtcNow <= validateByUtc) {
                    ApplicationRecord app = null;
                    if (int.TryParse(appid, out var aid))
                        app = _applicationsService.GetApplication(aid);
                    if (app != null && app.AppKey == apiKey) {
                        if (int.TryParse(userid, out var uid))
                            return GetUser(uid);
                    }

                    reason = "Application is not supported on this server";
                }
                else {
                    _loginsRepository.Delete(loginrecord);
                    reason = "Hash record expired.";
                    return null;
                }
            }
            catch (Exception ex) {
                if (loginrecord != null)
                    reason = "Data: " + loginrecord.Hash + " Error: " + ex;
                else
                    reason = "Error: " + ex;
                return null;
            }

            return null;
        }

        private string CreateHash(int userId, int appId, TimeSpan delay) {
            var challengeToken = new XElement("n",
                new XAttribute("ui", userId),
                new XAttribute("ai", appId),
                new XAttribute("utc", _clock.UtcNow.ToUniversalTime().Add(delay).ToString(CultureInfo.InvariantCulture))).ToString();
            var data = Encoding.UTF8.GetBytes(challengeToken);
            return Convert.ToBase64String(_encryptionService.Encode(data));
        }
    }
}