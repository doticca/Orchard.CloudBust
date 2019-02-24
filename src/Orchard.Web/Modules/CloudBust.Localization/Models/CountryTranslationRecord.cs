using Orchard.Data.Conventions;

namespace CloudBust.Localization.Models
{
    public class CountryTranslationRecord
    {
        public virtual int Id { get; set; }

        [Aggregate]
        public virtual CountryRecord Country { get; set; }

        [Aggregate]
        public virtual TranslationRecord Translation { get; set; }

        public virtual int Position { get; set; }

        public virtual string Name { get; set; }
    }
}