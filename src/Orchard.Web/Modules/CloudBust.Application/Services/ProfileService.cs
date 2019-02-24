using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using CloudBust.Application.Models;
using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.DisplayManagement;
using Orchard.Environment;
using Orchard.Environment.Configuration;
using Orchard.Environment.Descriptor;
using Orchard.Environment.State;
using Orchard.Events;
using Orchard.Localization;
using Orchard.MediaLibrary.Models;
using Orchard.MediaLibrary.Services;
using Orchard.Messaging.Services;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.Services;
using Orchard.Settings;
using Orchard.Users.Models;

namespace CloudBust.Application.Services
{
    public interface IJobsQueueService : IEventHandler
    {
        void Enqueue(string message, object parameters, int priority);
    }

    public class ProfileService : IProfileService
    {
        public const string SignalName = "CloudBust.Application.ProfileService";
        private static readonly TimeSpan DelayToValidate = new TimeSpan(7, 0, 0, 0); // one week to validate email
        private static readonly TimeSpan DelayToResetPassword = new TimeSpan(1, 0, 0, 0); // 24 hours to reset password
        private readonly IApplicationsService _applicationsService;
        private readonly IClock _clock;
        private readonly IContentManager _contentManager;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<FriendRecord> _friendRepository;
        private readonly IRepository<InvitationPendingRecord> _invitationPendingRepository;
        private readonly ILoginsService _loginsService;
        private readonly IMediaLibraryService _mediaLibraryService;
        private readonly IMembershipService _membershipService;

        private readonly IMessageService _messageService;
        private readonly IProcessingEngine _processingEngine;
        private readonly ISettingsService _settingsService;
        private readonly IShapeDisplay _shapeDisplay;
        private readonly IShapeFactory _shapeFactory;
        private readonly IShellDescriptorManager _shellDescriptorManager;
        private readonly ShellSettings _shellSettings;

        private readonly ISignals _signals;
        private readonly ISiteService _siteService;
        private readonly IRepository<UserApplicationRecord> _userapplicationRepository;
        private readonly IRepository<UserProfilePartRecord> _userprofileRepository;
        private readonly Work<WorkContext> _workContext;

        public ProfileService(IContentManager contentManager, IMembershipService membershipService, ISiteService siteService, IClock clock, IMessageService messageService, IShapeFactory shapeFactory, IApplicationsService applicationsService, IShapeDisplay shapeDisplay, IEncryptionService encryptionService, IRepository<UserProfilePartRecord> userprofileRepository, IRepository<UserApplicationRecord> userapplicationRepository, IRepository<FriendRecord> friendRepository, ILoginsService loginsService, ISignals signals, IProcessingEngine processingEngine, ISettingsService settingsService, IRepository<InvitationPendingRecord> invitationPendingRepository, ShellSettings shellSettings, IShellDescriptorManager shellDescriptorManager, IMediaLibraryService mediaLibraryService, Work<WorkContext> workContext)
        {
            _contentManager = contentManager;
            _membershipService = membershipService;
            _clock = clock;
            _applicationsService = applicationsService;
            _messageService = messageService;
            _shapeFactory = shapeFactory;
            _siteService = siteService;
            _encryptionService = encryptionService;
            _shapeDisplay = shapeDisplay;
            T = NullLocalizer.Instance;
            _userprofileRepository = userprofileRepository;
            _userapplicationRepository = userapplicationRepository;
            _signals = signals;
            _loginsService = loginsService;
            _processingEngine = processingEngine;
            _invitationPendingRepository = invitationPendingRepository;
            _settingsService = settingsService;
            _shellSettings = shellSettings;
            _shellDescriptorManager = shellDescriptorManager;
            _workContext = workContext;
            _mediaLibraryService = mediaLibraryService;
            _friendRepository = friendRepository;
        }

        public Localizer T { get; set; }

        public bool CreateUserForApplicationRecord(UserProfilePart profilePart, ApplicationRecord appRecord)
        {
            var profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null) return false;

