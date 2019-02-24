using System;
using System.Web;
using System.Web.Security;
using CloudBust.Application.Models;
using CloudBust.Application.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Configuration;
using Orchard.Environment.Extensions;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.Services;
using Orchard.Utility.Extensions;

namespace CloudBust.Application.Providers {
    [OrchardFeature("CloudBust.Application.WebApp")]
    [OrchardSuppressDependency("Orchard.Security.Providers.FormsAuthenticationService")]
    public class FormsAuthenticationService : IAuthenticationService {
        private const int CookieVersion = 3;
        private readonly IClock _clock;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoginsService _loginsService;
        private readonly IMembershipService _membershipService;
        private readonly IMembershipValidationService _membershipValidationService;
        private readonly IProfileService _profileService;
        private readonly IOrchardServices _services;

        private readonly ShellSettings _settings;
        private readonly ISettingsService _settingsService;
        private readonly ISslSettingsProvider _sslSettingsProvider;
        private bool _isAuthenticated;


        // This fixes a performance issue when the forms authentication cookie is set to a
        // user name not mapped to an actual Orchard user content item. If the request is
        // authenticated but a null user is returned, multiple calls to GetAuthenticatedUser
        // will cause multiple DB invocations, slowing down the request. We therefore
        // remember if the current user is a non-Orchard user between invocations.
        private bool _isNonOrchardUser;

        private IUser _signedInUser;

        public FormsAuthenticationService(
            IOrchardServices services,
            ShellSettings settings,
            ISettingsService settingsService,
            IClock clock,
            IMembershipService membershipService,
            IHttpContextAccessor httpContextAccessor,
            ISslSettingsProvider sslSettingsProvider,
            IProfileService profileService,
            IMembershipValidationService membershipValidationService,
            ILoginsService loginsService) {
            _services = services;
            _settings = settings;
            _clock = clock;
            _membershipService = membershipService;
            _httpContextAccessor = httpContextAccessor;
            _sslSettingsProvider = sslSettingsProvider;
            _membershipValidationService = membershipValidationService;
            _settingsService = settingsService;
            _profileService = profileService;
            _loginsService = loginsService;

            Logger = NullLogger.Instance;

            ExpirationTimeSpan = TimeSpan.FromDays(30);
        }

        public ILogger Logger { get; set; }

        public TimeSpan ExpirationTimeSpan { get; set; }

        public IUser GetAuthenticatedUser() {
            if (_isNonOrchardUser) {
                return null;
            }

            if (_signedInUser != null || _isAuthenticated) {
                return _signedInUser;
            }

            var alreadySigned = _signedInUser != null;
            var httpContext = _httpContextAccessor.Current();
            if (httpContext.IsBackgroundContext() || !httpContext.Request.IsAuthenticated ||
                !(httpContext.User.Identity is FormsIdentity)) {
                return null;
            }

            var formsIdentity = (FormsIdentity) httpContext.User.Identity;
            var userData = formsIdentity.Ticket.UserData ?? "";

            // The cookie user data is {userName.Base64};{tenant}.
            var userDataSegments = userData.Split(';');

            if (_settingsService.IsWebApplication() && userDataSegments.Length < 3) {
                return null;
            }

            if (userDataSegments.Length < 2) {
                return null;
            }

            var userDataName = userDataSegments[0];
            var userDataTenant = userDataSegments[1];
            string webAppKey;
            try {
                webAppKey = userDataSegments[2];
            }
            catch {
                webAppKey = string.Empty;
            }

            try {
                userDataName = userDataName.FromBase64();
            }
            catch {
                return null;
            }

            if (!string.Equals(userDataTenant, _settings.Name, StringComparison.Ordinal)) {
                return null;
            }

            var app = _settingsService.GetWebApplication();
            if (app != null && app.AppKey != webAppKey) {
                return null;
            }

            _signedInUser = _membershipService.GetUser(userDataName);
            if (_signedInUser == null || !_membershipValidationService.CanAuthenticateWithCookie(_signedInUser)) {
                _isNonOrchardUser = true;
                return null;
            }

            if (app != null && !_profileService.IsUserInApplication(_signedInUser, app)) {
                if (!string.Equals(_services.WorkContext.CurrentSite.SuperUser, _signedInUser.UserName,
                    StringComparison.Ordinal)) {
                    _isNonOrchardUser = true;
                    return null;
                }

                _profileService.CreateUserForApplicationRecord(_signedInUser.ContentItem.As<UserProfilePart>(), app);
            }
            else {
                if (app != null && !alreadySigned) {
                    _loginsService.SetSessionAppId(app.Id);
                }
            }

            _isAuthenticated = true;
            return _signedInUser;
        }

        public void SetAuthenticatedUserForRequest(IUser user) {
            _signedInUser = user;
            _isAuthenticated = true;
        }

        public void SignIn(IUser user, bool createPersistentCookie) {
            var now = _clock.UtcNow.ToLocalTime();

            // The cookie user data is "{userName.Base64};{tenant}".
            // The username is encoded to Base64 to prevent collisions with the ';' seprarator.
            var userData = string.Concat(user.UserName.ToBase64(), ";", _settings.Name);
            if (_settingsService.IsWebApplication()) {
                userData = string.Concat(user.UserName.ToBase64(), ";", _settings.Name, ";",
                    _settingsService.GetWebApplicationKey());
            }

            var ticket = new FormsAuthenticationTicket(
                CookieVersion,
                user.UserName,
                now,
                now.Add(ExpirationTimeSpan),
                createPersistentCookie,
                userData,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) {
                HttpOnly = true,
                Secure = _sslSettingsProvider.GetRequiresSSL(),
                Path = FormsAuthentication.FormsCookiePath
            };

            var httpContext = _httpContextAccessor.Current();

            if (!string.IsNullOrEmpty(_settings.RequestUrlPrefix)) {
                cookie.Path = GetCookiePath(httpContext);
            }

            if (FormsAuthentication.CookieDomain != null) {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            if (createPersistentCookie) {
                cookie.Expires = ticket.Expiration;
            }

            httpContext.Response.Cookies.Add(cookie);

            _isAuthenticated = true;
            _signedInUser = user;
        }

        public void SignOut() {
            _signedInUser = null;
            _isAuthenticated = false;
            FormsAuthentication.SignOut();

            // overwritting the authentication cookie for the given tenant
            var httpContext = _httpContextAccessor.Current();
            var rFormsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "") {
                Expires = DateTime.Now.AddYears(-1)
            };

            if (!string.IsNullOrEmpty(_settings.RequestUrlPrefix)) {
                rFormsCookie.Path = GetCookiePath(httpContext);
            }

            httpContext.Response.Cookies.Add(rFormsCookie);
        }

        private string GetCookiePath(HttpContextBase httpContext) {
            var cookiePath = httpContext.Request.ApplicationPath;
            if (cookiePath != null && cookiePath.Length > 1) {
                cookiePath += '/';
            }

            cookiePath += _settings.RequestUrlPrefix;

            return cookiePath;
        }
    }
}