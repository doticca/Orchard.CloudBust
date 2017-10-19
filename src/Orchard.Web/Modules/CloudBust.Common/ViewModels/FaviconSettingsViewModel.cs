using System.Collections.Generic;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.ViewModels {
    public class FaviconSettingsViewModel {
        public string FaviconUrl { get; set; }
        public IEnumerable<string> FaviconUrlSuggestions { get; set; }
    }
}