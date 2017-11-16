using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Models {
    public class FaviconSettingsPartRecord : ContentPartRecord {
        public virtual string FaviconUrl { get; set; }
        public virtual string AppleTouchIconUrl { get; set; }
        public virtual string PngImageUrl { get; set; }
        public virtual string AndroidManifestUrl { get; set; }
        public virtual string SafariPinnedUrl { get; set; }
        public virtual string SafariPinnedMask { get; set; }
        public virtual string MSApplicationConfigUrl { get; set; }
        public virtual string ThemeColor { get; set; }
    }
}