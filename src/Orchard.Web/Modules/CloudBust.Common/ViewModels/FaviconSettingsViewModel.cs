using System.Collections.Generic;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.ViewModels {
    public class FaviconSettingsViewModel {
        public string FaviconUrl { get; set; }
        public IEnumerable<string> FaviconUrlSuggestions { get; set; }
        public string AppleTouchIconUrl { get; set; }
        public IEnumerable<string> AppleTouchIconUrlSuggestions { get; set; }
        public string PngImageUrl { get; set; }
        public IEnumerable<string> PngImageSuggestions { get; set; }
        public string AndroidManifestUrl { get; set; }
        public IEnumerable<string> AndroidManifestSuggestions { get; set; }
        public string SafariPinnedUrl { get; set; }
        public string SafariPinnedMask { get; set; }
        public IEnumerable<string> SafariSuggestions { get; set; }
        public string MSApplicationConfigUrl { get; set; }
        public IEnumerable<string> MSApplicationConfigSuggestions { get; set; }
        public string ThemeColor { get; set; }
    }
}