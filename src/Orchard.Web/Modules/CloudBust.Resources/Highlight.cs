using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Highlight")]
    public class Highlight : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // defaults at common highlight
            manifest.DefineScript("Highlight")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/js/highlight.default.pack.js", "highlight.default.pack.js")
                    .SetVersion("9.13.1");
            manifest.DefineScript("Highlight_Full")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/js/highlight.full.pack.js", "highlight.full.pack.js")
                    .SetVersion("9.13.1");

            manifest.DefineStyle("Highlight_default")
                    .SetVersion("9.13.1")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/css/highlight/default.css", "highlight/default.css");
            manifest.DefineStyle("Highlight_xcode")
                    .SetVersion("9.13.1")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/css/highlight/xcode.css", "highlight/xcode.css");
            manifest.DefineStyle("Highlight_foundation")
                    .SetVersion("9.13.1")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/css/highlight/foundation.css", "highlight/foundation.css");
            manifest.DefineStyle("Highlight_dark")
                    .SetVersion("9.13.1")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/css/highlight/dark.css", "highlight/dark.css");
            manifest.DefineStyle("Highlight_vs")
                    .SetVersion("9.13.1")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/css/highlight/vs.css", "highlight/vs.css");
            manifest.DefineStyle("Highlight_vs2015")
                    .SetVersion("9.13.1")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/css/highlight/vs2015.css", "highlight/vs2015.css");
        }
    }
}