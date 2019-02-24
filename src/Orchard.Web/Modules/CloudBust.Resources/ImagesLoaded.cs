using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.ImagesLoaded")]
    public class ImagesLoaded : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // defaults at common highlight
            manifest.DefineScript("ImagesLoaded")
                    .SetDependencies("jQuery")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/js/imagesloaded.pkgd.min.js", "imagesloaded.pkgd.js")
                    .SetVersion("4.1.4");
        }
    }
}