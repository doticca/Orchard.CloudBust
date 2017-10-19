using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.FontAwesome")]
    public class FontAwesomeSettingsPartRecord: ContentPartRecord
    {
        public virtual bool AutoEnable { get; set; }
        public virtual bool AutoEnableAdmin { get; set; }
        public FontAwesomeSettingsPartRecord()
        {
            AutoEnable = true;
            AutoEnableAdmin = false;
        }
    }
}