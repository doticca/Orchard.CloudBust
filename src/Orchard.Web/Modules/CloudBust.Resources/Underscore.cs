using Orchard.UI.Resources;

namespace CloudBust.Resources {
    public class Underscore : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("Underscore")
                .SetUrl("https://cloudbust.blob.core.windows.net/public/js/underscore.min.js", "underscore.js")
                .SetVersion("1.9.1");
        }
    }
}