            var record = profileRecord.Applications.FirstOrDefault(x => x.ApplicationRecord.Name == appRecord.Name);
            if (record == null)
            {
                profileRecord.Applications.Add(new UserApplicationRecord
                {
                                                   UserProfilePartRecord = profileRecord,
                                                   ApplicationRecord = appRecord,
                                                   RegistrationStart = _clock.UtcNow
                });

                TriggerSignal();
            }

            if (profileRecord.Roles != null && profileRecord.Roles.Count != 0) return true;

            var defaultrole = _applicationsService.GetDefaultRole(appRecord);
            profileRecord.Roles?.Add(new UserUserRoleRecord
                                     {
                                         UserProfilePartRecord = profileRecord,
                                         UserRoleRecord = defaultrole
                                     });

            return true;
        }

        public UserProfilePart Get(IUser owner)
        {
            if (owner == null) return null;
            var user = owner.ContentItem;
            var profilepart = user.Parts.FirstOrDefault(p => p != null && p.TypePartDefinition.PartDefinition.Name == "UserProfilePart");
            return profilepart.As<UserProfilePart>();
        }

        public UserProfilePart Get(UserProfilePartRecord record)
        {
            return _contentManager.Get<UserProfilePart>(record.Id);
        }

        public UserProfilePart Get(int id)
        {
            return _contentManager.Get<UserProfilePart>(id);
        }

        public UserProfilePart Get(string username)
        {
            var owner = _loginsService.GetUser(username);

            return Get(owner);
        }

