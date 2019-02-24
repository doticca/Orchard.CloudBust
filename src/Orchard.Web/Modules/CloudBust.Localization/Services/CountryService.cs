using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudBust.Localization.Models;
using Orchard;
using Orchard.Caching;
using Orchard.Data;
using Orchard.Environment.Configuration;
using Orchard.Localization.Services;
using Orchard.Mvc;
using Orchard.Services;

namespace CloudBust.Localization.Services {
    public class CountryService : ICountryService {
        private const string FrontEndCookieName = "OrchardCurrentCulture-FrontEnd";
        private const int DefaultExpireTimeYear = 1;
        private readonly ICacheManager _cacheManager;
        private readonly IClock _clock;
        private readonly IRepository<CountryRecord> _countriesRepository;
        private readonly IRepository<CountryTranslationRecord> _countrytranslationRepository;
        private readonly ICultureManager _cultureManager;
        private readonly IGeoDataService _geodataService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrchardServices _orchardServices;
        private readonly ShellSettings _shellSettings;
        private readonly ISignals _signals;
        private readonly ISessionFactoryHolder _sessionFactory;

        public CountryService(
            IRepository<CountryRecord> countriesRepository,
            IRepository<CountryTranslationRecord> countrytranslationRepository,
            IHttpContextAccessor httpContextAccessor,
            IOrchardServices orchardServices,
            IGeoDataService geodataService,
            IClock clock,
            ShellSettings shellSettings,
            ICultureManager cultureManager, 
            ICacheManager cacheManager, 
            ISignals signals, 
            ISessionFactoryHolder sessionFactory) 
        {
            _countriesRepository = countriesRepository;
            _countrytranslationRepository = countrytranslationRepository;
            _orchardServices = orchardServices;
            _geodataService = geodataService;
            _httpContextAccessor = httpContextAccessor;
            _clock = clock;
            _shellSettings = shellSettings;
            _cultureManager = cultureManager;
            _cacheManager = cacheManager;
            _signals = signals;
            _sessionFactory = sessionFactory;
        }

        public string CountriesCachekey { get; } = "CloudBust.Localization.countries";
        public string CountriesCacheTrigger { get; } = "CloudBust.Localization.countries.changed";

        public CountryRecord CreateCountry(string name, string threeDigitCode, string twoDigitCode, CurrencyRecord currencyRecord, TranslationRecord translationRecord, int taxation) {
            _countriesRepository.Create(new CountryRecord {
                Name = name,
                ThreeDigitCode = threeDigitCode,
                TwoDigitCode = twoDigitCode,
                Currency = currencyRecord,
                Translation = translationRecord,
                Taxation = taxation
            });
            _countriesRepository.Flush();
            SignalCountryCollection();

            return GetCountry(name);
        }

        public string CurrentCulture() {
            var cookie = _orchardServices.WorkContext.HttpContext.Request.Cookies[FrontEndCookieName];

            if (cookie != null) return cookie.Value;

            var data = _geodataService.GetGeoLocation();
            if (data == null || string.IsNullOrWhiteSpace(data.country_code)) {
                var serverculture = ServerCulture();
                SetCulture(serverculture);
                return serverculture;
            }

            var country = GetCountryBySmallCode(data.country_code);
            if (country == null) return ServerCulture();

            var culture = country.Translation.TwoDigitCode.ToLowerInvariant() + "-" + country.TwoDigitCode.ToLowerInvariant();
            SetCulture(culture);

            return culture;
        }

        public bool DeleteCountry(int id) {
            var country = GetCountry(id);

            if (country == null) return false;

            _countriesRepository.Delete(country);
            _countriesRepository.Flush();
            SignalCountryCollection();

            return true;
        }

        public IEnumerable<TranslationRecord> GetAvailableTranslations() {
            IList<TranslationRecord> Translations = new List<TranslationRecord>();

            foreach (var culture in _cultureManager.ListCultures()) {
                var cultureparts = culture.Split('-');
                if (cultureparts.Length < 2) continue;

                var country_code = cultureparts[1];
                if (string.IsNullOrWhiteSpace(country_code)) continue;

                var country = GetCountryBySmallCode(country_code);
                if (country == null) continue;

                var Translation = GetTranslations(country).FirstOrDefault(t => t.TwoDigitCode.ToLowerInvariant() == cultureparts[0]);
                if (Translation != null) Translations.Add(Translation);
            }

            return Translations;
        }

        public CountryRecord GetCountryByCode(string threeDigitCode) {
            try {
                var countries = from country in _countriesRepository.Table where country.ThreeDigitCode == threeDigitCode select country;
                return countries.FirstOrDefault();
            }
            catch {
                return null;
            }
        }

