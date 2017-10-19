using Orchard.Environment.Extensions;

namespace CloudBust.Resources.ViewModels
{
    [OrchardFeature("CloudBust.Resources.ImagesLoaded")]
    public class imagesLoadedSettingsViewModel
    {
        public bool AutoEnable { get; set; }
        public bool AutoEnableAdmin { get; set; }
    }
}