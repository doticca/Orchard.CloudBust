using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Highlight")]
    public class Highlight : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // defaults at common highlight
            manifest.DefineScript("Highlight")
                .SetUrl("highlight.default.pack.js")                
                .SetVersion("9.12");

            manifest.DefineScript("Highlight_Full")
                .SetUrl("highlight.full.pack.js")
                .SetVersion("9.12");

            manifest.DefineStyle("Highlight_default")
                .SetVersion("9.12")
                .SetUrl("highlight/default.css", "highlight/default.css");
            manifest.DefineStyle("Highlight_xcode")
                .SetVersion("9.12")
                .SetUrl("highlight/xcode.css", "highlight/xcode.css");
            manifest.DefineStyle("Highlight_foundation")
                .SetVersion("9.12")
                .SetUrl("highlight/foundation.css", "highlight/foundation.css");
            manifest.DefineStyle("Highlight_dark")
                .SetVersion("9.12")
                .SetUrl("highlight/dark.css", "highlight/dark.css");
        }
    }
}