        public IUser GetGameCenterUserForVendorId(string vendorId)
        {
            try
            {
                var profiles = from profile in _userprofileRepository.Table where profile.VendorID == vendorId select profile;
                var r = profiles.FirstOrDefault();
                if (r != null)
                    return _contentManager.Query<UserPart, UserPartRecord>().Where(u => u.UserName == r.GCuserid).List().FirstOrDefault();
                return null;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<MediaPart> GetMediaFiles()
        {
            var mediafolder = GetMediaFolder();
            return _mediaLibraryService.GetMediaContentItems(mediafolder, 0, 0, "created", "");
        }

        public IEnumerable<MediaPart> GetMediaFiles(UserProfilePart profilePart, ApplicationRecord appRecord)
        {
            var mediafolder = GetMediaFolder(profilePart, appRecord);
            return _mediaLibraryService.GetMediaContentItems(mediafolder, 0, 0, "created", "");
        }

        public int GetMediaFilesCount()
        {
            var mediafolder = GetMediaFolder();
            return _mediaLibraryService.GetMediaContentItemsCountRecursive(mediafolder, null);
        }

        public int GetMediaFilesCount(UserProfilePart profilePart, ApplicationRecord appRecord)
        {
            var mediafolder = GetMediaFolder(profilePart, appRecord);
            return _mediaLibraryService.GetMediaContentItemsCountRecursive(mediafolder, null);
        }

        public string GetMediaFolder()
        {
            var mediafolder = _mediaLibraryService.GetRootedFolderPath("img");
            _mediaLibraryService.CreateFolder(null, mediafolder);
            return mediafolder;
        }

        public string GetMediaFolder(UserProfilePart profilePart, ApplicationRecord appRecord)
        {
            var uniqueId = GetUniqueId(profilePart);
            if (string.IsNullOrWhiteSpace(uniqueId)) return null;
            if (appRecord == null)
                appRecord = _settingsService.GetWebApplication();

            if (string.IsNullOrWhiteSpace(uniqueId) || appRecord == null) return null;

            // by design do not check if user exists in application

            var mediafolder = _mediaLibraryService.Combine(appRecord.Id.ToString(), uniqueId);
            _mediaLibraryService.CreateFolder(null, mediafolder);
            return mediafolder;
        }

        public UserPart GetParent(UserProfilePartRecord record)
        {
            return Get(record)?.As<UserPart>();
        }

        public UserPart GetParent(UserProfilePart part)
        {
            return part?.As<UserPart>();
        }

        public IUser GetUserForOrphanedVendorId(string vendorId)
        {
            try
            {
                return _contentManager.Query<UserPart, UserPartRecord>().Where(u => u.UserName == vendorId).List().FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<int> GetUserIDsForApplication(ApplicationRecord appRecord)
        {
            try
            {
                var users = GetUsersForApplication(appRecord);
                IList<int> myList = new List<int>();
                foreach (var user in users) myList.Add(user.Id);
                return myList;
            }
            catch
            {
                return new List<int>();
            }
        }

        public IEnumerable<UserRoleRecord> GetUserRoles(UserProfilePart profilePart, ApplicationRecord appRecord)
        {
            var profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null) return null;
            var record = profileRecord.Applications.FirstOrDefault(x => x.ApplicationRecord.Name == appRecord.Name);
            if (record == null) return new List<UserRoleRecord>();
            return (from con in profileRecord.Roles where con.UserRoleRecord.ApplicationRecord.Id == appRecord.Id select con.UserRoleRecord).ToList();
        }

        public bool SetUserRole(UserProfilePart profilePart, ApplicationRecord applicationRecord, string roleName, bool onlyThis = false)
        {
            var profileRecord = _userprofileRepository.Get(profilePart.Id);
            var record = profileRecord?.Applications.FirstOrDefault(x => x.ApplicationRecord.Name == applicationRecord.Name);
            if (record == null) return false;
            var applicationRole = _applicationsService.GetUserRoleByName(applicationRecord, roleName);
            if (applicationRole == null) return false;

            var userRole = new UserUserRoleRecord
                           {
                               UserProfilePartRecord = profileRecord,
                               UserRoleRecord = applicationRole
                           };

            if (onlyThis) RemoveAllUserRoles(profileRecord, applicationRecord);

            profileRecord.Roles.Add(userRole);
            _userprofileRepository.Flush();

            return true;
        }

        public void RemoveAllUserRoles(UserProfilePartRecord profileRecord, ApplicationRecord applicationRecord)
        {
            for (var roleIndex = 0; roleIndex < profileRecord.Roles.Count; roleIndex++)
            {
                var role = profileRecord.Roles[roleIndex];
                if (role.UserRoleRecord.ApplicationRecord == applicationRecord)
                    profileRecord.Roles.Remove(role);
            }
        }


        public IEnumerable<UserProfilePartRecord> GetUsersForApplication(ApplicationRecord appRecord)
        {
            try
            {
                var modules = from module in _userapplicationRepository.Table where module.ApplicationRecord.Name == appRecord.Name select module.UserProfilePartRecord;
                return modules.ToList();
            }
            catch
            {
                return new List<UserProfilePartRecord>();
            }
        }

        public bool IsUserInApplication(UserProfilePart profilePart, ApplicationRecord appRecord)
        {
            if (profilePart == null) return false;
            var profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null || appRecord == null) return false;

            var record = profileRecord.Applications.FirstOrDefault(x => x.ApplicationRecord.Name == appRecord.Name);
            return record != null;
        }

        public bool IsUserInApplication(IUser user, ApplicationRecord appRecord)
        {
            if (user == null) return false;
            var profilePart = Get(user).As<UserProfilePart>();
            return profilePart != null && IsUserInApplication(profilePart, appRecord);
        }

        public UserProfilePart ResetPassword(UserProfilePart profilePart)
        {
            if (profilePart == null) return null;

            profilePart.ResetPassword = true;

            return profilePart;
        }

        private void TriggerSignal()
        {
            _signals.Trigger(SignalName);
        }

        public string GetUniqueId(UserProfilePart profilePart)
        {
            if (profilePart == null) return null;
            var profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null) return null;


            if (string.IsNullOrWhiteSpace(profileRecord.UniqueID))
            {
                profileRecord.UniqueID = Guid.NewGuid().ToString("N");
                TriggerSignal();
            }

            return profileRecord.UniqueID;
        }

        #region registration

        public string CreateNonce(IUser user, TimeSpan delay, string appkey)
        {
            var challengeToken = new XElement("n", new XAttribute("ak", appkey), new XAttribute("un", user.UserName), new XAttribute("utc", _clock.UtcNow.ToUniversalTime().Add(delay).ToString(CultureInfo.InvariantCulture))).ToString();
            var data = Encoding.UTF8.GetBytes(challengeToken);
            return Convert.ToBase64String(_encryptionService.Encode(data));
        }

        public bool VerifyUserUnicity(string userName, string email)
        {
            var normalizedUserName = userName.ToLowerInvariant();

            if (_contentManager.Query<UserPart, UserPartRecord>().Where(user => user.NormalizedUserName == normalizedUserName || user.Email == email).List().Any())
                return false;

            return true;
        }

        public bool DecryptNonce(string nonce, out string username, out DateTime validateByUtc, out string appkey, out string error)
        {
            username = null;
            appkey = null;
            validateByUtc = _clock.UtcNow;

            try
            {
                var data = _encryptionService.Decode(Convert.FromBase64String(nonce));
                var xml = Encoding.UTF8.GetString(data);
                var element = XElement.Parse(xml);
                appkey = element.Attribute("ak")?.Value;
                username = element.Attribute("un")?.Value;
                validateByUtc = DateTime.Parse(element.Attribute("utc")?.Value, CultureInfo.InvariantCulture);
                var active = _clock.UtcNow <= validateByUtc;
                error = string.Empty;
                return active;
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return false;
            }
        }

        public bool SendChallengeMail(ApplicationRecord app, IUser user, Func<string, string> createUrl)
        {
            var nonce = CreateNonce(user, DelayToValidate, app.AppKey);
            var url = createUrl(nonce);

            var site = _siteService.GetSiteSettings();

            var template = _shapeFactory.Create("Template_User_Validation", Arguments.From(new
                                                                                           {
                                                                                               RegisteredWebsite = app.Name, //site.As<RegistrationSettingsPart>().ValidateEmailRegisteredWebsite,
                                                                                               ContactEmail = site.As<RegistrationSettingsPart>().ValidateEmailContactEMail,
                                                                                               ChallengeUrl = url,
                                                                                               ChallengeText = "Confirm Email"
                                                                                           }));
            template.Metadata.Wrappers.Add("Template_User_Wrapper");

            var parameters = new Dictionary<string, object>
                             {
                                 {"Application", app.AppKey},
                                 {"Subject", T("Please verify your E-Mail").Text},
                                 {"Body", _shapeDisplay.Display(template)},
                                 {"Recipients", user.Email}
                             };

            _messageService.Send("Email", parameters);
            return true;
        }

        public bool SendLostPasswordEmail(ApplicationRecord app, string usernameOrEmail, Func<string, string> createUrl)
        {
            var lowerName = usernameOrEmail.ToLowerInvariant();
            var user = _contentManager.Query<UserPart, UserPartRecord>().Where(u => u.NormalizedUserName == lowerName || u.Email == lowerName).List().FirstOrDefault();

            if (user != null)
            {
                var nonce = CreateNonce(user, DelayToResetPassword, app.AppKey);
                var url = createUrl(nonce);

                var template = _shapeFactory.Create("Template_User_LostPassword", Arguments.From(new
                                                                                                 {
                                                                                                     User = user,
                                                                                                     LostPasswordUrl = url
                                                                                                 }));
                template.Metadata.Wrappers.Add("Template_User_Wrapper");

                var parameters = new Dictionary<string, object>
                                 {
                                     {"Application", app.AppKey},
                                     {"Subject", T("Lost password").Text},
                                     {"Body", _shapeDisplay.Display(template)},
                                     {"Recipients", user.Email}
                                 };

                _messageService.Send("Email", parameters);
                return true;
            }

            return false;
        }

        public IUser ValidateChallenge(string nonce, out string appKey, out string description)
        {
            appKey = null;


            if (!DecryptNonce(nonce, out var username, out var validateByUtc, out var appkey, out _))
            {
                description = "decrypt error: " + string.Empty;
                return null;
            }

            if (validateByUtc < _clock.UtcNow)
            {
                description = "could not validate UTC";
                return null;
            }

            var user = _membershipService.GetUser(username);
            if (user == null)
            {
                description = "could not validate user";
                return null;
            }

            user.As<UserPart>().EmailStatus = UserStatus.Approved;
            appKey = appkey;

            var apprecord = _applicationsService.GetApplicationByKey(appkey);
            if (apprecord == null)
            {
                description = "could not validate application";
                return user;
            }

            CreateUserForApplicationRecord(user.As<UserProfilePart>(), apprecord);
            description = string.Empty;

            return user;
        }

        public IUser ValidateLostPassword(string nonce, out string appKey)
        {
            appKey = null;

            if (!DecryptNonce(nonce, out var username, out var validateByUtc, out _, out _)) return null;

            if (validateByUtc < _clock.UtcNow)
                return null;

            var user = _membershipService.GetUser(username);

            return user;
        }

        #endregion

        #region invitations

        private void CleanupInvitations(UserProfilePart profilePart, ApplicationRecord applicationRecord, string email)
        {
            try
            {
                var invitations = from invitation in _invitationPendingRepository.Table where invitation.ApplicationRecord.Id == applicationRecord.Id && invitation.UserProfilePartRecord.Id == profilePart.Id && invitation.invitationEmail.ToLowerInvariant() == email.ToLowerInvariant() select invitation;
                foreach (var invitation in invitations) invitation.UserProfilePartRecord.InvitationsPending.Remove(invitation);
            }
            catch { }
        }

        private void DeleteInvitationWithExpiredNonce(string nonce)
        {
            try
            {
                var invitations = from invitation in _invitationPendingRepository.Table where invitation.Hash == nonce select invitation;
                foreach (var invitation in invitations) invitation.UserProfilePartRecord.InvitationsPending.Remove(invitation);
            }
            catch { }
        }

        public bool IsFriendOfUser(UserProfilePart profilePart, ApplicationRecord appRecord, string username)
        {
            if (profilePart == null) return false;
            var profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null || appRecord == null) return false;

            var record = profileRecord.Friends.FirstOrDefault(x => x.ApplicationRecord == appRecord && x.UserName == username); //profileRecord.Applications.FirstOrDefault(x => x.ApplicationRecord.Name == appRecord.Name);
            if (record == null) return false;

            return true;
        }

