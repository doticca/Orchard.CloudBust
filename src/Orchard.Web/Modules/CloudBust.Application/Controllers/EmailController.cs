using CloudBust.Application.Models;
using CloudBust.Application.Services;
using CloudBust.Application.ViewModels;
using CloudBust.Common.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;
using Orchard.Themes;
using Orchard.Users.Events;
using System.Web.Mvc;
using ISettingsService = CloudBust.Application.Services.ISettingsService;

namespace CloudBust.Application.Controllers
{
    [Themed]
    public class EmailController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IUserEventHandler _userEventHandler;
        private readonly IDetectMobileService _detectMobileService;
        private readonly IApplicationsService _applicationsService;
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthenticationService _authenticationService;
        private readonly Services.ISettingsService _settingsService;
        private readonly ILoginsService _loginsService;

        public EmailController(
            IUserEventHandler userEventHandler,
            IProfileService profileService,
            IDetectMobileService detectMobileService,
            IApplicationsService applicationsService, IOrchardServices orchardServices, IAuthenticationService authenticationService, ISettingsService settingsService, ILoginsService loginsService)
        {
            _userEventHandler = userEventHandler;
            _profileService = profileService;
            _detectMobileService = detectMobileService;
            _applicationsService = applicationsService;
            _orchardServices = orchardServices;
            _authenticationService = authenticationService;
            _settingsService = settingsService;
            _loginsService = loginsService;
        }
     
        public ActionResult ChallengeEmailSent(string email)
        {
            return View(new EmailViewModel() { email = email });
        }
        public ActionResult ChallengeEmailSuccess(string email = null, string appkey = null)
        {
            IUser user = _authenticationService.GetAuthenticatedUser();
            ApplicationRecord apprecord = _settingsService.GetWebApplication();
            if (user != null && apprecord!=null)
            {
                if (_profileService.IsUserInApplication(user.As<UserProfilePart>(), apprecord))
                {
                    _loginsService.SetSessionAppId(apprecord.Id);
                    if (user.As<UserProfilePart>().ResetPassword)
                    {
                        return RedirectToAction("ResetPassword", "Account");
                    }
                }
            }



            MobileDetectionViewModel viewModel = new MobileDetectionViewModel();

            viewModel.IsIOS = false;
            viewModel.IsMobile = true;
            viewModel.IsMobileDevice = Request.Browser["IsMobileDevice"];
            viewModel.Platform = Request.Browser["Platform"];
            viewModel.Browser = Request.Browser["Browser"];
            viewModel.MobileDeviceManufacturer = Request.Browser["MobileDeviceManufacturer"];
            viewModel.MobileDeviceModel = Request.Browser["MobileDeviceModel"];
            viewModel.application = appkey;
            viewModel.email = email;
            viewModel.URLLink = string.IsNullOrWhiteSpace(appkey) ? "noappkey" : "cloudbust" + appkey + ":registered?user=" + email;
            if(viewModel.IsMobileDevice.ToLower() == "false")
            {
                viewModel.IsMobile = false;
            }
            else
            {
                if(viewModel.MobileDeviceManufacturer.ToLower() == "apple")
                {
                    viewModel.IsIOS = true;
                    return Redirect(viewModel.URLLink);
                }
            }

            return View("ChallengeEmailSuccess", viewModel);
        }
        public ActionResult ChallengeEmailFail()
        {
            return View();
        }
        public ActionResult ChallengeEmail(string nonce)
        {
            string appKey = null;
            string description = string.Empty;
            var user = _profileService.ValidateChallenge(nonce, out appKey, out description);

            if (user != null)
            {

                _userEventHandler.ConfirmedEmail(user);
                
                ApplicationRecord app = _applicationsService.GetApplicationByKey(appKey);
                if (app == null)
                {
                    return RedirectToAction("ChallengeEmailSuccess");
                }
                else
                {
                    var profilePart = user.As<UserProfilePart>();
                    if (profilePart != null && profilePart.ResetPassword)
                    {
                        _authenticationService.SignIn(user, false);
                        _userEventHandler.LoggedIn(user);
                    }
                    return RedirectToAction("ChallengeEmailSuccess", new { email = user.Email, appkey = app.AppKey });
                }
            }

            return RedirectToAction("ChallengeEmailFail");
        }
    }
}