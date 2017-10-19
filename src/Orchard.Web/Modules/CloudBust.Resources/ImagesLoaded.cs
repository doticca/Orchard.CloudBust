using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.ImagesLoaded")]
    public class imagesLoaded : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // defaults at common highlight
            manifest.DefineScript("imagesLoaded")
                .SetDependencies("jQuery")
                .SetUrl("https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.resources/js/imagesloaded.pkgd.min.js", "imagesloaded.pkgd.js")                
                .SetVersion("3.1.8");
        }
    }
}
