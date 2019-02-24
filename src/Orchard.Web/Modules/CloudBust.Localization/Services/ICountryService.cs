using Orchard;
using System.Collections.Generic;
using CloudBust.Localization.Models;

namespace CloudBust.Localization.Services
{
    public interface ICountryService : IDependency {
        void SignalCountryCollection();

        CountryRecord GetCountry(int id);

        CountryRecord GetCountry(string countryName);

        IEnumerable<TranslationRecord> GetTranslations(CountryRecord country);

        IEnumerable<TranslationRecord> GetAvailableTranslations();

        CountryRecord GetCountryFromCulture(string cultureCode, out TranslationRecord translation);

        string CurrentCulture();

        void SetCulture(string culture);

        CountryRecord GetCountryByCode(string threeDigitCode);

        string GetCountryCode(int id);

        string GetCountryCode(string name);

        IList<CountryRecord> GetCountries();

        CountryRecord CreateCountry(string name, string threeDigitCode, string twoDigitCode, CurrencyRecord currencyRecord, TranslationRecord translationRecord, int taxation);

        bool UpdateCountry(int id, string name, string threeDigitCode, string twoDigitCode, CurrencyRecord currencyRecord, TranslationRecord translationRecord, int taxation);

        bool UpdateCountryCode(int id, string threeDigitCode);

        bool UpdateCountryTranslation(int id, TranslationRecord translationRecord);

        bool UpdateCountryTranslation(string name, TranslationRecord translationRecord);

        bool DeleteCountry(int id);
    }
}