using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Bootstrap")]
    public class Bootstrap : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("Bootstrap")
                .SetUrl("https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.resources/js/bootstrap.min.js", "https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.resources/js/bootstrap.js")
                .SetVersion("3.3.7");

            manifest.DefineStyle("Bootstrap")
                .SetUrl("https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.resources/css/bootstrap.min.css", "https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.resources/css/bootstrap.css")
                .SetVersion("3.3.7");
        }
    }
}