        public bool AcceptInvitation(int id, IUser user, out UserProfilePart inviter, out UserProfilePart friend, out string appkey)
        {
            friend = null;
            inviter = null;
            appkey = string.Empty;
            try
            {
                var i = 0;

                var invitations = _invitationPendingRepository.Table.Where(item => item.Id == id).ToList();
                //var invitations = from invitation in _invitationPendingRepository.Table where invitation.Hash == nonce select invitation;
                foreach (var invitation in invitations)
                {
                    //invitation.UserProfilePartRecord.InvitationsPending.Remove(invitation);
                    var decrypted = DecryptInvitationNonce(invitation.Hash, out var userid, out var appid, out var email, out var valDate);
                    if (!decrypted) continue;

                    var profileRecord = _userprofileRepository.Get(userid);
                    if (profileRecord == null || string.IsNullOrWhiteSpace(email)) return false;
                    var profilePart = Get(profileRecord);
                    var friendPart = Get(email);

                    if (profilePart == null || friendPart == null) return false;
                    var friendRecord = _userprofileRepository.Get(friendPart.ContentItem.Record.Id);
                    if (friendRecord == null) return false;

                    var app = _applicationsService.GetApplication(appid);
                    if (app == null) return false;

                    if (!IsUserInApplication(profilePart, app)) return false;
                    if (!IsUserInApplication(friendPart, app)) return false;

                    appkey = app.AppKey;

                    if (!string.Equals(user.Email.ToLowerInvariant(), email.ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase))
                        if (app.owner != user.UserName)
                            return false;

                    friend = friendPart;
                    inviter = profilePart;

                    // check if already exists
                    if (IsFriendOfUser(profilePart, app, friendPart.UserName))
                    {
                        // already a user, log this attempt
                    }
                    else
                    {
                        profilePart.Friends.Add(new FriendRecord
                                                {
                                                    ApplicationRecord = app,
                                                    UserProfilePartRecord = profileRecord,
                                                    UserName = friendPart.UserName,
                                                    CreatedUtc = _clock.UtcNow
                                                });
                        TriggerSignal();
                    }

                    invitation.UserProfilePartRecord.InvitationsPending.Remove(invitation);
                    i++;
                }

                if (i > 0)
                    return true;
            }
            catch
            {
                return false;
            }

            return false;
        }

