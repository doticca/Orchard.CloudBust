using System;
using System.Web.Mvc;
using CloudBust.Subscribers.Services;
using CloudBust.Subscribers.ViewModels;
using Orchard;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace CloudBust.Subscribers.Controllers {
    [Admin]
    public class AdminController : Controller {
        private readonly IOrchardServices _orchardServices;
        private readonly ISubscriberService _subscriberService;

        public AdminController(ISubscriberService subscriberService, IOrchardServices orchardServices) {
            _subscriberService = subscriberService;

            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public ActionResult Add() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage subscribers."))) {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new SubscribersAddViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Add")]
        public ActionResult AddPost(SubscribersAddViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage subscribers."))) {
                return new HttpUnauthorizedResult();
            }

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
                    return RedirectToAction("Index");
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

        [HttpPost]
        public ActionResult Delete(int id) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to delete subscribers."))) {
                return new HttpUnauthorizedResult();
            }

            var subscriberRecord = _subscriberService.GetSubscriber(id);

            if (subscriberRecord != null) {
                // we need to do extra check if currency is in use
                _subscriberService.DeleteSubscriber(id);
                _orchardServices.Notifier.Information(T("Subscriber '{0}' was deleted successfully.",
                    subscriberRecord.Email));
            }

            return RedirectToAction("Index");
        }

        [OutputCache(Duration = 0)]
        public ActionResult Edit(int id) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage Subscribers."))) {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new SubscribersEditViewModel();

            var subscriberRecord = _subscriberService.GetSubscriber(id);
            if (subscriberRecord == null) {
                return RedirectToAction("Index");
            }

            viewModel.Id = subscriberRecord.Id;
            viewModel.Email = subscriberRecord.Email;

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(SubscribersEditViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage subscribers."))) {
                return new HttpUnauthorizedResult();
            }

            var subscriberRecord = _subscriberService.GetSubscriber(viewModel.Id);

            if (subscriberRecord == null) {
                return HttpNotFound();
            }

            var existing = _subscriberService.GetSubscriber(viewModel.Email);
            if (existing != null && existing.Id != subscriberRecord.Id) {
                ModelState.AddModelError("Name", T("A subscriber with that Email already exists.").Text);
            }

            if (!ModelState.IsValid) {
                return View(viewModel);
            }

            try {
                _subscriberService.UpdateSubscriber(viewModel.Id, viewModel.Email);
                _orchardServices.Notifier.Information(T("Changes to subscriber '{0}' were commited successfully.",
                    subscriberRecord.Email));

                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                Logger.Error(ex, "Error while editing subscriber.");
                _orchardServices.Notifier.Error(T("Failed to edit subscriber: {0} ", ex.Message));
                return View(viewModel);
            }
        }

        public ActionResult Index() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage subscribers."))) {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new SubscribersIndexViewModel {
                Subscribers = _subscriberService.GetSubscribers()
            };

            return View(viewModel);
        }
    }
}