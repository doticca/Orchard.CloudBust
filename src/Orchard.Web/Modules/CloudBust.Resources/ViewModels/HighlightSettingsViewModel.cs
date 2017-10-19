using Orchard.Environment.Extensions;

namespace CloudBust.Resources.ViewModels
{
    [OrchardFeature("CloudBust.Resources.Highlight")]
    public class HighlightSettingsViewModel
    {
        public string Style { get; set; }
        public bool AutoEnable { get; set; }
        public bool AutoEnableAdmin { get; set; }
        public bool FullBundle { get; set; }
    }
}