        public string GetCountryCode(int id) {
            var country = GetCountry(id);
            if (country != null) return country.ThreeDigitCode;

            return null;
        }

        public string GetCountryCode(string name) {
            var country = GetCountry(name);
            if (country != null) return country.ThreeDigitCode;

            return null;
        }

        public IEnumerable<TranslationRecord> GetTranslations(CountryRecord country) {
            IList<TranslationRecord> Translations = new List<TranslationRecord> {
                country.Translation
            };

            try {
                foreach (var link in country.CountryTranslation.OrderBy(l => l.Position)) Translations.Add(link.Translation);
            }
            catch {
                return Translations;
            }

            return Translations;
        }

        public void SetCulture(string culture) {
            if (_httpContextAccessor.Current() == null) return;

            var cookie = new HttpCookie(FrontEndCookieName, culture) {
                Expires = _clock.UtcNow.AddYears(DefaultExpireTimeYear)
            };

            var httpContextBase = _httpContextAccessor.Current();

            cookie.Domain = !httpContextBase.Request.IsLocal ? httpContextBase.Request.Url.Host : null;

            if (!string.IsNullOrEmpty(_shellSettings.RequestUrlPrefix)) cookie.Path = GetCookiePath(httpContextBase);

            httpContextBase.Request.Cookies.Remove(FrontEndCookieName);
            httpContextBase.Response.Cookies.Remove(FrontEndCookieName);
            httpContextBase.Response.Cookies.Add(cookie);

            SignalCountryCollection();
        }

        public bool UpdateCountry(int id, string name, string threeDigitCode, string twoDigitCode, CurrencyRecord currencyRecord, TranslationRecord translationRecord, int taxation) {
            var countryRecord = _countriesRepository.Table.FirstOrDefault(item => item.Id == id);

            if (countryRecord != null) {
                countryRecord.Name = name;
                countryRecord.ThreeDigitCode = threeDigitCode;
                countryRecord.TwoDigitCode = twoDigitCode;
                countryRecord.Currency = currencyRecord;
                countryRecord.Translation = translationRecord;
                countryRecord.Taxation = taxation;

                _countriesRepository.Flush();
                _countrytranslationRepository.Flush();
                SignalCountryCollection();

                return true;
            }

            return false;
        }

        public bool UpdateCountryCode(int id, string threeDigitCode) {
            var countryRecord = _countriesRepository.Table.FirstOrDefault(item => item.Id == id);

            if (countryRecord != null) {
                countryRecord.ThreeDigitCode = threeDigitCode;
                _countriesRepository.Flush();
                _countrytranslationRepository.Flush();
                SignalCountryCollection();

                return true;
            }

            return false;
        }

        public bool UpdateCountryTranslation(int id, TranslationRecord translationRecord) {
            var countryRecord = _countriesRepository.Table.FirstOrDefault(item => item.Id == id);

            if (countryRecord != null) {
                countryRecord.Translation = translationRecord;
                _countriesRepository.Flush();
                _countrytranslationRepository.Flush();
                SignalCountryCollection();

                return true;
            }

            return false;
        }

        public bool UpdateCountryTranslation(string name, TranslationRecord translationRecord) {
            var countryRecord = _countriesRepository.Table.FirstOrDefault(item => item.Name == name);

            if (countryRecord != null) {
                countryRecord.Translation = translationRecord;
                _countriesRepository.Flush();
                _countrytranslationRepository.Flush();
                SignalCountryCollection();

                return true;
            }

            return false;
        }

        private string GetCookiePath(HttpContextBase httpContext) {
            var cookiePath = httpContext.Request.ApplicationPath;
            if (cookiePath?.Length > 1) cookiePath += '/';

            cookiePath += _shellSettings.RequestUrlPrefix;

            return cookiePath;
        }

        private string ServerCulture() {
            return _orchardServices.WorkContext.CurrentCulture;
        }

        public bool UpdateCountryCode(string Name, string threeDigitCode) {
            var countryRecord = _countriesRepository.Table.FirstOrDefault(item => item.Name == Name);

            if (countryRecord != null) {
                countryRecord.ThreeDigitCode = threeDigitCode;
                _countriesRepository.Flush();
                _countrytranslationRepository.Flush();
                SignalCountryCollection();

                return true;
            }

            return false;
        }

        #region Cached

