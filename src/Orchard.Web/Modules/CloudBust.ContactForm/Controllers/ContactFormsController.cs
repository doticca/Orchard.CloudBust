using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web.Mvc;
using CloudBust.ContactForm.Services;
using CloudBust.ContactForm.ViewModels;
using Orchard;
using Orchard.AntiSpam.Models;
using Orchard.AntiSpam.ViewModels;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using Orchard.Services;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace CloudBust.ContactForm.Controllers {
    [Themed]
    public class ContactFormsController : Controller {
        private readonly IOrchardServices _orchardServices;
        private readonly IContactFormService _contactFormService;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly INotifier _notifier;
        private readonly IJsonConverter _jsonConverter;
        private const string ReCaptchaSecureUrl = "https://www.google.com/recaptcha/api/siteverify";

        public ContactFormsController(IContactFormService contactFormService, IOrchardServices orchardServices, IWorkContextAccessor workContextAccessor, INotifier notifier, IJsonConverter jsonConverter) {
            _contactFormService = contactFormService;

            _orchardServices = orchardServices;
            _workContextAccessor = workContextAccessor;
            _notifier = notifier;
            _jsonConverter = jsonConverter;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        [HttpPost]
        [ActionName("Add")]
        [AlwaysAccessible]
        public ActionResult AddPost(ContactFormsAddViewModel viewModel) {
            var workContext = _workContextAccessor.GetContext();
            var settings = workContext.CurrentSite.As<ReCaptchaSettingsPart>();

            var result = ExecuteValidateRequest(
                settings.PrivateKey,
                Request.ServerVariables["REMOTE_ADDR"],
                Request.Form["g-recaptcha-response"]
            );
            var responseModel = _jsonConverter.Deserialize<ReCaptchaPartResponseModel>(result);
            if (!responseModel.Success) {
                foreach (var errorCode in responseModel.ErrorCodes) {
                    if (errorCode == "missing-input-response") {
                        _notifier.Error(T("Please prove that you are not a bot."));
                        ModelState.AddModelError("ReCaptcha",
                            T("Please prove that you are not a bot.", viewModel.Email).Text);
                    }
                    else {
                        Logger.Information("An error occurred while submitting a reCaptcha: " + errorCode);
                        _notifier.Error(T("An error occurred while submitting a reCaptcha."));
                        ModelState.AddModelError("ReCaptcha",
                            T("An error occurred while submitting a reCaptcha: ", errorCode).Text);
                    }
                }
            }

            if (!ModelState.IsValid) {
                //if (string.IsNullOrWhiteSpace(viewModel.RetUrl)) {
                //    return View(viewModel);
                //}

                //return Redirect(viewModel.RetUrl);
                return Redirect("/");
            }

            try {
                _contactFormService.CreateContactForm(viewModel.Email, viewModel.Name, viewModel.Company, viewModel.Note);

                _orchardServices.Notifier.Information(T("Contact form for '{0}' was created successfully.", viewModel.Name));
                if (string.IsNullOrWhiteSpace(viewModel.RetUrl)) {
                    return RedirectToAction("Index", "Admin");
                }

                return Redirect(viewModel.RetUrl);
            }
            catch (Exception ex) {
                Logger.Error(ex, "Error while creating contact form.");
                _orchardServices.Notifier.Error(T("Contact form creation failed with error: {0}.", ex.Message));
                if (string.IsNullOrWhiteSpace(viewModel.RetUrl)) {
                    return View(viewModel);
                }

                return Redirect(viewModel.RetUrl);
            }
        }

        private static string ExecuteValidateRequest(string privateKey, string remoteip, string response) {
            var postData = String.Format(CultureInfo.InvariantCulture,
                "secret={0}&response={1}&remoteip={2}",
                privateKey,
                response,
                remoteip
            );

            WebRequest request = WebRequest.Create(ReCaptchaSecureUrl + "?" + postData);
            request.Method = "GET";
            request.Timeout = 5000; //milliseconds
            request.ContentType = "application/x-www-form-urlencoded";

            using (WebResponse webResponse = request.GetResponse()) {
                using (var reader = new StreamReader(webResponse.GetResponseStream())) {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}