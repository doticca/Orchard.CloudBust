using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Niceselect")]
    public class Niceselect : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // defaults at common highlight
            manifest.DefineScript("Niceselect")
                .SetDependencies("jQuery", "jQueryMigrate")
                .SetUrl("jquery.nice-select.min.js", "jquery.nice-select.js")                
                .SetVersion("1.0");
            manifest.DefineStyle("Niceselect")
                .SetUrl("nice-select.css")
                .SetVersion("1.0");
        }
    }
}