        public CountryRecord GetCountryFromCulture(string cultureCode, out TranslationRecord translation) {
            translation = null;

            try {
                var countrywithculture = GetCountryWithCulture(cultureCode);

                if (countrywithculture != null) {
                    translation = countrywithculture.Translation;
                    return countrywithculture;
                }

                var parts = cultureCode.ToLowerInvariant().Split('-');
                var t = GetCountryTranslation(parts[1], parts[0]); //(from link in _countrytranslationRepository.Table where link.Country.TwoDigitCode == parts[1] && link.Translation.TwoDigitCode == parts[0] select link).FirstOrDefault();

                if (t != null) {
                    translation = t.Translation;
                    return t.Country;
                }
            }
            catch {
                translation = null;

                return null;
            }

            var c = GetDefaultCountry();
            translation = c?.Translation;

            return c;
        }

        public CountryTranslationRecord GetCountryTranslation(string countryTwoDigit, string translationTwoDigit) {
            return _cacheManager.Get(
                KeyForCountryTranslation(countryTwoDigit, translationTwoDigit),
                ctx => {
                    ctx.Monitor(_signals.When(SignalForCountryTranslation(countryTwoDigit, translationTwoDigit)));
                    try {
                        var countryTranslationRecord = (from link in _countrytranslationRepository.Table where link.Country.TwoDigitCode == countryTwoDigit && link.Translation.TwoDigitCode == translationTwoDigit select link).FirstOrDefault();

                        FetchCountryTranslation(countryTranslationRecord);

                        return countryTranslationRecord;
                    }
                    catch {
                        SignalCountryCollection();
                        return null;
                    }
                }
            );
        }

        public CountryRecord GetCountry(int id) {
            return _cacheManager.Get(
                KeyForCountry(id),
                ctx => {
                    ctx.Monitor(_signals.When(SignalForCountry(id)));

                    try {
                        var countryRecord = _countriesRepository.Table.FirstOrDefault(item => item.Id == id);

                        FetchCountry(countryRecord);

                        return countryRecord;
                    }
                    catch {
                        SignalCountryCollection();
                        return null;
                    }
                }
            );
        }

        private CountryRecord GetDefaultCountry() {
            return _cacheManager.Get(
                KeyForDefaultCountry(),
                ctx => {
                    ctx.Monitor(_signals.When(SignalForDefaultCountry()));

                    try {
                        var defaultCountry = _countriesRepository.Table.Select(country => country).FirstOrDefault();
                        if (defaultCountry != null) {
                            string name;
                            name = defaultCountry.Currency.Name;
                            if (defaultCountry.CountryTranslation != null) {
                                foreach (var translation in defaultCountry.CountryTranslation) {
                                    name = translation.Country.Name;
                                    name = translation.Translation.Name;
                                }
                            }

                            if (defaultCountry.Translation != null) {
                                name = defaultCountry.Translation.Name;
                            }

                            return defaultCountry;
                        }

                        return null;
                    }
                    catch {
                        SignalCountryCollection();
                        return null;
                    }
                }
            );
        }

        private CountryRecord GetCountryWithCulture(string cultureCode) {
            return _cacheManager.Get(
                KeyForCountry(cultureCode),
                ctx => {
                    ctx.Monitor(_signals.When(SignalForCountry(cultureCode)));
                    try {
                        var parts = cultureCode.ToLowerInvariant().Split('-');
                        var countryRecord = (from country in _countriesRepository.Table
                            where country.Translation.TwoDigitCode.ToLowerInvariant() == parts[0] && country.TwoDigitCode.ToLowerInvariant() == parts[1]
                            select country).FirstOrDefault();

                        FetchCountry(countryRecord);

                        return countryRecord;
                    }
                    catch {
                        SignalCountryCollection();
                        return null;
                    }
                }
            );
        }

        public CountryRecord GetCountry(string countryName) {
            return _cacheManager.Get(
                KeyForCountryName(countryName),
                ctx => {
                    ctx.Monitor(_signals.When(SignalForCountryName(countryName)));

                    try {
                        var countryRecord = _countriesRepository.Table.FirstOrDefault(country => country.Name == countryName);

                        FetchCountry(countryRecord);

                        return countryRecord;
                    }
                    catch {
                        SignalCountryCollection();
                        return null;
                    }
                }
            );
        }

        public CountryRecord GetCountryBySmallCode(string twoDigitCode) {
            return _cacheManager.Get(
                KeyForCountrySmallCode(twoDigitCode),
                ctx => {
                    ctx.Monitor(_signals.When(SignalForCountrySmallCode(twoDigitCode)));

                    try {
                        var countryRecord = _countriesRepository.Table.FirstOrDefault(country => string.Equals(country.TwoDigitCode, twoDigitCode, StringComparison.InvariantCultureIgnoreCase));

                        FetchCountry(countryRecord);

                        return countryRecord;
                    }
                    catch {
                        SignalCountryCollection();
                        return null;
                    }
                }
            );
        }

