using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Particles")]
    public class Particles : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // defaults at common highlight
            manifest.DefineScript("Particles")
                .SetUrl("particles.min.js", "particles.js")                
                .SetVersion("2.0");

        }
    }
}
