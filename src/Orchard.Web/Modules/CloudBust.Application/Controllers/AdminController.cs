using System.Web.Mvc;
using CloudBust.Application.Models;
using CloudBust.Application.Services;
using CloudBust.Application.ViewModels;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;

namespace CloudBust.Application.Controllers
{
    [OrchardFeature("CloudBust.Application.WebApp")]
    //[OrchardSuppressDependency("Orchard.Users.Controllers.AccountController")]
    [HandleError]
    public class AdminController : Controller {
        private readonly IAuthenticationService _authenticationService;
        private readonly IProfileService _profileService;

        public AdminController(
            IProfileService profileService,
            IAuthenticationService authenticationService
        ) {
            _profileService = profileService;
            _authenticationService = authenticationService;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public ActionResult AcceptInvitation(int id) {
            var currentUser = _authenticationService.GetAuthenticatedUser();
            if (currentUser == null)
                return HttpNotFound();
            UserProfilePart friend = null;
            UserProfilePart inviter = null;
            var appkey = string.Empty;
            if (!_profileService.AcceptInvitation(id, currentUser, out inviter, out friend, out appkey) || friend == null)
                return InvitationExpired();
            var viewModel = new AcceptInvitationViewModel {
                user = inviter,
                friend = friend,
                ApplicationKey = appkey
            };

            return View(viewModel);
        }

        public ActionResult InvitationExpired() {
            return View("InvitationExpired");
        }
    }
}