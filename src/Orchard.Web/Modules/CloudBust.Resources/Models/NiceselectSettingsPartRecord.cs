using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.Niceselect")]
    public class NiceselectSettingsPartRecord: ContentPartRecord
    {
        public virtual bool AutoEnable { get; set; }

        public NiceselectSettingsPartRecord()
        {
            AutoEnable = true;
        }
    }
}