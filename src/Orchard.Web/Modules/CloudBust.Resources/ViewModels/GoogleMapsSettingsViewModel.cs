using Orchard.Environment.Extensions;

namespace CloudBust.Resources.ViewModels
{
    [OrchardFeature("CloudBust.Resources.GoogleMaps")]
    public class GoogleMapsSettingsViewModel
    {
        public bool AutoEnable { get; set; }
        public bool AutoEnableAdmin { get; set; }
        public string ApiKey { get; set; }
        public bool Sensor { get; set; }
        public string CallBack { get; set; }
        public string Language { get; set; }
        public virtual bool Async { get; set; }
        public virtual bool Defer { get; set; }
    }
}