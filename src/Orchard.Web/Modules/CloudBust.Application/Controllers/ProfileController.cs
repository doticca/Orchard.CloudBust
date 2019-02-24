using Orchard.Localization;
using System.Web.Mvc;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Themes;
using Orchard.Users.Services;
using Orchard.Users.Events;
using CloudBust.Application.Services;
using CloudBust.Common.Services;
using Orchard;

namespace CloudBust.Application.Controllers
{
    [HandleError, Themed]
    public class ProfileController : Controller {
        private readonly IProfileService _profileService;
        private readonly IUserEventHandler _userEventHandler;
        private readonly IDetectMobileService _detectMobileService;
        private readonly IApplicationsService _applicationsService;
        private readonly Application.Services.ISettingsService _settingsService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMembershipService _membershipService;
        private readonly IUserService _userService;
        private readonly IOrchardServices _orchardServices;
        public ProfileController(
            IUserEventHandler userEventHandler,
            IProfileService profileService,
            IDetectMobileService detectMobileService,
            IApplicationsService applicationsService,
            IAuthenticationService authenticationService,
            Application.Services.ISettingsService settingsService,
            IMembershipService membershipService,
            IUserService userService,
            IOrchardServices orchardServices
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
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }
        public ILogger Logger { get; set; }
        public Localizer T { get; set; }
        
        int MinPasswordLength
        {
            get
            {
                return _membershipService.GetSettings().MinimumPasswordLength;
            }
        }
        public ActionResult Index()
        {
            var currentUser = _authenticationService.GetAuthenticatedUser();

            if (currentUser == null)
            {
                var shape = _orchardServices.New.LogOn().Title(T("Access Denied").Text);
                return new ShapeResult(this, shape);
            }

            return View();
        }
    }
}