using System;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;
using Orchard.Localization;
using System.Web.Mvc;
using System.Web.Security;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.Themes;
using Orchard.Users.Services;
using Orchard.ContentManagement;
using Orchard.Users.Models;
using Orchard.UI.Notify;
using Orchard.Users.Events;
using CloudBust.Application.Services;
using CloudBust.Common.Services;
using CloudBust.Application.Models;
using Orchard;
using Orchard.Environment.Extensions;
using CloudBust.Application.ViewModels;
using Orchard.Utility.Extensions;

namespace CloudBust.Application.Controllers
{
    [OrchardFeature("CloudBust.Application.WebApp")]
    //[OrchardSuppressDependency("Orchard.Users.Controllers.AccountController")]
    [HandleError, Themed]
    public class AccountController : Controller {
        private readonly IProfileService _profileService;
        private readonly IUserEventHandler _userEventHandler;
        private readonly IDetectMobileService _detectMobileService;
        private readonly IApplicationsService _applicationsService;
        private readonly Services.ISettingsService _settingsService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMembershipService _membershipService;
        private readonly IUserService _userService;
        private readonly IOrchardServices _orchardServices;
        private readonly ILoginsService _loginsService;
        public AccountController(
            IUserEventHandler userEventHandler,
            IProfileService profileService,
            IDetectMobileService detectMobileService,
            IApplicationsService applicationsService,
            IAuthenticationService authenticationService,
            Services.ISettingsService settingsService,
            IMembershipService membershipService,
            IUserService userService,
            IOrchardServices orchardServices,
            ILoginsService loginsService
            )
        {
            _userEventHandler = userEventHandler;
            _profileService = profileService;
            _detectMobileService = detectMobileService;
            _applicationsService = applicationsService;
            _authenticationService = authenticationService;
            _membershipService = membershipService;
            _userService = userService;
            _orchardServices = orchardServices;
            _settingsService = settingsService;
            _loginsService = loginsService;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }
        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        [AlwaysAccessible]
        public ActionResult AccessDenied()
        {
            var returnUrl = Request.QueryString["ReturnUrl"];
            var currentUser = _authenticationService.GetAuthenticatedUser();

            if (currentUser == null)
            {
                Logger.Information("Access denied to anonymous request on {0}", returnUrl);
                var shape = _orchardServices.New.LogOn().Title(T("Access Denied").Text);
                return new ShapeResult(this, shape);
            }

            //TODO: (erikpo) Add a setting for whether or not to log access denieds since these can fill up a database pretty fast from bots on a high traffic site
            //Suggestion: Could instead use the new AccessDenined IUserEventHandler method and let modules decide if they want to log this event?
            Logger.Information("Access denied to user #{0} '{1}' on {2}", currentUser.Id, currentUser.UserName, returnUrl);

            _userEventHandler.AccessDenied(currentUser);

            return View();
        }
        int MinPasswordLength
        {
            get
            {
                return _membershipService.GetSettings().MinRequiredPasswordLength;
            }
        }
        private ShapeResult LogOnShape()
        {
            var shape = _orchardServices.New.LogOn().Title(T("Log On").Text);
            return new ShapeResult(this, shape);
        }

        [AlwaysAccessible]
        public ActionResult LogOn(string returnUrl)
        {
            IUser user = _authenticationService.GetAuthenticatedUser();
            ApplicationRecord apprecord = _settingsService.GetWebApplication();
            if (user != null && apprecord!=null)
            {
                if (_profileService.IsUserInApplication(user.As<UserProfilePart>(), apprecord))
                {
                    _loginsService.SetSessionAppId(apprecord.Id);
                    return this.RedirectLocal(returnUrl);
                }
            }
            return LogOnShape();
        }

