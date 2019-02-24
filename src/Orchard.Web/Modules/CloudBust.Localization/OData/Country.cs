using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CloudBust.Localization.Models;

namespace CloudBust.Localization.OData
{
    [DataContract]
    public class Country
    {
        public const string OType = "Country";

        public Country(CountryRecord p, string culture = "")
        {
            this.Id = p.Id;
            this.Type = OType;
            this.Name = p.Name;
            this.Code = p.TwoDigitCode;
            this.CurrencySign = p.Currency.ThreeDigitCode;
            var translations = new List<Translation>
            {
                new Translation(p.Translation, p.Name)
            };
            translations.AddRange(p.CountryTranslation.OrderBy(t => t.Position).Select(link => new Translation(link.Translation, link.Name)));
            this.Translations = translations;
            this.ActiveCulture = culture;
            this.Description = Name + "(" + Code + ")" + " - " + CurrencySign;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Type { get; private set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string CurrencySign { get; set; }

        [DataMember]
        public IList<Translation> Translations { get; set; }

        [DataMember]
        public string ActiveCulture { get; set; }
    }
}