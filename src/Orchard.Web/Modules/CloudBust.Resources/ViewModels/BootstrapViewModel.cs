using Orchard.Environment.Extensions;

namespace CloudBust.Resources.ViewModels
{
    [OrchardFeature("CloudBust.Resources.Bootstrap")]
    public class BootstrapSettingsViewModel
    {
        public bool AutoEnable { get; set; }
        public bool AutoEnableAdmin { get; set; }
    }
}