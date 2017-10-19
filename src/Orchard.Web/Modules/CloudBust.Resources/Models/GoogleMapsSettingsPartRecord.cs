using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.GoogleMaps")]
    public class GoogleMapsSettingsPartRecord: ContentPartRecord
    {
        public virtual bool AutoEnable { get; set; }
        public virtual bool AutoEnableAdmin { get; set; }
        public virtual string ApiKey { get; set; }
        public virtual bool Sensor { get; set; }
        public virtual string CallBack { get; set; }
        public virtual string Language { get; set; }
        public virtual bool Async { get; set; }
        public virtual bool Defer { get; set; }

        public GoogleMapsSettingsPartRecord()
        {
            ApiKey = string.Empty;
            CallBack = string.Empty;
            Language = "en";
            Sensor = false;
            AutoEnable = true;
            AutoEnableAdmin = false;
            Async = false;
            Defer = false;
        }
    }
}