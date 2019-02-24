using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.FontAwesome")]
    public class FontAwesome : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineStyle("FontAwesome")
                .SetUrl("https://cloudbust.blob.core.windows.net/public/css/font-awesome.min.css", "font-awesome.css")
                .SetVersion("4.4.1");
        }
    }
}
