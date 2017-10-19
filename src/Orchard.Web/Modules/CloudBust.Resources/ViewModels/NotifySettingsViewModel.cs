using Orchard.Environment.Extensions;

namespace CloudBust.Resources.ViewModels
{
    [OrchardFeature("CloudBust.Resources.Notify")]
    public class NotifySettingsViewModel
    {
        public bool AutoEnable { get; set; }
        public bool AutoEnableAdmin { get; set; }
    }
}