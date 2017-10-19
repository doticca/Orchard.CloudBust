using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.Magnific")]
    public class MagnificSettingsPartRecord: ContentPartRecord
    {
        public virtual bool AutoEnable { get; set; }
        public virtual bool AutoEnableAdmin { get; set; }

        public MagnificSettingsPartRecord()
        {
            AutoEnable = true;
            AutoEnableAdmin = false;
        }
    }
}