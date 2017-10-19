using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.DropZone")]
    public class Dropzone : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // defaults at common highlight
            manifest.DefineScript("Dropzone")
                .SetUrl("dropzone.js", "dropzone.js")                
                .SetVersion("4.0");
        }
    }
}
