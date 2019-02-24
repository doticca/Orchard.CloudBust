using System.Collections.Generic;
using CloudBust.Localization.Models;

namespace CloudBust.Localization.Models
{
    public class CountryInfo
    {
        public CountryRecord Country { get; set; }
        public IEnumerable<TranslationRecord> Translations { get; set; }
    }
}