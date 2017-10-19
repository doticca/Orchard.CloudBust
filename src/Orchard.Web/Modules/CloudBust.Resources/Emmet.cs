using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace js.Emmet {
    [OrchardFeature("CloudBust.Resources.Emmet")]
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("Emmet")
                .SetUrl("emmet.min.js", "emmet.js")
                .SetDependencies("Underscore");

        }
    }
}
