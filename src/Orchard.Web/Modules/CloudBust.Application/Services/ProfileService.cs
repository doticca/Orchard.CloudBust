using System.Linq;
using Orchard.ContentManagement;
using Orchard.Security;
using Orchard.Messaging.Services;
using Orchard.DisplayManagement;
using Orchard.Settings;
using Orchard.Localization;
using CloudBust.Application.Models;
using System;
using System.Xml.Linq;
using System.Text;
using Orchard.Services;
using System.Globalization;
using Orchard.Users.Models;
using System.Collections.Generic;
using Orchard.Data;
using Orchard.Caching;
using Orchard.Events;
using Orchard.Environment.State;
using Orchard.Environment.Configuration;
using Orchard.Environment.Descriptor;
using Orchard;
using Orchard.Environment;
using Orchard.Mvc.Extensions;
using Orchard.MediaLibrary.Services;

namespace CloudBust.Application.Services
{
    public interface IJobsQueueService : IEventHandler
    {
        void Enqueue(string message, object parameters, int priority);
    }
    public class ProfileService : IProfileService
    {
        private static readonly TimeSpan DelayToValidate = new TimeSpan(7, 0, 0, 0); // one week to validate email
        private static readonly TimeSpan DelayToResetPassword = new TimeSpan(1, 0, 0, 0); // 24 hours to reset password

        private readonly IMessageService _messageService;
        private readonly IMediaLibraryService _mediaLibraryService;
        private readonly IShapeFactory _shapeFactory;
        private readonly ISiteService _siteService;
        private readonly IApplicationsService _applicationsService;
        private readonly IContentManager _contentManager;
        private readonly IEncryptionService _encryptionService;
        private readonly IClock _clock;
        private readonly IShapeDisplay _shapeDisplay;
        private readonly IMembershipService _membershipService;
        private readonly ILoginsService _loginsService;
        private readonly IRepository<UserProfilePartRecord> _userprofileRepository;
        private readonly IRepository<UserApplicationRecord> _userapplicationRepository;
        private readonly IRepository<InvitationPendingRecord> _invitationPendingRepository;
        private readonly IRepository<FriendRecord> _friendRepository;
        private readonly IProcessingEngine _processingEngine;
        private readonly ISettingsService _settingsService;
        private readonly ShellSettings _shellSettings;
        private readonly IShellDescriptorManager _shellDescriptorManager;
        private readonly Work<WorkContext> _workContext;

        private readonly ISignals _signals;
        public const string SignalName = " CloudBust.Application.ProfileService";
        public ProfileService(
            IContentManager contentManager,
            IMembershipService membershipService,
            ISiteService siteService,
            IClock clock,
            IMessageService messageService,
            IShapeFactory shapeFactory,
            IApplicationsService applicationsService,
            IShapeDisplay shapeDisplay,
            IEncryptionService encryptionService,
            IRepository<UserProfilePartRecord> userprofileRepository,
            IRepository<UserApplicationRecord> userapplicationRepository,
            IRepository<FriendRecord> friendRepository,
            ILoginsService loginsService,
            ISignals signals,
            IProcessingEngine processingEngine,
            ISettingsService settingsService,
            IRepository<InvitationPendingRecord> invitationPendingRepository,
            ShellSettings shellSettings,
            IShellDescriptorManager shellDescriptorManager,
            IMediaLibraryService mediaLibraryService,
            Work<WorkContext> workContext
            )
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

        private void TriggerSignal()
        {
            _signals.Trigger(SignalName);
        }

        public UserProfilePart Get(IUser owner)
        {
            if (owner == null) return null;
            ContentItem user = owner.ContentItem;
            var profilepart = user.Parts.FirstOrDefault(p => p is ContentPart && p.TypePartDefinition.PartDefinition.Name == "UserProfilePart");
            return profilepart.As<UserProfilePart>();
        }
        public UserProfilePart Get(UserProfilePartRecord record)
        {
            return _contentManager.Get<UserProfilePart>(record.Id);
        }
        public UserPart GetParent(UserProfilePartRecord record)
        {
            UserProfilePart part = Get(record);
            if (part != null)
                return part.As<UserPart>();
            return null;
        }
        public UserPart GetParent(UserProfilePart part)
        {
            if (part != null)
                return part.As<UserPart>();
            return null;
        }
        public UserProfilePart Get(int Id)
        {
            return _contentManager.Get<UserProfilePart>(Id);
        }
        public UserProfilePart Get(string Username)
        {
            IUser owner = _loginsService.GetUser(Username);

            return Get(owner);
        }
        public bool IsUserInApplication(UserProfilePart profilePart, ApplicationRecord appRecord)
        {
            if (profilePart == null) return false;
            UserProfilePartRecord profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null || appRecord == null) return false;

