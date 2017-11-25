using System.Linq;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;
using CloudBust.Application.Models;
using System;
using System.Xml.Linq;
using System.Text;
using Orchard.Services;
using System.Globalization;
using Orchard.Users.Models;
using Orchard.Data;
using Orchard.Logging;

namespace CloudBust.Application.Services
{
    public class LoginsService : ILoginsService
    {
        private static readonly TimeSpan DelayToValidate = new TimeSpan(90, 0, 0, 0); // one week to keep hash alive
        private readonly IOrchardServices _orchardServices;
        private readonly IApplicationsService _applicationsService;
        private readonly IEncryptionService _encryptionService;
        private readonly IClock _clock;
        private readonly IRepository<UserProfilePartRecord> _userprofileRepository;
        private readonly IRepository<UserApplicationRecord> _userapplicationRepository;
        private readonly IRepository<LoginsRecord> _loginsRepository;
        private readonly ISettingsService _settingsService;

        public LoginsService(
            IOrchardServices orchardServices, 
            IContentManager contentManager,
            IClock clock,
            IApplicationsService applicationsService,
            IEncryptionService encryptionService,
            IRepository<UserProfilePartRecord> userprofileRepository,
            IRepository<UserApplicationRecord> userapplicationRepository,
            IRepository<LoginsRecord> loginsRepository,
            ISettingsService settingsService

            )
        {
            _orchardServices = orchardServices;
            _clock = clock;
            _applicationsService = applicationsService;
            _encryptionService = encryptionService;
            _userprofileRepository = userprofileRepository;
            _userapplicationRepository = userapplicationRepository;
            _loginsRepository = loginsRepository;
            _settingsService = settingsService;
            Logger = NullLogger.Instance;
        }
        public ILogger Logger { get; set; }

        public IUser CheckUser()
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;
            if (user != null)
            {
                int aid = GetSessionAppId(user);
                ApplicationRecord app = _applicationsService.GetApplication(aid);
                if (app != null && app.AppKey == _settingsService.GetWebApplicationKey())
                {
                    return user;
                }
            }
            return null;
        }

        public LoginsRecord LoginWithHash(string Hash)
        {
            return null;
        }

