using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Models {
    public class FaviconSettingsPartRecord : ContentPartRecord {
        public virtual string FaviconUrl { get; set; }
    }
}