        public IEnumerable<UserProfilePart> GetInvitersOfUser(UserProfilePart userProfilePart, ApplicationRecord appRecord)
        {
            if (userProfilePart == null || string.IsNullOrWhiteSpace(userProfilePart.UserName))
                return new List<UserProfilePart>();
            return GetInvitersOfUser(userProfilePart.UserName, appRecord);
        }

        public IEnumerable<UserProfilePart> GetInvitersOfUser(string username, ApplicationRecord appRecord)
        {
            if (appRecord == null)
                appRecord = _settingsService.GetWebApplication();

            try
            {
                var friends = from friend in _friendRepository.Table where friend.ApplicationRecord == appRecord && friend.UserName.ToLowerInvariant() == username.ToLowerInvariant() select friend;
                var Friends = new List<UserProfilePart>();
                foreach (var friend in friends) Friends.Add(Get(friend.UserName));
                return Friends;
            }
            catch
            {
                return new List<UserProfilePart>();
            }
        }

        private InvitationPendingRecord CreateInvitation(UserProfilePart user, ApplicationRecord appRecord, string Message, string email)
        {
            var profileRecord = _userprofileRepository.Get(user.Id);
            if (profileRecord == null) return null;

            CleanupInvitations(user, appRecord, email);
            var inv = new InvitationPendingRecord();

            inv.UserProfilePartRecord = profileRecord;
            inv.Hash = createInvitationHash(profileRecord.Id, appRecord.Id, email, DelayToValidate);
            inv.CreatedUtc = null;
            inv.ApplicationRecord = appRecord;
            inv.Message = Message;
            inv.invitationEmail = email.ToLowerInvariant();
            profileRecord.InvitationsPending.Add(inv);

            return inv;
        }

