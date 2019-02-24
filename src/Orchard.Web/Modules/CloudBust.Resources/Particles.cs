using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Particles")]
    public class Particles : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("Particles")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/js/particles.min.js", "particles.js")
                    .SetVersion("2.0");
        }
    }
}