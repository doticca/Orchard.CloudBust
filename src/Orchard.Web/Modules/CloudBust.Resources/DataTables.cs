using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.DataTables")]
    public class DataTables : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("DataTables_Base")
                .SetUrl("datatables/jquery.dataTables.min.js", "datatables/jquery.dataTables.js")
                .SetDependencies("jQuery")
                .SetVersion("1.10.16");

            manifest.DefineScript("DataTables_Foundation")
                .SetUrl("datatables/dataTables.foundation.min.js", "datatables/dataTables.foundation.js")
                .SetDependencies("DataTables_Base")
                .SetVersion("1.10.16");

            manifest.DefineScript("DataTables_Responsive")
                .SetUrl("datatables/dataTables.responsive.min.js", "datatables/dataTables.responsive.js")
                .SetDependencies("DataTables_Base")
                .SetVersion("1.10.16");

            manifest.DefineScript("DataTables")
                .SetUrl("datatables/responsive.foundation.min.js", "datatables/responsive.foundation.js")
                .SetDependencies("DataTables_Foundation", "DataTables_Responsive")
                .SetVersion("1.10.16");




            manifest.DefineStyle("DataTables_Base")
                .SetUrl("datatables/css/dataTables.foundation.min.css", "datatables/css/dataTables.foundation.css")
                .SetVersion("1.10.16");

            manifest.DefineStyle("DataTables")
                .SetUrl("datatables/css/responsive.foundation.min.css", "datatables/css/responsive.foundation.css")
                .SetDependencies("DataTables_Base")
                .SetVersion("1.10.16");
        }
    }
}
