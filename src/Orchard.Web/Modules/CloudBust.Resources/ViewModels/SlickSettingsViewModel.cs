using Orchard.Environment.Extensions;

namespace CloudBust.Resources.ViewModels
{
    [OrchardFeature("CloudBust.Resources.Slick")]
    public class SlickSettingsViewModel
    {
        public bool AutoEnable { get; set; }
        public bool AutoEnableAdmin { get; set; }
    }
}