        public IUser GetUser(int Id)
        {
            return _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.Id == Id).List().FirstOrDefault();
        }
        public IUser GetUser(string Username)
        {
            string Normalizedusername = Username.ToLowerInvariant();
            return _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.NormalizedUserName == Normalizedusername || u.Email == Username).List().FirstOrDefault();
        }

        public IUser ValidateHash(string Hash, string ApiKey, out string reason)
        {
            reason = string.Empty;
            LoginsRecord loginrecord = null;
            try
            {
                var logins = from login in _loginsRepository.Table where login.Hash == Hash select login;
                if (logins == null)
                {
                    reason = "Hash not Found.";
                    return null;
                }
                loginrecord = logins.FirstOrDefault();
                if (loginrecord == null)
                {
                    reason = "Hash not Found.";
                    return null;
                }

                 
                var hasharray = Convert.FromBase64String(loginrecord.Hash);
                Logger.Information(String.Format("Will decrypt {0}.", loginrecord.Hash));
                var data = _encryptionService.Decode(hasharray);
                var xml = Encoding.UTF8.GetString(data);
                var element = XElement.Parse(xml);
                DateTime validateByUtc;
                string appid = element.Attribute("ai").Value;
                string userid = element.Attribute("ui").Value;
                validateByUtc = DateTime.Parse(element.Attribute("utc").Value, CultureInfo.InvariantCulture);
                if (_clock.UtcNow <= validateByUtc)
                {
                    int aid;
                    ApplicationRecord app = null;
                    if(Int32.TryParse(appid, out aid))
                        app = _applicationsService.GetApplication(aid);
                    if (app != null && app.AppKey == ApiKey)
                    {
                        int uid;
                        if (Int32.TryParse(userid, out uid))
                            return GetUser(uid);
                    }
                    reason = "Application is not supported on this server";
                }
                else
                {
                    _loginsRepository.Delete(loginrecord);
                    reason = "Hash record expired.";
                    return null;
                }
            }
            catch(Exception ex)
            {
                if(loginrecord != null)
                    reason = "Data: " + loginrecord.Hash + " Error: " + ex.ToString();
                else
                    reason = "Error: " + ex.ToString();
                return null;
            }
            return null;
        }

        public void DeleteHash(string Hash)
        {
            try
            {
                var logins = from login in _loginsRepository.Table where login.Hash == Hash select login;
                foreach (LoginsRecord login in logins)
                {
                    _loginsRepository.Delete(login);
                }
            }
            catch
            {
                return;
            }
        }
        public void CleanupHashes(UserProfilePart profilePart, ApplicationRecord applicationRecord)
        {
            try
            {
                var logins = from login in _loginsRepository.Table where login.ApplicationRecord.Id == applicationRecord.Id && login.UserProfilePartRecord.Id == profilePart.Id select login;
                foreach (LoginsRecord login in logins)
                {

                    DateTime allowed = _clock.UtcNow.Subtract(DelayToValidate);
                    if(DateTime.Compare(login.UpdatedUtc.Value, allowed)>0)
                        _loginsRepository.Delete(login);
                }
            }
            catch
            {
                return;
            }
        }
        public string GetHash(UserProfilePart profilePart, ApplicationRecord applicationRecord)
        {
            UserProfilePartRecord profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null) return null;
            try
            {
                var logins = from login in _loginsRepository.Table where login.ApplicationRecord.Id == applicationRecord.Id && login.UserProfilePartRecord.Id == profilePart.Id select login;
                //foreach (LoginsRecord login in logins)
                //{
                //    _loginsRepository.Delete(login);
                //}
                var first = logins.FirstOrDefault();
                if(first !=null)
                    return first.Hash;
            }
            catch
            {
                return null;
            }
            return null;
        }
        public string CreateHash(UserProfilePart profilePart, ApplicationRecord applicationRecord)
        {
            UserProfilePartRecord profileRecord =  _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null) return null;

            // first delete all hashes for this user and application
            CleanupHashes(profilePart, applicationRecord);

            var utcNow = _clock.UtcNow;

            LoginsRecord r = new LoginsRecord();
            r.Hash = createHash(profileRecord.Id, applicationRecord.Id, DelayToValidate);
            r.UserProfilePartRecord = profileRecord;
            r.ApplicationRecord = applicationRecord;
            r.UpdatedUtc = utcNow;

            _loginsRepository.Create(r);

            return r.Hash;
        }
        public DateTime GetServerTime()
        {
            return _clock.UtcNow;
        }
        public void ClearSessionAppId()
        {
            _orchardServices.WorkContext.HttpContext.Session.Remove("doticca_aid");
        }
        public void SetSessionAppId(int applicationID)
        {
            try
            {
                _orchardServices.WorkContext.HttpContext.Session["doticca_aid"] = applicationID;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public int GetSessionAppId(IUser user)
        {
            if (user == null)
                return 0;

            try
            {
                string appid = _orchardServices.WorkContext.HttpContext.Session["doticca_aid"].ToString();

                if (string.IsNullOrWhiteSpace(appid))
                {
                    var app = _settingsService.GetWebApplication();
                    if (app != null)
                    {
                        SetSessionAppId(app.Id);
                        return app.Id;
                    }
                    else
                        return 0;
                }
                int aid;
                if (!Int32.TryParse(appid, out aid))
                    return 0;

                return aid;
            }
            catch
            {
                try
                {
                    var app = _settingsService.GetWebApplication();
                    if (app != null)
                    {
                        SetSessionAppId(app.Id);
                        return app.Id;
                    }                   
                }
                catch
                {
                    return 0;
                }
                return 0;
            }
        }

        private string createHash(int userId, int appId, TimeSpan delay)
        {
            var challengeToken = new XElement("n",
                new XAttribute("ui", userId),
                new XAttribute("ai", appId),
                new XAttribute("utc", _clock.UtcNow.ToUniversalTime().Add(delay).ToString(CultureInfo.InvariantCulture))).ToString();
            var data = Encoding.UTF8.GetBytes(challengeToken);
            return Convert.ToBase64String(_encryptionService.Encode(data));
        }

    }
}