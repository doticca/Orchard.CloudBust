using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

// ReSharper disable InconsistentNaming

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.jQuery3")]
    public class jQuery : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // jQuery.
            manifest.DefineScript("jQuery").SetUrl("https://cloudbust.blob.core.windows.net/public/js/jquery-3.3.1.min.js", "jquery-3.3.1.js").SetVersion("3.3.1");

            // jQuery UI (full package).
            manifest.DefineScript("jQueryUI").SetUrl("https://cloudbust.blob.core.windows.net/public/js/jquery-ui.min.js", "jquery-ui.js").SetVersion("1.12.1").SetDependencies("jQuery");
            manifest.DefineStyle("jQueryUI").SetUrl("https://cloudbust.blob.core.windows.net/public/css/jquery-ui.min.css", "jquery-ui.css").SetVersion("1.12.1");            
        }
    }
}
