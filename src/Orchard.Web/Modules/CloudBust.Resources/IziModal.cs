using Orchard.UI.Resources;

namespace CloudBust.Resources {
    public class IziModal : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("IziModal")
                    .SetDependencies("jQuery")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/js/iziModal.min.js", "izimodal.js")
                    .SetVersion("1.6.0");

            manifest.DefineStyle("IziModal")
                    .SetVersion("1.6.0")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/css/iziModal.min.css", "izimodal.css");
        }
    }
}