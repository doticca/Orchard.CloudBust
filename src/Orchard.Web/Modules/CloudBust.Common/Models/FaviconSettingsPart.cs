using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Models {
    public class FaviconSettingsPart : ContentPart<FaviconSettingsPartRecord> {
        public string FaviconUrl {
            get { return Retrieve(r => r.FaviconUrl); }
            set { Store(r => r.FaviconUrl, value); }
        }
        public string AppleTouchIconUrl
        {
            get { return Retrieve(r => r.AppleTouchIconUrl); }
            set { Store(r => r.AppleTouchIconUrl, value); }
        }
        public string AndroidManifestUrl
        {
            get { return Retrieve(r => r.AndroidManifestUrl); }
            set { Store(r => r.AndroidManifestUrl, value); }
        }
        public string SafariPinnedUrl
        {
            get { return Retrieve(r => r.SafariPinnedUrl); }
            set { Store(r => r.SafariPinnedUrl, value); }
        }
        public string SafariPinnedMask
        {
            get { return Retrieve(r => r.SafariPinnedMask); }
            set { Store(r => r.SafariPinnedMask, value); }
        }
        public string MSApplicationConfigUrl
        {
            get { return Retrieve(r => r.MSApplicationConfigUrl); }
            set { Store(r => r.MSApplicationConfigUrl, value); }
        }
        public string PngImageUrl
        {
            get { return Retrieve(r => r.PngImageUrl); }
            set { Store(r => r.PngImageUrl, value); }
        }
        public string ThemeColor
        {
            get { return Retrieve(r => r.ThemeColor); }
            set { Store(r => r.ThemeColor, value); }
        }
    }
}
