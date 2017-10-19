using Orchard.UI.Resources;

namespace CloudBust.Resources {
    public class Notify : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("Notify")
                .SetDependencies("jQuery")
                .SetUrl("https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.resources/js/notify.min.js", "notify.js")
                .SetVersion("0.4.2");
        }
    }
}