            var record = profileRecord.Applications.FirstOrDefault(x => x.ApplicationRecord.Name == appRecord.Name);
            if (record == null) return false;

            return true;
        }
        public string GetUniqueID(UserProfilePart profilePart)
        {
            if (profilePart == null) return null;
            UserProfilePartRecord profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null) return null;


            if (string.IsNullOrWhiteSpace(profileRecord.UniqueID))
            {
                profileRecord.UniqueID = Guid.NewGuid().ToString("N");
                TriggerSignal();
            }
            return profileRecord.UniqueID;
        }
        public string GetMediaFolder(UserProfilePart profilePart, ApplicationRecord appRecord)
        {
            string uniqueID = GetUniqueID(profilePart);
            if (string.IsNullOrWhiteSpace(uniqueID)) return null;
            if (appRecord == null)
                appRecord = _settingsService.GetWebApplication();
            
            if (string.IsNullOrWhiteSpace(uniqueID) || appRecord == null) return null;

            // by design do not check if user exists in application

            string mediafolder = _mediaLibraryService.Combine(appRecord.Id.ToString(), uniqueID);
            _mediaLibraryService.CreateFolder(null, mediafolder);
            return mediafolder;
        }
        public int GetMediaFilesCount(UserProfilePart profilePart, ApplicationRecord appRecord)
        {
            string mediafolder = GetMediaFolder(profilePart, appRecord);
            return _mediaLibraryService.GetMediaContentItemsCountRecursive(mediafolder, null);          
        }
        public bool IsUserInApplication(IUser user, ApplicationRecord appRecord)
        {
            if (user == null) return false;
            UserProfilePart profilePart = Get(user).As<UserProfilePart>();
            if (profilePart == null) return false;
            return IsUserInApplication(profilePart, appRecord);
        }

        public bool CreateUserForApplicationRecord(UserProfilePart profilePart, ApplicationRecord appRecord)
        {
            UserProfilePartRecord profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null) return false;

            var utcNow = _clock.UtcNow;

            var record = profileRecord.Applications.FirstOrDefault(x => x.ApplicationRecord.Name == appRecord.Name);
            if (record == null)
            {

                profileRecord.Applications.Add(new UserApplicationRecord
                {
                    UserProfilePartRecord = profileRecord,
                    ApplicationRecord = appRecord,
                    RegistrationStart = utcNow
                });

                TriggerSignal();
            }

            if (profileRecord.Roles == null || profileRecord.Roles.Count == 0)
            {
                UserRoleRecord defaultrole = _applicationsService.GetDefaultRole(appRecord);
                profileRecord.Roles.Add(new UserUserRoleRecord
                {
                    UserProfilePartRecord = profileRecord,
                    UserRoleRecord = defaultrole
                });
            }

            return true;
        }

        public IEnumerable<UserRoleRecord> GetUserRoles(UserProfilePart profilePart, ApplicationRecord appRecord)
        {
            UserProfilePartRecord profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null) return null;
            var record = profileRecord.Applications.FirstOrDefault(x => x.ApplicationRecord.Name == appRecord.Name);
            if (record == null)
            {
                return new List<UserRoleRecord>();
            }
            var Roles = new List<UserRoleRecord>();
            foreach (UserUserRoleRecord con in profileRecord.Roles)
            {
                if (con.UserRoleRecord.ApplicationRecord.Id == appRecord.Id)
                {
                    Roles.Add(con.UserRoleRecord);
                }
            }
            return Roles;
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

        public IUser GetUserForOrphanedVendorId(string vendorID)
        {
            try
            {
                return _contentManager.Query<UserPart, UserPartRecord>().Where(u => u.UserName == vendorID).List().FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public IUser GetGameCenterUserForVendorId(string vendorID)
        {
            try
            {
                var profiles = from profile in _userprofileRepository.Table where profile.VendorID == vendorID select profile;
                var r = profiles.FirstOrDefault();
                if (r != null)
                {
                    return _contentManager.Query<UserPart, UserPartRecord>().Where(u => u.UserName == r.GCuserid).List().FirstOrDefault();
                }
                else
                {
                    return null;
                }
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
                foreach (var user in users)
                {
                    myList.Add(user.Id);
                }
                return myList;
            }
            catch
            {
                return new List<int>();
            }
        }

        #region registration
        public string CreateNonce(IUser user, TimeSpan delay, string appkey)
        {
            var challengeToken = new XElement("n",
                new XAttribute("ak", appkey),
                new XAttribute("un", user.UserName),
                new XAttribute("utc", _clock.UtcNow.ToUniversalTime().Add(delay).ToString(CultureInfo.InvariantCulture))).ToString();
            var data = Encoding.UTF8.GetBytes(challengeToken);
            return Convert.ToBase64String(_encryptionService.Encode(data));
        }
        public bool VerifyUserUnicity(string userName, string email)
        {
            string normalizedUserName = userName.ToLowerInvariant();

            if (_contentManager.Query<UserPart, UserPartRecord>()
                                   .Where(user =>
                                          user.NormalizedUserName == normalizedUserName ||
                                          user.Email == email)
                                   .List().Any())
            {
                return false;
            }

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
                appkey = element.Attribute("ak").Value;
                username = element.Attribute("un").Value;
                validateByUtc = DateTime.Parse(element.Attribute("utc").Value, CultureInfo.InvariantCulture);
                bool active = _clock.UtcNow <= validateByUtc;
                error = string.Empty;
                return active;
            }
            catch(Exception ex)
            {
                error = ex.ToString();
                return false;
            }

        }
        public bool SendChallengeMail(ApplicationRecord app, IUser user, Func<string, string> createUrl)
        {
            string nonce = CreateNonce(user, DelayToValidate, app.AppKey);
            string url = createUrl(nonce);

            var site = _siteService.GetSiteSettings();

            var template = _shapeFactory.Create("Template_User_Validation", Arguments.From(new
            {
                RegisteredWebsite = app.Name, //site.As<RegistrationSettingsPart>().ValidateEmailRegisteredWebsite,
                ContactEmail = site.As<RegistrationSettingsPart>().ValidateEmailContactEMail,
                ChallengeUrl = url,
                ChallengeText = app.Name + " Registration"
            }));
            template.Metadata.Wrappers.Add("Template_User_Wrapper");

            var parameters = new Dictionary<string, object> {
                        {"Application", app.AppKey},
                        {"Subject", T("Verification E-Mail").Text},
                        {"Body", _shapeDisplay.Display(template)},
                        {"Recipients", user.Email }
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
                string nonce = CreateNonce(user, DelayToResetPassword, app.AppKey);
                string url = createUrl(nonce);

                var template = _shapeFactory.Create("Template_User_LostPassword", Arguments.From(new
                {
                    User = user,
                    LostPasswordUrl = url
                }));
                template.Metadata.Wrappers.Add("Template_User_Wrapper");

                var parameters = new Dictionary<string, object> {
                            {"Application", app.AppKey},
                            {"Subject", T("Lost password").Text},
                            {"Body", _shapeDisplay.Display(template)},
                            {"Recipients", user.Email }
                        };

                _messageService.Send("Email", parameters);
                return true;
            }

            return false;
        }
        public IUser ValidateChallenge(string nonce, out string appKey, out string Description)
        {
            string username;
            string appkey;
            string error = string.Empty;

            appKey = null;


            DateTime validateByUtc;

            if (!DecryptNonce(nonce, out username, out validateByUtc, out appkey, out error))
            {
                Description = "decrypt error: " + error;
                return null;
            }

            if (validateByUtc < _clock.UtcNow)
            {
                Description = "could not validate UTC";
                return null;
            }

            var user = _membershipService.GetUser(username);
            if (user == null)
            {
                Description = "could not validate user";
                return null;
            }

            user.As<UserPart>().EmailStatus = UserStatus.Approved;
            appKey = appkey;

            ApplicationRecord apprecord = _applicationsService.GetApplicationByKey(appkey);
            if (apprecord == null)
            {
                Description = "could not validate application";
                return user;
            }

            CreateUserForApplicationRecord(user.As<UserProfilePart>(), apprecord);
            Description = string.Empty;

            return user;
        }

        public IUser ValidateLostPassword(string nonce, out string appKey)
        {
            string username;
            string appkey;
            string error = string.Empty;
            appKey = null;
            DateTime validateByUtc;

            if (!DecryptNonce(nonce, out username, out validateByUtc, out appkey, out error))
            {
                return null;
            }

            if (validateByUtc < _clock.UtcNow)
                return null;

            var user = _membershipService.GetUser(username);
            if (user == null)
                return null;

            return user;
        }
        #endregion

        #region invitations
        private void CleanupInvitations(UserProfilePart profilePart, ApplicationRecord applicationRecord, string email)
        {
            try
            {
                var invitations = from invitation in _invitationPendingRepository.Table
                                  where invitation.ApplicationRecord.Id == applicationRecord.Id 
                                  && invitation.UserProfilePartRecord.Id == profilePart.Id
                                  && invitation.invitationEmail.ToLowerInvariant() == email.ToLowerInvariant() select invitation;
                foreach (InvitationPendingRecord invitation in invitations)
                {
                    invitation.UserProfilePartRecord.InvitationsPending.Remove(invitation);
                }
            }
            catch
            {
                return;
            }
        }
        private void DeleteInvitationWithExpiredNonce(string nonce)
        {
            try
            {
                var invitations = from invitation in _invitationPendingRepository.Table
                                  where invitation.Hash == nonce
                                  select invitation;
                foreach (InvitationPendingRecord invitation in invitations)
                {
                    invitation.UserProfilePartRecord.InvitationsPending.Remove(invitation);
                }
            }
            catch
            {
                return;
            }
        }
        public bool IsFriendOfUser(UserProfilePart profilePart, ApplicationRecord appRecord, string UserName)
        {
            if (profilePart == null) return false;
            UserProfilePartRecord profileRecord = _userprofileRepository.Get(profilePart.Id);
            if (profileRecord == null || appRecord == null) return false;

            var record = profileRecord.Friends.FirstOrDefault(x => x.ApplicationRecord == appRecord && x.UserName == UserName); //profileRecord.Applications.FirstOrDefault(x => x.ApplicationRecord.Name == appRecord.Name);
            if (record == null) return false;

            return true;
        }
        public bool AcceptInvitation(string nonce, IUser user, out UserProfilePart inviter, out UserProfilePart friend, out string appkey)
        {
            friend = null;
            inviter = null;
            appkey = string.Empty;
            try
            {
                int i = 0;
                var invitations = from invitation in _invitationPendingRepository.Table
                                  where invitation.Hash == nonce
                                  select invitation;
                foreach (InvitationPendingRecord invitation in invitations)
                {
                    //invitation.UserProfilePartRecord.InvitationsPending.Remove(invitation);
                    int userid = 0;
                    int appid = 0;
                    string email = string.Empty;
                    DateTime valDate;
                    bool decrypted = DecryptInvitationNonce(nonce, out userid, out appid, out email, out valDate);
                    if(decrypted)
                    {
                        UserProfilePartRecord profileRecord = _userprofileRepository.Get(userid);
                        if (profileRecord == null || string.IsNullOrWhiteSpace(email)) return false;
                        UserProfilePart profilePart = Get(profileRecord);
                        UserProfilePart friendPart = Get(email);

                        if (profilePart == null || friendPart == null) return false;
                        UserProfilePartRecord friendRecord = _userprofileRepository.Get(friendPart.ContentItem.Record.Id);
                        if (friendRecord == null) return false;

                        ApplicationRecord app = _applicationsService.GetApplication(appid);
                        if (app == null) return false;

                        if (!IsUserInApplication(profilePart, app)) return false;
                        if (!IsUserInApplication(friendPart, app)) return false;

                        appkey = app.AppKey;

                        if (user.Email.ToLowerInvariant() != email.ToLowerInvariant())
                            if(app.owner != user.UserName)
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
                }
                if(i>0)
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
        public IEnumerable<UserProfilePart> GetInvitersOfUser(string UserName, ApplicationRecord appRecord)
        {
            if (appRecord == null)
                appRecord = _settingsService.GetWebApplication();

            try
            {
                var friends = (from friend in _friendRepository.Table
                                    where friend.ApplicationRecord == appRecord
                                    && friend.UserName.ToLowerInvariant() == UserName.ToLowerInvariant()
                                    select friend);
                var Friends = new List<UserProfilePart>();
                foreach (FriendRecord friend in friends)
                {
                    Friends.Add(Get(friend.UserName));                    
                }
                return Friends;
            }
            catch
            {
                return new List<UserProfilePart>();
            }
        }

        private InvitationPendingRecord CreateInvitation(UserProfilePart user, ApplicationRecord appRecord, string Message, string email)
        {
            UserProfilePartRecord profileRecord = _userprofileRepository.Get(user.Id);
            if (profileRecord == null) return null;

            CleanupInvitations(user, appRecord, email);
            InvitationPendingRecord inv = new InvitationPendingRecord();

            inv.UserProfilePartRecord = profileRecord;
            inv.Hash= createInvitationHash(profileRecord.Id, appRecord.Id, email, DelayToValidate);
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
            {
                if (record.CreatedUtc.HasValue)
                    return true;
            }
            return false;
        }
        public void PendingInvitationProcessed(int id)
        {
            var record = GetPendingInvitation(id);
            if(record!=null)
            {
                var utcNow = _clock.UtcNow;
                record.CreatedUtc = utcNow;
            }
        }
        private string createInvitationHash(int userId, int appId, string email, TimeSpan delay)
        {
            var challengeToken = new XElement("n",
                new XAttribute("ui", userId),
                new XAttribute("ai", appId),
                new XAttribute("ie", email.ToLowerInvariant()),
                new XAttribute("utc", _clock.UtcNow.ToUniversalTime().Add(delay).ToString(CultureInfo.InvariantCulture))).ToString();
            var data = Encoding.UTF8.GetBytes(challengeToken);
            return Convert.ToBase64String(_encryptionService.Encode(data));
        }
        public void InviteWithEmail(ApplicationRecord applicationRecord, UserProfilePart user, string email, string Message)
        {
            UserProfilePartRecord profileRecord = _userprofileRepository.Get(user.Id);
            if (profileRecord == null) return;

            if(applicationRecord == null)
                applicationRecord = _settingsService.GetWebApplication();

            var record = profileRecord.Applications.FirstOrDefault(x => x.ApplicationRecord.Name == applicationRecord.Name);
            if (record == null)
            {
                return;
            }

            var invitation = CreateInvitation(user, applicationRecord, Message, email);
            if (invitation !=null)
            {
                var _Url = new System.Web.Mvc.UrlHelper(_workContext.Value.HttpContext.Request.RequestContext);
                string url = _Url.MakeAbsolute(
                                                        _Url.Action("Invitation", "Account", new
                                                        {
                                                            Area = "CloudBust.Application",
                                                            nonce = invitation.Hash
                                                        },
                                                        "https"
                                                        )
                                                    );


                _processingEngine.AddTask(_shellSettings, _shellDescriptorManager.GetShellDescriptor(), "IInvitationsProcessor.Invite", new Dictionary<string, object>
                {
                    { "id", invitation.Id },
                    { "url", url }
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
                userid = Int32.Parse(element.Attribute("ui").Value);
                appid = Int32.Parse(element.Attribute("ai").Value);
                email = element.Attribute("ie").Value;
                validateByUtc = DateTime.Parse(element.Attribute("utc").Value, CultureInfo.InvariantCulture);
                bool active = _clock.UtcNow <= validateByUtc;
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
            int userid = -1;
            int appid = -1;

            invitee = null;
            application = null;
            DateTime validateByUtc;

            if (!DecryptInvitationNonce(nonce, out userid, out appid, out email, out validateByUtc))
            {
                return null;
            }

            if (validateByUtc < _clock.UtcNow || userid < 1 || appid < 1 || string.IsNullOrWhiteSpace(email))
                return null;

            invitee = Get(userid);
            var profilePart = invitee;
            application = _applicationsService.GetApplication(appid);
            var applicationRecord = application;

            if (invitee == null || application == null)
                return null;


            var inv = (from invitation in _invitationPendingRepository.Table
                              where invitation.ApplicationRecord.Id == applicationRecord.Id
                              && invitation.UserProfilePartRecord.Id == profilePart.Id
                              && invitation.invitationEmail.ToLowerInvariant() == email.ToLowerInvariant()
                              select invitation).FirstOrDefault();

            if (inv == null) return null;

            return inv;
        }
        public IEnumerable<InvitationPendingRecord> GetPendingInvitations(UserProfilePart profilePart, ApplicationRecord applicationRecord)
        {
            if (applicationRecord == null)
                applicationRecord = _settingsService.GetWebApplication();

            IUser user = profilePart.As<IUser>();
            try
            {
                var invintations = (from invitation in _invitationPendingRepository.Table
                           where invitation.ApplicationRecord.Id == applicationRecord.Id
                           && invitation.invitationEmail.ToLower() == user.Email.ToLowerInvariant()
                           select invitation);

                foreach(var inv in invintations)
                {
                    int userid = 0;
                    int appid = 0;
                    string email = null;
                    DateTime vdate;
                    DecryptInvitationNonce(inv.Hash, out userid, out appid, out email, out vdate);                    
                }

                var cleaninvintations = (from invitation in _invitationPendingRepository.Table
                                    where invitation.ApplicationRecord.Id == applicationRecord.Id
                                    && invitation.invitationEmail.ToLower() == user.Email.ToLowerInvariant()
                                    select invitation);

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
                var invintations = (from invitation in profilePart.InvitationsPending
                                    where invitation.ApplicationRecord.Id == applicationRecord.Id
                                    select invitation);

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