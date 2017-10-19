using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.Notify")]
    public class NotifySettingsPartRecord : ContentPartRecord
    {
        public virtual bool AutoEnable { get; set; }
        public virtual bool AutoEnableAdmin { get; set; }

        public NotifySettingsPartRecord()
        {
            AutoEnable = true;
            AutoEnableAdmin = true;
        }
    }
}