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
    public class CurrenciesController : Controller {
        private readonly ICurrencyService _currencyService;
        private readonly IOrchardServices _orchardServices;
        private readonly ShellSettings _shellSettings;

        public CurrenciesController(ICurrencyService currencyService, IOrchardServices orchardServices,
            ShellSettings shellSettings) {
            _currencyService = currencyService;
            _shellSettings = shellSettings;

            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        [OutputCache(Duration = 0)]
        public ActionResult Add() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage currencies.")))
                return new HttpUnauthorizedResult();

            var viewModel = new CurrencyAddViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Add")]
        public ActionResult AddPost(CurrencyAddViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage currencies."))) return new HttpUnauthorizedResult();

            if (string.IsNullOrWhiteSpace(viewModel.Name))
                ModelState.AddModelError("Name",
                    T("You need to specify a name for the new currency.", viewModel.Name).Text);
            if (_currencyService.GetCurrency(viewModel.Name) != null)
                ModelState.AddModelError("Name",
                    T("A currency with the same name already exists.", viewModel.Name).Text);
            if (string.IsNullOrWhiteSpace(viewModel.ThreeDigitCode))
                ModelState.AddModelError("ThreeDigitCode",
                    T("You need to specify a three-digit code for the new currency.", viewModel.ThreeDigitCode).Text);
            if (_currencyService.GetCurrencyByCode(viewModel.ThreeDigitCode) != null)
                ModelState.AddModelError("ThreeDigitCode",
                    T("A currency with the same three-digit code already exists.", viewModel.Name).Text);

            if (!ModelState.IsValid) return View(viewModel);

            try {
                _currencyService.CreateCurrency(viewModel.Name, viewModel.ThreeDigitCode);

                _orchardServices.Notifier.Information(T("Currency '{0}' was created successfully.", viewModel.Name));
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                Logger.Error(ex, "Error while creating currency.");
                _orchardServices.Notifier.Error(T("Currency creation failed with error: {0}.", ex.Message));
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to delete currencies.")))
                return new HttpUnauthorizedResult();

            var currency = _currencyService.GetCurrency(id);

            if (currency != null) {
                // we need to do extra check if currency is in use
                _currencyService.DeleteCurrency(id);
                _orchardServices.Notifier.Information(T("Currency '{0}' was deleted successfully.", currency.Name));
            }

            return RedirectToAction("Index");
        }

        [OutputCache(Duration = 0)]
        public ActionResult Edit(int id) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage currencies.")))
                return new HttpUnauthorizedResult();

            var viewModel = new CurrencyEditViewModel();

            var currency = _currencyService.GetCurrency(id);
            if (currency == null) return RedirectToAction("Index");
            viewModel.Id = currency.Id;
            viewModel.Name = currency.Name;
            viewModel.ThreeDigitCode = currency.ThreeDigitCode;
            viewModel.Sign = currency.Sign;
            viewModel.ShowSignAfter = currency.ShowSignAfter;

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(CurrencyEditViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage currencies.")))
                return new HttpUnauthorizedResult();

            var currency = _currencyService.GetCurrency(viewModel.Id);

            if (currency == null)
                return HttpNotFound();

            var existing = _currencyService.GetCurrency(viewModel.Name);
            if (existing != null && existing.Id != currency.Id)
                ModelState.AddModelError("Name", T("A currency with that Name already exists.").Text);
            existing = _currencyService.GetCurrencyByCode(viewModel.ThreeDigitCode);
            if (existing != null && existing.Id != currency.Id)
                ModelState.AddModelError("ThreeDigitCode", T("A currency with that Code already exists.").Text);
            if (!ModelState.IsValid) return View(viewModel);

            try {
                _currencyService.UpdateCurrency(viewModel.Id, viewModel.Name, viewModel.ThreeDigitCode, viewModel.Sign, viewModel.ShowSignAfter);
                _orchardServices.Notifier.Information(T("Changes to currency '{0}' were commited successfully.",
                    currency.Name));

                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                Logger.Error(ex, "Error while editing currency.");
                _orchardServices.Notifier.Error(T("Failed to edit currency: {0} ", ex.Message));
                return View(viewModel);
            }
        }

        [OutputCache(Duration = 0)]
        public ActionResult Index() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage currencies.")))
                return new HttpUnauthorizedResult();

            var viewModel = new CurrenciesIndexViewModel {
                Currencies = _currencyService.GetCurrencies()
            };

            return View(viewModel);
        }
    }
}