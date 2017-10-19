using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Slick")]
    public class Slick : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // defaults at common highlight
            manifest.DefineScript("Slick")
                .SetDependencies("jQuery")
                .SetUrl("slick.min.js", "slick.js")                
                .SetVersion("1.4.1");

            manifest.DefineStyle("Slick_Core")
                .SetVersion("1.4.1")
                .SetUrl("slick/slick.css", "slick/slick.css");
            manifest.DefineStyle("Slick")
                .SetVersion("1.4.1")
                .SetDependencies("Slick_Core")
                .SetUrl("slick/slick-theme.css", "slick/slick-theme.css");
        }
    }
}
