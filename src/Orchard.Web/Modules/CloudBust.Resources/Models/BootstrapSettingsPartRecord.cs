using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.Bootstrap")]
    public class BootstrapSettingsPartRecord : ContentPartRecord
    {
        public virtual bool AutoEnable { get; set; }
        public virtual bool AutoEnableAdmin { get; set; }

        public BootstrapSettingsPartRecord()
        {
            AutoEnable = true;
            AutoEnableAdmin = false;
        }
    }
}