using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.OwlCarousel")]
    public class OwlCarousel : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // defaults at common highlight
            manifest.DefineScript("OwlCarousel")
                .SetDependencies("jQuery", "jQueryMigrate")
                .SetUrl("owl.carousel.min.js")                
                .SetVersion("1.3.3");

            manifest.DefineStyle("OwlCarousel_Core")
                .SetVersion("1.3.3")
                .SetUrl("owlcarousel/owl.carousel.css");
            manifest.DefineStyle("OwlCarousel")
                .SetVersion("1.3.3")
                .SetDependencies("OwlCarousel_Core")
                .SetUrl("owlcarousel/owl.theme.css");
        }
    }
}
