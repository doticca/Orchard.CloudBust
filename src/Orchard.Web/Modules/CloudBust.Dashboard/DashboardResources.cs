using Orchard.UI.Resources;

namespace CloudBust.Dashboard
{
    public class DashboardResources : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("TypeAhead") 
                    .SetDependencies("Foundation")
                    .SetUrl("typeahead.js?20190205.1", "typeahead.js?20190205.1")
                    .SetVersion("0.9.0");

            manifest.DefineScript("CB_Dashboard")
                .SetDependencies("Underscore", "TypeAhead", "IziToast", "IziModal", "Foundation")
                .SetUrl("cloudbust.dashboard.js?20190205.1", "cloudbust.dashboard.js?20190205.1")
                .SetVersion("0.9.0");
        }
    }
}
