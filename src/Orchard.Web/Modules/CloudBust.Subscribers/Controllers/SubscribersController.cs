using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web.Mvc;
using CloudBust.Subscribers.Services;
using CloudBust.Subscribers.ViewModels;
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

namespace CloudBust.Subscribers.Controllers {
    [Themed]
    public class SubscribersController : Controller {
        private readonly IOrchardServices _orchardServices;
        private readonly ISubscriberService _subscriberService;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly INotifier _notifier;
        private readonly IJsonConverter _jsonConverter;
        private const string ReCaptchaSecureUrl = "https://www.google.com/recaptcha/api/siteverify";

        public SubscribersController(ISubscriberService subscriberService, IOrchardServices orchardServices, IWorkContextAccessor workContextAccessor, IJsonConverter jsonConverter, INotifier notifier) {
            _subscriberService = subscriberService;

            _orchardServices = orchardServices;
            _workContextAccessor = workContextAccessor;
            _jsonConverter = jsonConverter;
            _notifier = notifier;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        [HttpPost]
        [ActionName("Add")]
        [AlwaysAccessible]
        public ActionResult AddPost(SubscribersAddViewModel viewModel) {
            if (string.IsNullOrWhiteSpace(viewModel.Email)) {
                ModelState.AddModelError("Email",
                    T("You need to specify an email for the new subscriber.", viewModel.Email).Text);
            }

            if (_subscriberService.GetSubscriber(viewModel.Email) != null) {
                ModelState.AddModelError("Email",
                    T("A subscriber with the same name already exists.", viewModel.Email).Text);
            }

            if (!ModelState.IsValid) {
                if (string.IsNullOrWhiteSpace(viewModel.RetUrl)) {
                    return View(viewModel);
                }

                return Redirect(viewModel.RetUrl);
            }

            try {
                _subscriberService.CreateSubscriber(viewModel.Email);

                _orchardServices.Notifier.Information(T("Subscriber '{0}' was created successfully.", viewModel.Email));
                if (string.IsNullOrWhiteSpace(viewModel.RetUrl)) {
                    return RedirectToAction("Index", "Admin");
                }

                return Redirect(viewModel.RetUrl);
            }
            catch (Exception ex) {
                Logger.Error(ex, "Error while creating subscriber.");
                _orchardServices.Notifier.Error(T("Subscriber creation failed with error: {0}.", ex.Message));
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