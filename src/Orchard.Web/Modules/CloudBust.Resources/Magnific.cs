using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Magnific")]
    public class Magnific : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // defaults at common highlight
            manifest.DefineScript("Magnific")
                .SetDependencies("jQuery", "jQueryMigrate")
                .SetUrl("magnific.min.js", "magnific.js")                
                .SetVersion("1.1.0");

            manifest.DefineStyle("Magnific")
                .SetVersion("1.1.0")
                .SetUrl("magnific.css", "magnific.css");
        }
    }
}
