using Orchard.UI.Resources;

namespace CloudBust.Foundation {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("Icons_Foundation")
                .SetUrl("foundation-icons.css");
        }
    }
}
