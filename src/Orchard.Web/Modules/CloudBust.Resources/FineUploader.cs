using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.FineUploader")]
    public class FineUploader : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // defaults at common highlight
            manifest.DefineScript("FineUploader")
                .SetUrl("fineuploader/fine-uploader.min.js", "fineuploader/fine-uploader.js")                
                .SetVersion("5.14.1");
            manifest.DefineScript("FineUploader_jQuery")
                .SetUrl("fineuploader/jquery.fine-uploader.min.js", "fineuploader/jquery.fine-uploader.js")
                .SetDependencies("jQuery")
                .SetVersion("5.14.1");

            manifest.DefineStyle("FineUploader")
                .SetUrl("fineuploader/fine-uploader-new.min.css", "fineuploader/fine-uploader-new.css")
                .SetVersion("5.14.1");
        }
    }
}
