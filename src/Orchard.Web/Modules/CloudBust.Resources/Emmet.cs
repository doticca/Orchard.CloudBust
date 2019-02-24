using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Emmet")]
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("Emmet")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/js/emmet.min.js", "emmet.js")
                    .SetDependencies("Underscore");
        }
    }
}