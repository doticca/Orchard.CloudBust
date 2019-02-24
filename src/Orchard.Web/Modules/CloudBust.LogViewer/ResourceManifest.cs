using Orchard.UI.Resources;

namespace CloudBust.LogViewer
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineScript("LogViewer/Index").SetUrl("logviewer.index.js").SetDependencies("jQuery");
        }
    }
}