        public InvitationPendingRecord GetPendingInvitation(int id)
        {
            var record = _invitationPendingRepository.Get(id);
            return record;
        }

        public bool IsPendingInvitationProcessed(int id)
        {
            var record = _invitationPendingRepository.Get(id);
            if (record != null)
                if (record.CreatedUtc.HasValue)
                    return true;
            return false;
        }

        public void PendingInvitationProcessed(int id)
        {
            var record = GetPendingInvitation(id);
            if (record != null)
            {
                var utcNow = _clock.UtcNow;
                record.CreatedUtc = utcNow;
            }
        }

        private string createInvitationHash(int userId, int appId, string email, TimeSpan delay)
        {
            var challengeToken = new XElement("n", new XAttribute("ui", userId), new XAttribute("ai", appId), new XAttribute("ie", email.ToLowerInvariant()), new XAttribute("utc", _clock.UtcNow.ToUniversalTime().Add(delay).ToString(CultureInfo.InvariantCulture))).ToString();
            var data = Encoding.UTF8.GetBytes(challengeToken);
            return Convert.ToBase64String(_encryptionService.Encode(data));
        }

        public void InviteWithEmail(ApplicationRecord applicationRecord, UserProfilePart user, string email, string Message)
        {
            var profileRecord = _userprofileRepository.Get(user.Id);
            if (profileRecord == null) return;

            if (applicationRecord == null)
                applicationRecord = _settingsService.GetWebApplication();

            var record = profileRecord.Applications.FirstOrDefault(x => x.ApplicationRecord.Name == applicationRecord.Name);
            if (record == null) return;

            var invitation = CreateInvitation(user, applicationRecord, Message, email);
            if (invitation != null)
            {
                var _Url = new UrlHelper(_workContext.Value.HttpContext.Request.RequestContext);
                var url = _Url.MakeAbsolute(_Url.Action("Invitation", "Account", new
                                                                                 {
                                                                                     Area = "CloudBust.Application",
                                                                                     nonce = invitation.Hash
                                                                                 }, "https"));


                _processingEngine.AddTask(_shellSettings, _shellDescriptorManager.GetShellDescriptor(), "IInvitationsProcessor.Invite", new Dictionary<string, object>
                                                                                                                                        {
                                                                                                                                            {"id", invitation.Id},
                                                                                                                                            {"url", url}
                                                                                                                                        });
            }
        }