        public IList<CountryRecord> GetCountries() {
            return _cacheManager.Get(
                CountriesCachekey,
                ctx => {
                    ctx.Monitor(_signals.When(CountriesCacheTrigger));

                    try {
                        var countries = _countriesRepository.Table.Select(country => country);
                        foreach (var countryRecord in countries) {
                            FetchCountry(countryRecord);
                        }

                        return countries.ToList();
                    }
                    catch {
                        SignalCountryCollection();
                        return new List<CountryRecord>();
                    }
                }
            );
        }

        public void SignalCountryCollection() {
            _signals.Trigger(CountriesCacheTrigger); // collection
            var countries = GetCountries();
            _signals.Trigger(SignalForDefaultCountry()); // default country

            foreach (var country in countries) {
                _signals.Trigger(SignalForCountryName(country.Name)); // signals by country name
                _signals.Trigger(SignalForCountry(country.Id)); // signals by country id
                _signals.Trigger(SignalForCountrySmallCode(country.TwoDigitCode)); // signals for country two digit code
                var twodigitcode = country.TwoDigitCode;
                var transtwodigitcode = country.Translation.TwoDigitCode;
                var cultureCode = transtwodigitcode + "-" + twodigitcode;
                _signals.Trigger(SignalForCountry(cultureCode));
            }

            var translations = from link in _countrytranslationRepository.Table select link;
            foreach (var translation in translations) {
                var twodigitcode = translation.Country.TwoDigitCode.ToLowerInvariant();
                var transtwodigitcode = translation.Translation.TwoDigitCode.ToLowerInvariant();
                _signals.Trigger(SignalForCountryTranslation(twodigitcode, transtwodigitcode));
            }
        }

        private static void FetchCountry(CountryRecord countryRecord) {
            if (countryRecord != null) {
                string name;

                if (countryRecord.Translation != null) {
                    name = countryRecord.Translation.Name;
                }

                if (countryRecord.Currency != null) {
                    name = countryRecord.Currency.Name;
                }

                if (countryRecord.CountryTranslation != null) {
                    foreach (var translation in countryRecord.CountryTranslation) {
                        name = translation.Country.Name;
                        name = translation.Translation.Name;
                        if (translation.Country.Translation != null) {
                            name = translation.Country.Translation.Name;
                        }

                        if (translation.Translation != null) {
                            name = translation.Translation.Name;
                        }
                    }
                }
            }
        }

        private static void FetchCountryTranslation(CountryTranslationRecord countryTranslationRecord) {
            if (countryTranslationRecord == null) return;

            if (countryTranslationRecord.Country != null) {
                var countryRecord = countryTranslationRecord.Country;
                FetchCountry(countryRecord);
            }

            if (countryTranslationRecord.Translation != null) {
                var translationName = countryTranslationRecord.Translation.Name;
            }
        }

        private string KeyForDefaultCountry() {
            return CountriesCachekey + ".default";
        }

        private string SignalForDefaultCountry() {
            return CountriesCachekey + ".default" + ".changed";
        }

        private string KeyForCountry(int countryId) {
            return CountriesCachekey + "." + countryId;
        }
        
        private string SignalForCountry(int countryId) {
            return CountriesCachekey + "." + countryId + ".changed";
        }        

        private string KeyForCountry(string cultureCode) {
            return CountriesCachekey + "." + cultureCode.ToLowerInvariant();
        }

        private string SignalForCountry(string cultureCode) {
            return CountriesCachekey + "." + cultureCode.ToLowerInvariant() + ".changed";
        }

        private string KeyForCountryName(string countryName) {
            return CountriesCachekey + ".name." + countryName;
        }

        private string SignalForCountryName(string countryName) {
            return CountriesCachekey + ".name." + countryName + ".changed";
        }

        private string KeyForCountrySmallCode(string countryTwoDigit) {
            return CountriesCachekey + ".scode." + countryTwoDigit;
        }

        private string SignalForCountrySmallCode(string countryTwoDigit) {
            return CountriesCachekey + ".scode." + countryTwoDigit + ".changed";
        }

        private string KeyForCountryTranslation(string countryTwoDigit, string translationTwoDigit) {
            return CountriesCachekey + "." + countryTwoDigit.ToLowerInvariant() + "-" + translationTwoDigit.ToLowerInvariant();
        }

        private string SignalForCountryTranslation(string countryTwoDigit, string translationTwoDigit) {
            return CountriesCachekey + "." + countryTwoDigit.ToLowerInvariant() + "-" + translationTwoDigit.ToLowerInvariant() + ".changed";
        }

        #endregion
    }
}