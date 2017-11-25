using Orchard;
using Orchard.Security;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloudBust.Application.OData;
using Orchard.Users.Models;
using Orchard.ContentManagement;
using CloudBust.Common.OData;
using System;
using Orchard.Users.Events;
using CloudBust.Application.Helpers;
using CloudBust.Application.Services;
using CloudBust.Application.Models;
using Facebook;
using System.Text.RegularExpressions;
using Orchard.Utility.Extensions;
using Orchard.Mvc.Extensions;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using Orchard.Caching;
using CloudBust.Application.OData.Profile;
using System.Globalization;

namespace CloudBust.Application.Controllers
{
    public class loginController : ApiController
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IMembershipService _membershipService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserEventHandler _userEventHandler;
        private readonly IApplicationsService _applicationsService;
        private readonly IProfileService _profileService;
        private readonly ILoginsService _loginsService;
        private readonly IContentManager _contentManager;
        private readonly ISettingsService _settingsService;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public loginController(
                                IAuthenticationService authenticationService, 
                                IOrchardServices orchardServices,
                                IMembershipService membershipService,
                                IUserEventHandler userEventHandler,
                                IApplicationsService applicationsService,
                                IProfileService profileService,
                                IContentManager contentManager,
                                ILoginsService loginsService,
                                ISettingsService settingsService,
                                ICacheManager cacheManager,
                                ISignals signals
                               )
        {
            _authenticationService = authenticationService;
            _orchardServices = orchardServices;
            _membershipService = membershipService;
            _userEventHandler = userEventHandler;
            _profileService = profileService;
            _applicationsService = applicationsService;
            _loginsService = loginsService;
            _contentManager = contentManager;
            _settingsService = settingsService;
            _cacheManager = cacheManager;
            _signals = signals;
        }


        #region Game Center

