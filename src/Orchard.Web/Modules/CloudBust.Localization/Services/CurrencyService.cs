using System.Collections.Generic;
using System.Linq;
using CloudBust.Localization.Models;
using Orchard.Data;

namespace CloudBust.Localization.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IRepository<CurrencyRecord> _currenciesRepository;

        public CurrencyService(
            IRepository<CurrencyRecord> currenciesRepository
        )
        {
            _currenciesRepository = currenciesRepository;
        }

        public CurrencyRecord GetCurrency(int id)
        {
            try
            {
                var currency = _currenciesRepository.Get(id);
                return currency;
            }
            catch
            {
                return null;
            }
        }

        public CurrencyRecord GetCurrency(string name)
        {
            try
            {
                var currencies = from currency in _currenciesRepository.Table
                    where currency.Name == name
                    select currency;
                return currencies.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public CurrencyRecord GetCurrencyByCode(string threeDigitCode)
        {
            try
            {
                var currencies = from currency in _currenciesRepository.Table
                    where currency.ThreeDigitCode == threeDigitCode
                    select currency;
                return currencies.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public string GetCurrencyCode(int id)
        {
            var currency = GetCurrency(id);
            return currency?.ThreeDigitCode;
        }

        public string GetCurrencyCode(string name)
        {
            var currency = GetCurrency(name);
            return currency?.ThreeDigitCode;
        }

        public string GetCurrencyName(string threeDigitCode)
        {
            var currency = GetCurrencyByCode(threeDigitCode);
            return currency?.Name;
        }

        public string GetCurrencyName(int id)
        {
            var currency = GetCurrency(id);
            return currency?.Name;
        }

        public IEnumerable<CurrencyRecord> GetCurrencies()
        {
            try
            {
                var currencies = from currency in _currenciesRepository.Table select currency;
                return currencies.ToList();
            }
            catch
            {
                return new List<CurrencyRecord>();
            }
        }

        public CurrencyRecord CreateCurrency(string name, string threeDigitCode)
        {
            _currenciesRepository.Create(new CurrencyRecord
            {
                Name = name,
                ThreeDigitCode = threeDigitCode
            });

            var currency = GetCurrency(name);

            return currency;
        }

        public bool UpdateCurrency(int id, string name, string threeDigitCode, string sign, bool showSignAfter)
        {
            var currency = GetCurrency(id);
            if (currency == null) {
                return false;
            }

            currency.Name = name;
            currency.ThreeDigitCode = threeDigitCode;
            currency.Sign = sign;
            currency.ShowSignAfter = showSignAfter;

            return true;
        }

        public bool UpdateCurrencyName(int id, string name)
        {
            var currency = GetCurrency(id);
            if (currency == null) {
                return false;
            }

            currency.Name = name;

            return true;
        }

        public bool UpdateCurrencyCode(int id, string threeDigitCode)
        {
            var currency = GetCurrency(id);
            if (currency != null)
            {
                currency.ThreeDigitCode = threeDigitCode;
                return true;
            }

            return false;
        }

        public bool DeleteCurrency(int id)
        {
            var currency = GetCurrency(id);
            if (currency == null) {
                return false;
            }

            _currenciesRepository.Delete(currency);

            return true;
        }

        public bool UpdateCurrencyCode(string name, string threeDigitCode)
        {
            var currency = GetCurrency(name);
            if (currency == null) {
                return false;
            }

            currency.ThreeDigitCode = threeDigitCode;

            return true;
        }
    }
}