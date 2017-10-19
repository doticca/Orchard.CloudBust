using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Models {
    public class FaviconSettingsPart : ContentPart<FaviconSettingsPartRecord> {
        public string FaviconUrl {
            get { return Retrieve(r => r.FaviconUrl); }
            set { Store(r => r.FaviconUrl, value); }
        }
    }
}