        private X509Certificate2 GetCertificate(string url)
        {
            var client = new WebClient();
            var rawData = client.DownloadData(url);
            return new X509Certificate2(rawData);
        }
        private static byte[] ToBigEndian(ulong value)
        {
            var buffer = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                buffer[7 - i] = unchecked((byte)(value & 0xff));
                value = value >> 8;
            }
            return buffer;
        }
        private byte[] ConcatSignature(string playerId, string bundleId, ulong timestamp, string salt)
        {
            var data = new List<byte>();
            data.AddRange(Encoding.UTF8.GetBytes(playerId));
            data.AddRange(Encoding.UTF8.GetBytes(bundleId));
            data.AddRange(ToBigEndian(timestamp));
            data.AddRange(Convert.FromBase64String(salt));
            return data.ToArray();
        }
        private bool ValidateSignature(LoginGC login, string BundleId)
        {
            try
            {
                var cert = GetCertificate(login.PublicKeyUrl);
                if (cert.Verify())
                {
                    var csp = cert.PublicKey.Key as RSACryptoServiceProvider;
                    if (csp != null)
                    {
                        var sha256 = new SHA256Managed();
                        var sig = ConcatSignature(login.Username, BundleId, login.Timestamp, login.Salt);
                        var hash = sha256.ComputeHash(sig);
                        var signature = Convert.FromBase64String(login.Signature);

                        if (csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), signature))
                        {
                            // Valid user.
                            // Do server related user management stuff.
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine(ex);
                return false;
            }
        }
        private bool ValidateLogonBuild(LoginGC login, out string updateurl)
        {
            updateurl = string.Empty;
            ApplicationRecord apprecord = _applicationsService.GetApplicationByKey(login.ApiKey);
            if (apprecord == null)
                return false;           // wrong cloudbast application id

            switch (login.Platform)
            {
                case "ios":
                    updateurl = apprecord.UpdateUrl;
                    break;
                case "osx":
                    updateurl = apprecord.UpdateUrlOSX;
                    break;
                case "tvos":
                    updateurl = apprecord.UpdateUrlTvOS;
                    break;
                case "watch":
                    updateurl = apprecord.UpdateUrlWatch;
                    break;
                default:
                    updateurl = apprecord.UpdateUrlDeveloper;
                    break;

            }

            if((int)login.Build<apprecord.MinimumClientBuild || (int)login.Build > apprecord.ServerBuild)
            {
                return false;
            }
            return true;
        }
        private string ValidateLogonBuild(Login login)
        {
            string updateurl = string.Empty;
            ApplicationRecord apprecord = _applicationsService.GetApplicationByKey(login.ApiKey);
            if (apprecord == null)
                return " ";           // wrong cloudbast application id

            switch (login.Platform)
            {
                case "ios":
                    updateurl = apprecord.UpdateUrl;
                    if (string.IsNullOrWhiteSpace(updateurl)) updateurl = "No Data available";
                    break;
                case "osx":
                    updateurl = apprecord.UpdateUrlOSX;
                    if (string.IsNullOrWhiteSpace(updateurl)) updateurl = "No Data available";
                    break;
                case "tvos":
                    updateurl = apprecord.UpdateUrlTvOS;
                    if (string.IsNullOrWhiteSpace(updateurl)) updateurl = "No Data available";
                    break;
                case "watch":
                    updateurl = apprecord.UpdateUrlWatch;
                    if (string.IsNullOrWhiteSpace(updateurl)) updateurl = "No Data available";
                    break;
                default:
                    updateurl = apprecord.UpdateUrlDeveloper;
                    if (string.IsNullOrWhiteSpace(updateurl)) updateurl = "No Data available";
                    break;

            }

            if ((int)login.Build < apprecord.MinimumClientBuild || (int)login.Build > apprecord.ServerBuild)
            {
                return updateurl;
            }
            return string.Empty;
        }
        private IUser ValidateLogonGameCenter(LoginGC login, out string Hash, out bool newUser)
        {
            Hash = string.Empty;
            newUser = true;
            ApplicationRecord apprecord = _applicationsService.GetApplicationByKey(login.ApiKey);
            if (apprecord == null)
                return null;           // wrong cloudbast application id

            string bundleID = string.Empty;
            switch (login.Platform)
            {
                case "ios":
                    bundleID = apprecord.BundleIdentifier;
                    break;
                case "osx":
                    bundleID = apprecord.BundleIdentifierOSX;
                    break;
                case "tvos":
                    bundleID = apprecord.BundleIdentifierTvOS;
                    break;
                case "watch":
                    bundleID = apprecord.BundleIdentifierWatch;
                    break;
                default:
                    bundleID = apprecord.BundleIdentifier;
                    break;

            }
            if (string.IsNullOrWhiteSpace(bundleID))
                return null;

            // the login info holds GC info
            bool fromVendor = false;
            // gamecenter users (vendor or not) use the username as email
            string username = login.Username;
            var lowerusername = username == null ? "" : username.ToLowerInvariant();
            IUser user = null;
            UserProfilePart profile = null;

            if (string.IsNullOrWhiteSpace(login.Salt) || string.IsNullOrWhiteSpace(login.Signature))
            {
                // not a gc user
                fromVendor = true;
            }
            else if (!ValidateSignature(login, bundleID))
            { 
                // game center user with wrong credentials
                return null;
            }


            
            if (!fromVendor)
            {
                // search by username
                user = _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.NormalizedUserName == username).List().FirstOrDefault();
                if(user == null)
                {
                    // new GC user, check if it was already a VendorID
                    user = _profileService.GetUserForOrphanedVendorId(login.VendorID);
                    if (user != null)
                    {
                        // and update credential info
                        user.As<UserPart>().UserName = username;
                        user.As<UserPart>().NormalizedUserName = lowerusername;
                    }
                }
            }
            else
            {
                // check if vendor user already exists
                user = _profileService.GetUserForOrphanedVendorId(login.VendorID);
                // if yes, we should login with hash ONLY
                if(user!=null)
                    return null;
            }            

            
            if (user == null) // new vendorid & new gcid
            {             
                // since everything is correct, we have to create a new user
                var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();
                if (registrationSettings.UsersCanRegister)
                {
                    // create a user with random password
                    user = _membershipService.CreateUser(new CreateUserParams(username, Guid.NewGuid().ToString(), string.Empty, null, null, true)) as UserPart;
                    // add profile fields
                    profile = user.As<UserProfilePart>();
                    if (!fromVendor)
                    {
                        profile.GCuserid = username;
                        profile.GCalias = login.Name;
                    }

                }
            }
            else
            {
                newUser = false;
                profile = user.As<UserProfilePart>();
                profile.GCuserid = username;
                profile.GCalias = login.Name;
            }
            profile.VendorID = login.VendorID;
            profile.Platform = login.Platform;
            profile.Model = login.Model;
            profile.SystemName = login.SystemName;
            profile.SystemVersion = login.SystemVersion;

            Hash = _loginsService.CreateHash(profile, apprecord);
            _profileService.CreateUserForApplicationRecord(profile, apprecord);
            _loginsService.SetSessionAppId(apprecord.Id);
            return user;
        }

        [AlwaysAccessible]
        [HttpPost]
        public HttpResponseMessage LoginGameCenter(LoginGC login)
        {
            string Hash = string.Empty;
            bool newUser;

            if(string.IsNullOrWhiteSpace(login.Platform))
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new uError("Platform not specified", 417));
            }
            else
            {
                if (!LoginGC.Platforms.Any(login.Platform.Contains))
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new uError("Wrong platform specified", 417));
                }
            }
            if (string.IsNullOrWhiteSpace(login.VendorID))
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new uError("Vendor ID not specified", 417));
            }
            string updateUrl = string.Empty;
            if(!ValidateLogonBuild(login, out updateUrl))
            {
                return Request.CreateResponse(HttpStatusCode.Redirect, new uError(updateUrl, 302));
            }
            IUser user = ValidateLogonGameCenter(login, out Hash, out newUser);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
            }

            _authenticationService.SignIn(user, false);
            _userEventHandler.LoggedIn(user);

            Profile profile = new Profile(user, Request, Hash, newUser);
            return Request.CreateResponse(HttpStatusCode.OK, profile);                            
        }

        #endregion

        #region Facebook
        private IUser ValidateLogonFacebook(LoginFB login, out string Hash, out bool newUser)
        {
            Hash = string.Empty;
            newUser = true;
            ApplicationRecord apprecord = _applicationsService.GetApplicationByKey(login.ApiKey);
            if (apprecord == null)
                return null;           // wrong cloudbast application id

            DebugFB debuginfo = FBHelper.GetDebugInfo(login.Token, apprecord);
            if (!debuginfo.isValid)
                return null;           // access token is not valid
            if (debuginfo.Application != apprecord.Name || debuginfo.AppId != apprecord.fbAppKey)
                return null;           // access token for another application

            string email = login.Username;
            var lowerEmail = email == null ? "" : email.ToLowerInvariant();

            // load user with FBemail
            IUser user = _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.Email == lowerEmail).List().FirstOrDefault();
            UserProfilePart profile = null;
            if (user == null)
            {
                var fb = new FacebookClient(login.Token);
                dynamic me = fb.Get("me");

                // since everything is correct, we have to create a new user
                var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();
                if (registrationSettings.UsersCanRegister)
                {
                    // create a user with random password
                    user = _membershipService.CreateUser(new CreateUserParams(lowerEmail, Guid.NewGuid().ToString(), lowerEmail, null, null, true)) as UserPart;

                    // add facebook fields
                    profile = user.As<UserProfilePart>();
                    profile.FBemail = lowerEmail;
                    profile.FBtoken = login.Token;
                    profile.FirstName = me.first_name;
                    profile.LastName = me.last_name;
                }
            }
            else
            {
                profile = user.As<UserProfilePart>();
                profile.FBemail = lowerEmail;
                profile.FBtoken = login.Token;
                newUser = false;
            }
            Hash = _loginsService.CreateHash(profile, apprecord);
            _profileService.CreateUserForApplicationRecord(profile, apprecord);
            _loginsService.SetSessionAppId(apprecord.Id);
            return user;
        }

        [AlwaysAccessible]
        [HttpPost]
        public HttpResponseMessage LoginFacebook(LoginFB login)
        {
            string Hash = string.Empty;
            bool newUser;

            IUser user = ValidateLogonFacebook(login, out Hash, out newUser);

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
            }

            _authenticationService.SignIn(user, false);
            _userEventHandler.LoggedIn(user);

            Profile profile = new Profile(user, Request, Hash, newUser);
            return Request.CreateResponse(HttpStatusCode.OK, profile);
        }

        #endregion

        [AlwaysAccessible]
        [HttpPost]
        public HttpResponseMessage Login(Login login)
        {
            bool IsNewUser = false;

            _userEventHandler.LoggingIn(login.Username, string.IsNullOrWhiteSpace(login.Hash) ? login.Password : login.Hash);

            IUser user = _orchardServices.WorkContext.CurrentUser;

            ApplicationRecord apprecord = _applicationsService.GetApplicationByKey(login.ApiKey);
            if (apprecord == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            string reason = string.Empty;
            bool clientNeedsUpdate = false;
            if (user != null)
            {
                IUser newUser = ValidateLogOn(login, out reason, out clientNeedsUpdate);
                if (newUser != null && newUser.Id == user.Id)
                {
                    if (_profileService.IsUserInApplication(user.As<UserProfilePart>(), apprecord))
                    {
                        Profile profile = new Profile(user, Request, _loginsService.GetHash(user.As<UserProfilePart>(), apprecord), IsNewUser);
                        _loginsService.SetSessionAppId(apprecord.Id);
                        return Request.CreateResponse(HttpStatusCode.OK, profile);
                    }
                }
                else
                {
                    LogOut();
                }
            }
            user = ValidateLogOn(login, out reason, out clientNeedsUpdate);
            if (user != null)
            {
                UserProfilePart profilePart = user.As<UserProfilePart>();
                if (!_profileService.IsUserInApplication(user.As<UserProfilePart>(), apprecord))
                    IsNewUser = true;

                // ensure that user is in default roles
                _profileService.CreateUserForApplicationRecord(profilePart, apprecord);
                _authenticationService.SignIn(user, false);
                _userEventHandler.LoggedIn(user);
                string newHash = login.Hash;
                if (string.IsNullOrWhiteSpace(newHash))
                {
                    if(!IsNewUser)
                        newHash = _loginsService.GetHash(profilePart, apprecord);
                    if (string.IsNullOrWhiteSpace(newHash)) newHash = _loginsService.CreateHash(profilePart, apprecord);
                }
                Profile profile = new Profile(user, Request, newHash, IsNewUser);
                _loginsService.SetSessionAppId(apprecord.Id);
                return Request.CreateResponse(HttpStatusCode.OK, profile);
            }
            _loginsService.ClearSessionAppId();
            return Request.CreateResponse(clientNeedsUpdate ? HttpStatusCode.UpgradeRequired : HttpStatusCode.Unauthorized, new uError(string.IsNullOrWhiteSpace(reason) ? "User not authorized" : reason, 401));
        }

        [AlwaysAccessible]
        [HttpGet]
        public HttpResponseMessage LogOut(string Hash = null)
        {
            var wasLoggedInUser = _authenticationService.GetAuthenticatedUser();
            _authenticationService.SignOut();
            _loginsService.ClearSessionAppId();
            if (wasLoggedInUser != null)
            {
                if (Hash != null)
                {
                    _loginsService.DeleteHash(Hash);
                }
                _userEventHandler.LoggedOut(wasLoggedInUser);
            }


            return Request.CreateResponse(HttpStatusCode.OK, "User succesfully logged out");
        }

        [HttpGet]
        // v1/user/time
        public HttpResponseMessage PingUser()
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;
            if (user != null)
            {
                // check session variable
                int aid = _loginsService.GetSessionAppId(user);
                if (aid > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, _loginsService.GetServerTime().ToString("yyyy-MM-dd'T'HH:mm:ssZ", CultureInfo.InvariantCulture));
                else
                    return LogOut();
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
        }

        [AlwaysAccessible]
        [HttpGet]
        // v1/ping
        public HttpResponseMessage Ping()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _loginsService.GetServerTime().ToString("yyyy-MM-dd'T'HH:mm:ssZ", CultureInfo.InvariantCulture));
        }

        [HttpGet]
        // v1/user/ping
        public HttpResponseMessage sPing()
        {
            IUser user = _loginsService.CheckUser();
            if (user != null)
            {
                
                var mkey = CBSignals.SignalServer;
                int[] serverData = _cacheManager.Get(mkey, ctx =>
                {
                    ctx.Monitor(_signals.When(mkey));
                    int aid = _loginsService.GetSessionAppId(user);

                    return new[] { _applicationsService.GetApplication(aid).ServerBuild, _applicationsService.GetApplication(aid).MinimumClientBuild };
                });

                return Request.CreateResponse(HttpStatusCode.OK, new Server(_loginsService.GetServerTime(), serverData[0], serverData[1]));
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
        }

        public IUser GetUserByMail(string email)
        {
            var lowerName = email == null ? "" : email.ToLowerInvariant();

            return _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.Email == lowerName).List().FirstOrDefault();
        }
        private IUser ValidateLogOn(Login login, out string reason, out bool requiresUpdate)
        {
            bool validate = true;
            IUser user = null;
            reason = string.Empty;
            requiresUpdate = false;

            if (string.IsNullOrWhiteSpace(login.Platform))
            {
                reason = "No platform specified.";
                return null;
            }
            else
            {
                if (!OData.Profile.Login.Platforms.Any(login.Platform.Contains))
                {
                    reason = "Platform is not supported.";
                    return null;
                }
            }
            if (string.IsNullOrWhiteSpace(login.VendorID))
            {
                reason = "No vendor ID specified.";
                return null;
            }
            string updateUrl = ValidateLogonBuild(login);
            if (!string.IsNullOrEmpty(updateUrl))
            {
                reason = updateUrl;
                requiresUpdate = true;
                return null;
            }


            if (!string.IsNullOrWhiteSpace(login.Hash))
            {
                string hashReason;
                user = _loginsService.ValidateHash(login.Hash, login.ApiKey, out hashReason);
                if (user == null)
                    reason = hashReason;
                return user; 
            }

            if (String.IsNullOrEmpty(login.Username))
            {
                validate = false;
            }
            if (String.IsNullOrEmpty(login.Password))
            {
                validate = false;
            }

            if (!validate)
                return null;

            user = _membershipService.ValidateUser(login.Username, login.Password);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        private int MinPasswordLength
        {
            get
            {
                return _membershipService.GetSettings().MinRequiredPasswordLength;
            }
        }

        [HttpPost]
        [AlwaysAccessible]
        public HttpResponseMessage RegisterChallenge(RegisterConfirmation confirmation)
        {
            string appKey = null;
            string description = string.Empty;
            var user = _profileService.ValidateChallenge(confirmation.nonce, out appKey, out description);

            if (user != null)
            {
                _userEventHandler.ConfirmedEmail(user);
                return Request.CreateResponse(HttpStatusCode.OK, new uError("OK", 200));
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError, new uError(description + ":" + confirmation.nonce, 500));
        }

        [HttpPost]
        [AlwaysAccessible]
        [System.Web.Mvc.ValidateInput(false)]
        public HttpResponseMessage Register(Register Register)
        {
            // ensure users can register
            var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();
            if (!registrationSettings.UsersCanRegister)
            {
                return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, new uError("Method Not Allowed", 405));
            }

            if (Register.Password.Length < MinPasswordLength)
            {
                return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, new uError("Method Not Allowed", 405));
            }

            if (!_profileService.VerifyUserUnicity(Register.Email, Register.Email))
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, new uError("Conflict on the Server", 409));
            }
            ApplicationRecord apprecord = _applicationsService.GetApplicationByKey(Register.ApiKey);
            if (apprecord == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            if (ValidateRegistration(Register))
            {
                // Attempt to register the user
                // No need to report this to IUserEventHandler because _membershipService does that for us
                var user = _membershipService.CreateUser(new CreateUserParams(Register.Email, Register.Password, Register.Email, null, null, false));

                if (user != null)
                {
                    UserProfilePart profile = user.As<UserProfilePart>();
                    if (profile != null)
                    {
                        profile.FirstName = Register.FirstName;
                        profile.LastName = Register.LastName;
                    }
                    if (user.As<UserPart>().EmailStatus == UserStatus.Pending)
                    {
                        var siteUrl = _orchardServices.WorkContext.CurrentSite.BaseUrl;
                        var _Url = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);

                        _profileService.SendChallengeMail(
                            apprecord, 
                            user.As<UserPart>(),
                            nonce =>

                                 _Url.MakeAbsolute(
                                    _Url.Action("ChallengeEmail", "Email", new 
                                        {
                                            Area = "CloudBust.Application", 
                                            nonce = nonce
                                        }, 
                                        "https"
                                    )
                                )
                            );
                        _userEventHandler.SentChallengeEmail(user);
                        return Request.CreateResponse(HttpStatusCode.Created, new uError("Create", 201,false));
                    }

                    if (user.As<UserPart>().RegistrationStatus == UserStatus.Pending)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotModified, new uError("Not Modified", 304));
                    }

                    _authenticationService.SignIn(user, false);
                    return Request.CreateResponse(HttpStatusCode.OK, new uError("OK", 200));
                }

                return Request.CreateResponse(HttpStatusCode.InternalServerError, new uError("Internal Server Error", 500));
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError, new uError("Internal Server Error", 500));
        }
        private string protocolChallengeEmail(string nonce)
        {
            return "alterniity://challenge?nonce="+nonce;
        }
        private bool ValidateRegistration(Register Register)
        {
            bool validate = true;

            if (String.IsNullOrEmpty(Register.Email))
            {     
                validate = false;
            }
            else if (Register.Email.Length >= 255)
            {
                validate = false;
            }
            else if (!Regex.IsMatch(Register.Email, UserPart.EmailPattern, RegexOptions.IgnoreCase))
            {
                validate = false;
            }
            if (Register.Password == null || Register.Password.Length < MinPasswordLength)
            {
                validate = false;
            }
            if (!validate)
                return false;

            return true;
        }

    }
}