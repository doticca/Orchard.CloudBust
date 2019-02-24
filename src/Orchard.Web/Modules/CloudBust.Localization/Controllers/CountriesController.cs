using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CloudBust.Localization.Models;
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
    public class CountriesController : Controller {
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly IOrchardServices _orchardServices;
        private readonly ShellSettings _thisShellSettings;
        private readonly ITranslationService _translationService;

        public CountriesController(
            ICountryService countryService,
            IOrchardServices orchardServices,
            ShellSettings shellSettings,
            ICurrencyService currencyService,
            ITranslationService translationService) {
            _countryService = countryService;
            _thisShellSettings = shellSettings;
            _currencyService = currencyService;
            _translationService = translationService;
            _orchardServices = orchardServices;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        [OutputCache(Duration = 0)]
        public ActionResult Add() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage countries.")))
                return new HttpUnauthorizedResult();

            var viewModel = new CountryAddViewModel {Currencies = _currencyService.GetCurrencies()};
            if (viewModel.Currencies == null || !viewModel.Currencies.Any()) {
                _orchardServices.Notifier.Information(T("Please add a Currency before creating a new Country."));
                return RedirectToAction("Index");
            }

            viewModel.Translations = _translationService.GetTranslations();
            if (viewModel.Translations == null || !viewModel.Translations.Any()) {
                _orchardServices.Notifier.Information(T("Please add a Translation before creating a new Country."));
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Add")]
        public ActionResult AddPost(CountryAddViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage countries."))) return new HttpUnauthorizedResult();

            if (string.IsNullOrWhiteSpace(viewModel.Name))
                ModelState.AddModelError("Name", T("You need to specify a name for the new country.").Text);
            if (_countryService.GetCountry(viewModel.Name) != null)
                ModelState.AddModelError("Name", T("A country with the same name already exists.").Text);
            if (string.IsNullOrWhiteSpace(viewModel.ThreeDigitCode))
                ModelState.AddModelError("ThreeDigitCode",
                    T("You need to specify a three-digit code for the new country.").Text);
            if (_countryService.GetCountryByCode(viewModel.ThreeDigitCode) != null)
                ModelState.AddModelError("ThreeDigitCode",
                    T("A country with the same three-digit code already exists.").Text);

            var currency = _currencyService.GetCurrency(viewModel.Currency);
            if (currency == null) ModelState.AddModelError("Currency", T("We could not find this currency.").Text);

            var translation = _translationService.GetTranslation(viewModel.Translation);
            if (translation == null)
                ModelState.AddModelError("Translation", T("We could not find this translation.").Text);

            if (!ModelState.IsValid) return View(viewModel);

            try {
                _countryService.CreateCountry(viewModel.Name, viewModel.ThreeDigitCode, viewModel.TwoDigitCode,
                    currency, translation, viewModel.Taxation ?? 0);

                _orchardServices.Notifier.Information(T("Country '{0}' was created successfully.", viewModel.Name));
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                Logger.Error(ex, "Error while creating country.");
                _orchardServices.Notifier.Error(T("Country creation failed with error: {0}.", ex.Message));
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to delete countries.")))
                return new HttpUnauthorizedResult();

            var country = _countryService.GetCountry(id);

            if (country != null) {
                // we need to do extra check if country is in use
                _countryService.DeleteCountry(id);
                _orchardServices.Notifier.Information(T("Country '{0}' was deleted successfully.", country.Name));
            }

            return RedirectToAction("Index");
        }

        [OutputCache(Duration = 0)]
        public ActionResult Edit(int id) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage countries.")))
                return new HttpUnauthorizedResult();

            var viewModel = new CountryEditViewModel();

            var country = _countryService.GetCountry(id);
            if (country == null) return RedirectToAction("Index");
            viewModel.Id = country.Id;
            viewModel.Name = country.Name;
            viewModel.TwoDigitCode = country.TwoDigitCode;
            viewModel.ThreeDigitCode = country.ThreeDigitCode;
            viewModel.Taxation = country.Taxation;
            viewModel.Currency = country.Currency.Id;
            viewModel.Translation = country.Translation.Id;
            viewModel.Currencies = _currencyService.GetCurrencies();
            if (viewModel.Currencies == null || !viewModel.Currencies.Any()) {
                _orchardServices.Notifier.Information(T("Please add a Currency before creating a new Country."));
                return RedirectToAction("Index");
            }

            viewModel.Translations = _translationService.GetTranslations();
            if (viewModel.Translations == null || !viewModel.Translations.Any()) {
                _orchardServices.Notifier.Information(T("Please add a Translation before creating a new Country."));
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(CountryEditViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage countries.")))
                return new HttpUnauthorizedResult();

            var country = _countryService.GetCountry(viewModel.Id);

            if (country == null)
                return HttpNotFound();

            var existing = _countryService.GetCountry(viewModel.Name);
            if (existing != null && existing.Id != country.Id)
                ModelState.AddModelError("Name", T("A country with that Name already exists.").Text);
            existing = _countryService.GetCountryByCode(viewModel.ThreeDigitCode);
            if (existing != null && existing.Id != country.Id)
                ModelState.AddModelError("ThreeDigitCode", T("A country with that Code already exists.").Text);

            var currency = _currencyService.GetCurrency(viewModel.Currency);
            if (currency == null) ModelState.AddModelError("Currency", T("We could not find this currency.").Text);

            var translation = _translationService.GetTranslation(viewModel.Translation);
            if (translation == null)
                ModelState.AddModelError("Translation", T("We could not find this translation.").Text);

            if (!ModelState.IsValid) return View(viewModel);

            try {
                _countryService.UpdateCountry(viewModel.Id, viewModel.Name, viewModel.ThreeDigitCode,
                    viewModel.TwoDigitCode, currency, translation, viewModel.Taxation);
                _orchardServices.Notifier.Information(T("Changes to country '{0}' were commited successfully.",
                    country.Name));

                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                Logger.Error(ex, "Error while editing country.");
                _orchardServices.Notifier.Error(T("Failed to edit country: {0} ", ex.Message));
                return View(viewModel);
            }
        }

        [OutputCache(Duration = 0)]
        public ActionResult Index() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                T("You don't have permission to manage countries.")))
                return new HttpUnauthorizedResult();

            var viewModel = new CountriesIndexViewModel();
            var countries = _countryService.GetCountries();
            IList<CountryInfo> countryInfos = countries.Select(country => new CountryInfo {
                                                            Country = country,
                                                            Translations = _countryService.GetTranslations(country)
                                                        })
                                                       .ToList();
            viewModel.Countries = countryInfos;

            return View(viewModel);
        }
    }
}