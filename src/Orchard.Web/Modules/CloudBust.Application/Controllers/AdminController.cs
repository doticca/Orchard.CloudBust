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
    [HandleError]
    public class AdminController : Controller {
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
        public AdminController(
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

        public ActionResult AcceptInvitation(string nonce)
        {
            var currentUser = _authenticationService.GetAuthenticatedUser();
            if (currentUser == null)
                return HttpNotFound();
            UserProfilePart friend = null;
            UserProfilePart inviter = null;
            string appkey = string.Empty;
            if (!_profileService.AcceptInvitation(nonce, currentUser, out inviter, out friend, out appkey) || friend == null)
                return InvitationExpired();
            var viewModel = new AcceptInvitationViewModel()
            {
                user = inviter,
                friend = friend,
                ApplicationKey = appkey
            };

            return View(viewModel);
        }

        public ActionResult InvitationExpired()
        {
            return View("InvitationExpired");
        }
    }
}