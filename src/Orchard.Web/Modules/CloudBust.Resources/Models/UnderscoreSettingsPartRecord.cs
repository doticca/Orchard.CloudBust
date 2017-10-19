using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.Underscore")]
    public class UnderscoreSettingsPartRecord: ContentPartRecord
    {
        public virtual bool AutoEnable { get; set; }
        public virtual bool AutoEnableAdmin { get; set; }

        public UnderscoreSettingsPartRecord()
        {
            AutoEnable = true;
            AutoEnableAdmin = true;
        }
    }
}