        [HttpPost]
        [AlwaysAccessible]
        [System.Web.Mvc.ValidateInput(false)]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userNameOrEmail, string password, string returnUrl, bool rememberMe = false)
        {
            bool IsNewUser = false;

            _userEventHandler.LoggingIn(userNameOrEmail, password);

            IUser user = _orchardServices.WorkContext.CurrentUser;

            ApplicationRecord apprecord = _settingsService.GetWebApplication();
            //if (apprecord == null)
            //    return LogOnShape();
            if (user != null)
            {
                IUser newUser = ValidateLogOn(userNameOrEmail, password);
                if (newUser != null && newUser.Id == user.Id)
                {
                    if (_profileService.IsUserInApplication(user.As<UserProfilePart>(), apprecord))
                    {
                        _loginsService.SetSessionAppId(apprecord.Id);
                        return this.RedirectLocal(returnUrl);
                    }
                }
                else
                {
                    LogOut();
                }
            }
            user = ValidateLogOn(userNameOrEmail, password);

            if (user != null)
            {
                UserProfilePart profilePart = user.As<UserProfilePart>();
                if (!_profileService.IsUserInApplication(user.As<UserProfilePart>(), apprecord))
                    IsNewUser = true;

                // ensure that user is in default roles
                if (apprecord!=null)
                    _profileService.CreateUserForApplicationRecord(profilePart, apprecord);

                _authenticationService.SignIn(user, rememberMe);
                _userEventHandler.LoggedIn(user);
                if (apprecord != null)
                {
                    string newHash = string.Empty;
                    if (!IsNewUser) newHash = _loginsService.GetHash(profilePart, apprecord);
                    if (string.IsNullOrWhiteSpace(newHash)) newHash = _loginsService.CreateHash(profilePart, apprecord);
                    _loginsService.SetSessionAppId(apprecord.Id);
                }
                return this.RedirectLocal(returnUrl);
            }
            _loginsService.ClearSessionAppId();
            return LogOnShape();
        }
        private void LogOut(string Hash = null)
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
        }

        public ActionResult LogOff(string returnUrl)
        {
            LogOut();

            return this.RedirectLocal(returnUrl);
        }
        [AlwaysAccessible]
        public ActionResult Register()
        {
            // ensure users can register
            var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();
            if (!registrationSettings.UsersCanRegister)
            {
                return HttpNotFound();
            }

            ViewData["PasswordLength"] = MinPasswordLength;

            var shape = _orchardServices.New.Register();
            return new ShapeResult(this, shape);
        }
        [HttpPost]
        [AlwaysAccessible]
        [ValidateInput(false)]
        public ActionResult Register(string firstname, string lastname, string email, string password, string returnUrl = null)
        {
            // ensure users can register
            var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();
            if (!registrationSettings.UsersCanRegister)
            {
                return HttpNotFound();
            }

            ViewData["PasswordLength"] = MinPasswordLength;

            bool verified = true;
            if (String.IsNullOrEmpty(firstname))
            {
                ModelState.AddModelError("firstname", T("You must specify your First Name."));
                verified = false;
            }

            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", T("You must specify an email."));
                verified = false;
            }
            else
            {
                if (email.Length >= UserPart.MaxUserNameLength || email.Length >= UserPart.MaxEmailLength)
                {
                    ModelState.AddModelError("email", T("The email you provided is too long."));
                    verified = false;
                }
                else if (!Regex.IsMatch(email, UserPart.EmailPattern, RegexOptions.IgnoreCase))
                {
                    // http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx    
                    ModelState.AddModelError("email", T("You must specify a valid email address."));
                    verified = false;
                }

            }
            if (password == null || password.Length < MinPasswordLength)
            {
                ModelState.AddModelError("password", T("You must specify a password of {0} or more characters.", MinPasswordLength));
                verified = false;
            }

            if (verified)
            {
                if (!_profileService.VerifyUserUnicity(email, email))
                {
                    ModelState.AddModelError("userExists", T("User with that username and/or email already exists."));
                    verified = false;
                }
            }
            ApplicationRecord apprecord = _settingsService.GetWebApplication();
            if (verified)
            {                
                if (apprecord == null)
                {
                    ModelState.AddModelError("application", T("An internal server error occured. Pleasy try again later."));
                    verified = false;
                }
            }

            if (verified)
            {
                var user = _membershipService.CreateUser(new CreateUserParams(email, password, email, null, null, false));

                if (user != null)
                {
                    UserProfilePart profile = user.As<UserProfilePart>();
                    if (profile != null)
                    {
                        profile.FirstName = firstname;
                        profile.LastName = lastname;
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
                                    Request.Url.Scheme
                                    )
                                )
                            );
                        _userEventHandler.SentChallengeEmail(user);
                        return RedirectToAction("ChallengeEmailSent", "Email", new { ReturnUrl = returnUrl, email = user.Email });
                    }

                    if (user.As<UserPart>().RegistrationStatus == UserStatus.Pending)
                    {
                        return RedirectToAction("RegistrationPending", new { ReturnUrl = returnUrl });
                    }
                    _userEventHandler.LoggingIn(email, password);
                    _authenticationService.SignIn(user, false);
                    _userEventHandler.LoggedIn(user);

                    return this.RedirectLocal(returnUrl);
                }

                ModelState.AddModelError("_FORM", T(ErrorCodeToString(/*createStatus*/MembershipCreateStatus.ProviderError)));
            }

            var shape = _orchardServices.New.Register();
            return new ShapeResult(this, shape);
        }

        public ActionResult RegistrationPending() {
            return View();
        }

        public ActionResult Invitation(string nonce)
        {
            UserProfilePart invitee = null;
            ApplicationRecord application = null;

            var invitation = _profileService.ValidateInvitationChallenge(nonce, out invitee, out application);
            if (invitation == null)
                return View("InvitationExpired");

            var viewModel = new InvitationViewModel();
            viewModel.invitation = invitation;
            viewModel.user = _profileService.Get(invitation.invitationEmail).As<UserProfilePart>();
            viewModel.inviter = _profileService.Get(invitation.UserProfilePartRecord).As<UserPart>();

            var currentUser = _authenticationService.GetAuthenticatedUser();

            if (currentUser == null)
            {
                if(viewModel.user !=null)
                {
                    return View("InvitationLogin", viewModel);
                }
                else { 
                    return View("InvitationRegistration", viewModel);
                }
            }
            else
            {
                if(currentUser.Email != viewModel.invitation.invitationEmail)
                {
                    return View("InvitationError", viewModel);
                }
                else
                { 
                    return View("InvitationUser", viewModel);
                }
            }
        }
        [HttpPost]
        [AlwaysAccessible]
        [ValidateInput(false)]
        public ActionResult RegisterInvitation(string firstname, string lastname, string email, string password, string invitationemail, string returnUrl = null)
        {
            // ensure users can register
            var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();
            if (!registrationSettings.UsersCanRegister)
            {
                return HttpNotFound();
            }

            ViewData["PasswordLength"] = MinPasswordLength;

            bool verified = true;
            if (String.IsNullOrEmpty(firstname))
            {
                ModelState.AddModelError("firstname", T("You must specify your First Name."));
                verified = false;
            }

            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", T("You must specify an email."));
                verified = false;
            }
            else
            {
                if (email.Length >= UserPart.MaxUserNameLength || email.Length >= UserPart.MaxEmailLength)
                {
                    ModelState.AddModelError("email", T("The email you provided is too long."));
                    verified = false;
                }
                else if (!Regex.IsMatch(email, UserPart.EmailPattern, RegexOptions.IgnoreCase))
                {
                    // http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx    
                    ModelState.AddModelError("email", T("You must specify a valid email address."));
                    verified = false;
                }

            }
            if (password == null || password.Length < MinPasswordLength)
            {
                ModelState.AddModelError("password", T("You must specify a password of {0} or more characters.", MinPasswordLength));
                verified = false;
            }

            if (verified)
            {
                if (!_profileService.VerifyUserUnicity(email, email))
                {
                    ModelState.AddModelError("userExists", T("User with that username and/or email already exists."));
                    verified = false;
                }
            }
            ApplicationRecord apprecord = _settingsService.GetWebApplication();
            if (verified)
            {
                if (apprecord == null)
                {
                    ModelState.AddModelError("application", T("An internal server error occured. Pleasy try again later."));
                    verified = false;
                }
            }

            if (verified)
            {
                var user = _membershipService.CreateUser(new CreateUserParams(email, password, email, null, null, false));

                if (user != null)
                {
                    UserProfilePart profile = user.As<UserProfilePart>();
                    if (profile != null)
                    {
                        profile.FirstName = firstname;
                        profile.LastName = lastname;
                    }
                    if (user.As<UserPart>().EmailStatus == UserStatus.Pending)
                    {
                        if(email.ToLowerInvariant() == invitationemail.ToLowerInvariant())
                        {
                            user.As<UserPart>().EmailStatus = UserStatus.Approved;
                        }
                        else
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
                                        Request.Url.Scheme
                                        )
                                    )
                                );
                            _userEventHandler.SentChallengeEmail(user);
                            return RedirectToAction("ChallengeEmailSent", "Email", new { ReturnUrl = returnUrl, email = user.Email });
                        }
                    }

                    if (user.As<UserPart>().RegistrationStatus == UserStatus.Pending)
                    {
                        return RedirectToAction("RegistrationPending", new { ReturnUrl = returnUrl });
                    }
                    _userEventHandler.LoggingIn(email, password);
                    _authenticationService.SignIn(user, false);
                    _userEventHandler.LoggedIn(user);

                    return this.RedirectLocal(returnUrl);
                }

                ModelState.AddModelError("_FORM", T(ErrorCodeToString(/*createStatus*/MembershipCreateStatus.ProviderError)));
            }

            var shape = _orchardServices.New.Register();
            return new ShapeResult(this, shape);
        }

        public ActionResult AcceptInvitation(string nonce)
        {
            var currentUser = _authenticationService.GetAuthenticatedUser();
            if (currentUser == null)
                return HttpNotFound();
            UserProfilePart friend = null;
            UserProfilePart inviter = null;
            string appkey = string.Empty;
            if (!_profileService.AcceptInvitation(nonce, currentUser, out inviter, out friend, out appkey) || friend == null)
                return InvitationNotFound();
            var viewModel = new AcceptInvitationViewModel()
            {
                user = inviter,
                friend = friend
            };

            return View(viewModel);
        }

        public ActionResult InvitationNotFound()
        {
            return View();
        }

        [AlwaysAccessible]
        public ActionResult RequestLostPassword()
        {
            // ensure users can request lost password
            var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();
            if (!registrationSettings.EnableLostPassword)
            {
                return HttpNotFound();
            }

            return View();
        }

        [HttpPost]
        [AlwaysAccessible]
        public ActionResult RequestLostPassword(string username)
        {
            // ensure users can request lost password
            var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();
            if (!registrationSettings.EnableLostPassword)
            {
                return HttpNotFound();
            }

            if (String.IsNullOrWhiteSpace(username))
            {
                ModelState.AddModelError("username", T("You must specify your e-mail."));
                return View();
            }

            var siteUrl = _orchardServices.WorkContext.CurrentSite.BaseUrl;
            if (String.IsNullOrWhiteSpace(siteUrl))
            {
                siteUrl = HttpContext.Request.ToRootUrlString();
            }
            if (username.Length >= UserPart.MaxUserNameLength || username.Length >= UserPart.MaxEmailLength)
            {
                ModelState.AddModelError("username", T("The email you provided is too long."));
                return View();
            }
            else if (!Regex.IsMatch(username, UserPart.EmailPattern, RegexOptions.IgnoreCase))
            {
                ModelState.AddModelError("username", T("You must specify a valid email address."));
                return View();
            }
            if (_profileService.VerifyUserUnicity(username, username))
            {
                ModelState.AddModelError("username", T("This e-mail is not found in our user database. Please check your spelling"));
                return View();
            }
            ApplicationRecord apprecord = _settingsService.GetWebApplication();
            if (apprecord == null)
            {
                ModelState.AddModelError("username", T("An internal server error occured. Pleasy try again later."));
                return View();
            }

            _profileService.SendLostPasswordEmail(
                apprecord,
                username, 
                nonce => 
                    Url.MakeAbsolute(
                        Url.Action("LostPassword", "Account", new { Area = "CloudBust.Application", nonce = nonce }), 
                        siteUrl
                        )
                );

            //_orchardServices.Notifier.Information(T("Check your e-mail for the confirmation link."));

            return View("RequestLostPasswordSent");
        }

        [AlwaysAccessible]
        public ActionResult LostPassword(string nonce)
        {
            string appKey = null;
            if (_profileService.ValidateLostPassword(nonce, out appKey) == null)
            {
                return RedirectToAction("LogOn");
            }

            ViewData["PasswordLength"] = MinPasswordLength;

            return View();
        }
        [HttpPost]
        [AlwaysAccessible]
        [ValidateInput(false)]
        public ActionResult LostPassword(string nonce, string newPassword, string confirmPassword)
        {
            IUser user;
            string appKey = null;
            if ((user = _profileService.ValidateLostPassword(nonce, out appKey)) == null)
            {
                return Redirect("~/");
            }

            ViewData["PasswordLength"] = MinPasswordLength;

            if (newPassword == null || newPassword.Length < MinPasswordLength)
            {
                ModelState.AddModelError("newPassword", T("You must specify a new password of {0} or more characters.", MinPasswordLength));
            }

            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", T("The new password and confirmation password do not match."));
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            _membershipService.SetPassword(user, newPassword);

            _userEventHandler.ChangedPassword(user);

            return RedirectToAction("ChangePasswordSuccess");
        }

        [Authorize]
        [AlwaysAccessible]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MinPasswordLength;

            return View();
        }

        [Authorize]
        [HttpPost]
        [AlwaysAccessible]
        [ValidateInput(false)]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exceptions result in password not being changed.")]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            ViewData["PasswordLength"] = MinPasswordLength;

            if (!ValidateChangePassword(currentPassword, newPassword, confirmPassword))
            {
                return View();
            }

            try
            {
                var validated = _membershipService.ValidateUser(User.Identity.Name, currentPassword);

                if (validated != null)
                {
                    _membershipService.SetPassword(validated, newPassword);
                    _userEventHandler.ChangedPassword(validated);
                    return RedirectToAction("ChangePasswordSuccess");
                }

                ModelState.AddModelError("_FORM",
                                         T("The current password is incorrect or the new password is invalid."));
                return ChangePassword();
            }
            catch
            {
                ModelState.AddModelError("_FORM", T("The current password is incorrect or the new password is invalid."));
                return ChangePassword();
            }
        }

        [AlwaysAccessible]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
        #region validation
        private bool ValidateRegistration(string userName, string email, string password, string confirmPassword)
        {
            bool validate = true;

            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", T("You must specify a username."));
                validate = false;
            }
            else
            {
                if (userName.Length >= UserPart.MaxUserNameLength)
                {
                    ModelState.AddModelError("username", T("The username you provided is too long."));
                    validate = false;
                }
            }

            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", T("You must specify an email address."));
                validate = false;
            }
            else if (email.Length >= UserPart.MaxEmailLength)
            {
                ModelState.AddModelError("email", T("The email address you provided is too long."));
                validate = false;
            }
            else if (!Regex.IsMatch(email, UserPart.EmailPattern, RegexOptions.IgnoreCase))
            {
                // http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx    
                ModelState.AddModelError("email", T("You must specify a valid email address."));
                validate = false;
            }

            if (!validate)
                return false;

            if (!_userService.VerifyUserUnicity(userName, email))
            {
                ModelState.AddModelError("userExists", T("User with that username and/or email already exists."));
            }
            if (password == null || password.Length < MinPasswordLength)
            {
                ModelState.AddModelError("password", T("You must specify a password of {0} or more characters.", MinPasswordLength));
            }
            if (!String.Equals(password, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", T("The new password and confirmation password do not match."));
            }
            return ModelState.IsValid;
        }
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        private bool ValidateChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (String.IsNullOrEmpty(currentPassword))
            {
                ModelState.AddModelError("currentPassword", T("You must specify a current password."));
            }
            if (newPassword == null || newPassword.Length < MinPasswordLength)
            {
                ModelState.AddModelError("newPassword", T("You must specify a new password of {0} or more characters.", MinPasswordLength));
            }

            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", T("The new password and confirmation password do not match."));
            }

            return ModelState.IsValid;
        }
        private IUser ValidateLogOn(string userNameOrEmail, string password)
        {
            bool validate = true;
            IUser user = null;

            if (String.IsNullOrEmpty(userNameOrEmail))
            {
                validate = false;
            }
            if (String.IsNullOrEmpty(password))
            {
                validate = false;
            }

            if (!validate)
                return null;

            user = _membershipService.ValidateUser(userNameOrEmail, password);
            if (user == null)
            {
                return null;
            }

            return user;
        }
        #endregion
    }
}