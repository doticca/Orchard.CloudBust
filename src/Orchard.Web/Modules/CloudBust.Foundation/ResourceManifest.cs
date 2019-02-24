using Orchard.UI.Resources;

namespace CloudBust.Foundation {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("Icons_Foundation")
                    .SetUrl("foundation-icons.css");

            manifest.DefineScript("WhatInput")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/js/what-input.min.js",
                         "~/Modules/CloudBust.Foundation/Scripts/what-input.js")
                    .SetVersion("4.2.0")
                    .SetDependencies("jQuery");

            manifest.DefineScript("Foundation")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/js/foundation.6.5.1.min.js",
                         "~/Modules/CloudBust.Foundation/Scripts/foundation.6.5.1.js")
                    .SetVersion("6.5.1")
                    .SetDependencies("Whatinput");

            manifest.DefineStyle("CloudBust_FeaturedHead")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/css/featuredhead.min.css", "featuredhead.css");
        }
    }
}