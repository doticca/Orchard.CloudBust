using Orchard.Data.Conventions;
using System.Collections.Generic;

namespace CloudBust.Localization.Models
{
    public class CountryRecord 
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string TwoDigitCode { get; set; }

        public virtual string ThreeDigitCode { get; set; }

        [Aggregate]
        public virtual CurrencyRecord Currency { get; set; }

        public virtual int Taxation { get; set; }

        [Aggregate]
        public virtual TranslationRecord Translation { get; set; }

        [Aggregate]
        [CascadeAllDeleteOrphan]
        public virtual IList<CountryTranslationRecord> CountryTranslation { get; set; }

        public CountryRecord()
        {
            CountryTranslation = new List<CountryTranslationRecord>();
        }
    }
}