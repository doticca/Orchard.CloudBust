using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.ElegantIcon")]
    public class ElegantIcon : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineStyle("ElegantIcon")
                .SetUrl("fonts/elegant/elegant-icon.css")
                .SetVersion("1.0");
        }
    }
}
