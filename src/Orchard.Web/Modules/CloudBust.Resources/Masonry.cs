using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Masonry")]
    public class Masonry : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // defaults at common highlight
            manifest.DefineScript("Masonry")
                .SetDependencies("jQuery")
                .SetUrl("https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.resources/js/masonry.pkgd.min.js", "https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.resources/js/masonry.pkgd.js")                
                .SetVersion("4.1.1");
        }
    }
}
