using Orchard;
using System.Collections.Generic;
using CloudBust.Localization.Models;

namespace CloudBust.Localization.Services
{
    public interface ICurrencyService : IDependency
    {
        CurrencyRecord GetCurrency(int id);
        CurrencyRecord GetCurrency(string name);
        CurrencyRecord GetCurrencyByCode(string threeDigitCode);
        string GetCurrencyCode(int id);
        string GetCurrencyCode(string name);
        string GetCurrencyName(string threeDigitCode);
        string GetCurrencyName(int id);
        IEnumerable<CurrencyRecord> GetCurrencies();
        CurrencyRecord CreateCurrency(string name, string threeDigitCode);
        bool UpdateCurrency(int id, string name, string threeDigitCode, string sign, bool showSignAfter);
        bool UpdateCurrencyName(int id, string name);
        bool UpdateCurrencyCode(int id, string threeDigitCode);
        bool DeleteCurrency(int id);
    }
}