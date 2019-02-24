using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.CookieCuttr")]
    public class CookieCuttr : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("CookieCuttr")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/js/jquery.cookiecuttr.min.js",
                         "jquery.cookiecuttr.js")
                    .SetDependencies("jQueryCookie", "jQuery")
                    .SetVersion("1.0.0");

            manifest.DefineStyle("CookieCuttr")
                    .SetUrl("https://cloudbust.blob.core.windows.net/public/css/cookiecuttr.min.css", "cookiecuttr.css")
                    .SetVersion("1.0.0");
        }
    }
}