        public bool DecryptInvitationNonce(string nonce, out int userid, out int appid, out string email, out DateTime validateByUtc)
        {
            userid = -1;
            appid = -1;
            email = null;
            validateByUtc = _clock.UtcNow;

            try
            {
                var data = _encryptionService.Decode(Convert.FromBase64String(nonce));
                var xml = Encoding.UTF8.GetString(data);
                var element = XElement.Parse(xml);
                userid = int.Parse(element.Attribute("ui").Value);
                appid = int.Parse(element.Attribute("ai").Value);
                email = element.Attribute("ie").Value;
                validateByUtc = DateTime.Parse(element.Attribute("utc").Value, CultureInfo.InvariantCulture);
                var active = _clock.UtcNow <= validateByUtc;
                if (!active)
                    DeleteInvitationWithExpiredNonce(nonce);
                return active;
            }
            catch
            {
                return false;
            }
        }

        public InvitationPendingRecord ValidateInvitationChallenge(string nonce, out UserProfilePart invitee, out ApplicationRecord application)
        {
            string email = null;
            var userid = -1;
            var appid = -1;

            invitee = null;
            application = null;
            DateTime validateByUtc;

            if (!DecryptInvitationNonce(nonce, out userid, out appid, out email, out validateByUtc)) return null;

            if (validateByUtc < _clock.UtcNow || userid < 1 || appid < 1 || string.IsNullOrWhiteSpace(email))
                return null;

            invitee = Get(userid);
            var profilePart = invitee;
            application = _applicationsService.GetApplication(appid);
            var applicationRecord = application;

            if (invitee == null || application == null)
                return null;


            var inv = (from invitation in _invitationPendingRepository.Table where invitation.ApplicationRecord.Id == applicationRecord.Id && invitation.UserProfilePartRecord.Id == profilePart.Id && invitation.invitationEmail.ToLowerInvariant() == email.ToLowerInvariant() select invitation).FirstOrDefault();

            if (inv == null) return null;

            return inv;
        }

        public IEnumerable<InvitationPendingRecord> GetPendingInvitations(UserProfilePart profilePart, ApplicationRecord applicationRecord)
        {
            if (applicationRecord == null)
                applicationRecord = _settingsService.GetWebApplication();

            var user = profilePart.As<IUser>();
            try
            {
                var invintations = from invitation in _invitationPendingRepository.Table where invitation.ApplicationRecord.Id == applicationRecord.Id && invitation.invitationEmail.ToLower() == user.Email.ToLowerInvariant() select invitation;

                foreach (var inv in invintations)
                {
                    var userid = 0;
                    var appid = 0;
                    string email = null;
                    DateTime vdate;
                    DecryptInvitationNonce(inv.Hash, out userid, out appid, out email, out vdate);
                }

                var cleaninvintations = from invitation in _invitationPendingRepository.Table where invitation.ApplicationRecord.Id == applicationRecord.Id && invitation.invitationEmail.ToLower() == user.Email.ToLowerInvariant() select invitation;

                return cleaninvintations.ToList();
            }
            catch
            {
                return new List<InvitationPendingRecord>();
            }
        }

        public IEnumerable<InvitationPendingRecord> GetPendingInvitationsSent(UserProfilePart profilePart, ApplicationRecord applicationRecord)
        {
            if (applicationRecord == null)
                applicationRecord = _settingsService.GetWebApplication();

            try
            {
                var invintations = from invitation in profilePart.InvitationsPending where invitation.ApplicationRecord.Id == applicationRecord.Id select invitation;

                return invintations.ToList();
            }
            catch
            {
                return new List<InvitationPendingRecord>();
            }
        }

        #endregion
    }
}