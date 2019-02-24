using System.Collections.Generic;
using CloudBust.Localization.Models;

namespace CloudBust.Localization.ViewModels
{
    public class CountryEditViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TwoDigitCode { get; set; }

        public string ThreeDigitCode { get; set; }

        public IEnumerable<TranslationRecord> Translations { get; set; }

        public int Translation { get; set; }

        public IEnumerable<CurrencyRecord> Currencies { get; set; }

        public int Currency { get; set; }

        public int Taxation { get; set; }
    }
}