using System;
using System.Web.Mvc;
using CloudBust.ContactForm.Services;
using CloudBust.ContactForm.ViewModels;
using Orchard;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace CloudBust.ContactForm.Controllers {
    [Admin]
    public class AdminController : Controller {
        private readonly IOrchardServices _orchardServices;
        private readonly IContactFormService _contactFormService;

        public AdminController(IContactFormService contactFormService, IOrchardServices orchardServices) {
            _contactFormService = contactFormService;

            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public ActionResult Add() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage contact forms."))) {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new ContactFormsAddViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Add")]
        public ActionResult AddPost(ContactFormsAddViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage contact forms."))) {
                return new HttpUnauthorizedResult();
            }

            if (!ModelState.IsValid) {
                if (string.IsNullOrWhiteSpace(viewModel.RetUrl)) {
                    return View(viewModel);
                }

                return Redirect(viewModel.RetUrl);
            }

            try {
                _contactFormService.CreateContactForm(viewModel.Email, viewModel.Name, viewModel.Company, viewModel.Note);

                _orchardServices.Notifier.Information(T("Contact form for '{0}' was created successfully.", viewModel.Name));
                if (string.IsNullOrWhiteSpace(viewModel.RetUrl)) {
                    return RedirectToAction("Index");
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

        [HttpPost]
        public ActionResult Delete(int id) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to delete contact forms."))) {
                return new HttpUnauthorizedResult();
            }

            var contactFormRecord = _contactFormService.GetContactForm(id);

            if (contactFormRecord != null) {
                // we need to do extra check if currency is in use
                _contactFormService.DeleteContactForm(id);
                _orchardServices.Notifier.Information(T("Contact Form '{0}' was deleted successfully.",
                    contactFormRecord.Id));
            }

            return RedirectToAction("Index");
        }

        [OutputCache(Duration = 0)]
        public ActionResult Edit(int id) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage contact forms."))) {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new ContactFormsEditViewModel();

            var contactFormRecord = _contactFormService.GetContactForm(id);
            if (contactFormRecord == null) {
                return RedirectToAction("Index");
            }

            viewModel.Id = contactFormRecord.Id;
            viewModel.Email = contactFormRecord.Email;
            viewModel.Name = contactFormRecord.Name;
            viewModel.Company = contactFormRecord.Company;
            viewModel.Note = contactFormRecord.Note;

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(ContactFormsEditViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage contact forms."))) {
                return new HttpUnauthorizedResult();
            }

            var contactFormRecord = _contactFormService.GetContactForm(viewModel.Id);

            if (contactFormRecord == null) {
                return HttpNotFound();
            }

            if (!ModelState.IsValid) {
                return View(viewModel);
            }

            try {
                _contactFormService.UpdateContactForm(viewModel.Id, viewModel.Email, viewModel.Name, viewModel.Company, viewModel.Note);
                _orchardServices.Notifier.Information(T("Changes to contact form '{0}' were commited successfully.",
                    contactFormRecord.Id));

                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                Logger.Error(ex, "Error while editing contact form.");
                _orchardServices.Notifier.Error(T("Failed to edit contact form: {0} ", ex.Message));
                return View(viewModel);
            }
        }

        public ActionResult Index() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage contact forms."))) {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new ContactFormsIndexViewModel {
                ContactForms = _contactFormService.GetContactForms()
            };

            return View(viewModel);
        }
    }
}