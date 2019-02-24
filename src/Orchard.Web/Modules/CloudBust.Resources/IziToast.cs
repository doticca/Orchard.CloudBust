using Orchard.UI.Resources;

namespace CloudBust.Resources {
    public class IziToast : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("IziToast")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/js/iziToast.min.js", "izitoast.js")
                    .SetVersion("1.3.0");

            manifest.DefineStyle("IziToast")
                    .SetVersion("1.3.0")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/css/iziToast.min.css", "izitoast.css");
        }
    }
}