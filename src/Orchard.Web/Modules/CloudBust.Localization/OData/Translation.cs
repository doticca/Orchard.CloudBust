using System.Runtime.Serialization;
using CloudBust.Localization.Models;

namespace CloudBust.Localization.OData
{
    [DataContract]
    public class Translation
    {
        public const string OType = "Translation";

        public Translation(TranslationRecord p, string countryTranslatedName)
        {
            this.Id = p.Id;
            this.Type = OType;
            this.Name = p.Name;
            this.Description = countryTranslatedName;
            this.Code = p.TwoDigitCode;
            this.CountryTranslatedName = countryTranslatedName;
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
        public string CountryTranslatedName { get; set; }

        [DataMember]
        public string Code { get; set; }
    }
}