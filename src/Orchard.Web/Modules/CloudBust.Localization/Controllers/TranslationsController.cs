using System;
using System.Web.Mvc;
using CloudBust.Localization.Services;
using CloudBust.Localization.ViewModels;
using Orchard;
using Orchard.Environment.Configuration;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace CloudBust.Localization.Controllers {
    [Themed]
    public class TranslationsController : Controller {
        public TranslationsController(ITranslationService translationService, IOrchardServices orchardServices, ShellSettings shellSettings) {
            _translationService = translationService;

            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        private readonly IOrchardServices _orchardServices;
        private readonly ITranslationService _translationService;

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        [OutputCache(Duration = 0)]
        public ActionResult Add() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("You don't have permission to manage translations.")))
                return new HttpUnauthorizedResult();

            var viewModel = new TranslationAddViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Add")]
        public ActionResult AddPost(TranslationAddViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("You don't have permission to manage translations."))) return new HttpUnauthorizedResult();

            if (string.IsNullOrWhiteSpace(viewModel.Name)) ModelState.AddModelError("Name", T("You need to specify a name for the new translation.", viewModel.Name).Text);
            if (_translationService.GetTranslation(viewModel.Name) != null) ModelState.AddModelError("Name", T("A translation with the same name already exists.", viewModel.Name).Text);
            if (string.IsNullOrWhiteSpace(viewModel.TwoDigitCode)) ModelState.AddModelError("TwoDigitCode", T("You need to specify a two-digit code for the new translation.", viewModel.TwoDigitCode).Text);
            if (_translationService.GetTranslationByCode(viewModel.TwoDigitCode) != null) ModelState.AddModelError("TwoDigitCode", T("A translation with the same two-digit code already exists.", viewModel.Name).Text);

            if (!ModelState.IsValid) return View(viewModel);

            try {
                _translationService.CreateTranslation(viewModel.Name, viewModel.TwoDigitCode);

                _orchardServices.Notifier.Information(T("Translation '{0}' was created successfully.", viewModel.Name));
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex) {
                Logger.Error(ex, "Error while creating translation.");
                _orchardServices.Notifier.Error(T("Translation creation failed with error: {0}.", ex.Message));
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("You don't have permission to delete translations.")))
                return new HttpUnauthorizedResult();

            var translation = _translationService.GetTranslation(id);

            if (translation != null) {
                // we need to do extra check if translation is in use
                _translationService.DeleteTranslation(id);
                _orchardServices.Notifier.Information(T("Translation '{0}' was deleted successfully.", translation.Name));
            }

            return RedirectToAction("Index");
        }

        [OutputCache(Duration = 0)]
        public ActionResult Edit(int id) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("You don't have permission to manage translations.")))
                return new HttpUnauthorizedResult();

            var viewModel = new TranslationEditViewModel();

            var translation = _translationService.GetTranslation(id);
            if (translation == null) return RedirectToAction("Index");
            viewModel.Id = translation.Id;
            viewModel.Name = translation.Name;
            viewModel.TwoDigitCode = translation.TwoDigitCode;

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(TranslationEditViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("You don't have permission to manage translations.")))
                return new HttpUnauthorizedResult();

            var translation = _translationService.GetTranslation(viewModel.Id);

            if (translation == null)
                return HttpNotFound();

            var existing = _translationService.GetTranslation(viewModel.Name);
            if (existing != null && existing.Id != translation.Id) ModelState.AddModelError("Name", T("A translation with that Name already exists.").Text);
            existing = _translationService.GetTranslationByCode(viewModel.TwoDigitCode);
            if (existing != null && existing.Id != translation.Id) ModelState.AddModelError("ThreeDigitCode", T("A translation with that Code already exists.").Text);
            if (!ModelState.IsValid) return View(viewModel);

            try {
                _translationService.UpdateTranslation(viewModel.Id, viewModel.Name, viewModel.TwoDigitCode);
                _orchardServices.Notifier.Information(T("Changes to translation '{0}' were commited successfully.", translation.Name));
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                Logger.Error(ex, "Error while editing translation.");
                _orchardServices.Notifier.Error(T("Failed to edit translation: {0} ", ex.Message));
                return View(viewModel);
            }
        }

        [OutputCache(Duration = 0)]
        public ActionResult Index() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("You don't have permission to manage translations.")))
                return new HttpUnauthorizedResult();

            var viewModel = new TranslationsIndexViewModel {Translations = _translationService.GetTranslations()};

            return View(viewModel);
        }
    }
}