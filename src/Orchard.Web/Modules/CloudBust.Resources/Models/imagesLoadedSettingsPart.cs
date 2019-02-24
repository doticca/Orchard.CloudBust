using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Models {
    [OrchardFeature("CloudBust.Resources.ImagesLoaded")]
    public class ImagesLoadedSettingsPart : ContentPart<ImagesLoadedSettingsPartRecord> {
        public bool AutoEnable
        {
            get => Record.AutoEnable;
            set => Record.AutoEnable = value;
        }

        public bool AutoEnableAdmin
        {
            get => Record.AutoEnableAdmin;
            set => Record.AutoEnableAdmin = value;
        }
    }
}