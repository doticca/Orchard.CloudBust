using Orchard.UI.Resources;

namespace CloudBust.Blogs
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            manifest.DefineScript("Grapto_Viewers").SetUrl("viewers.js").SetDependencies("ShapesBase");
            manifest.DefineStyle("Grapto_Viewers").SetUrl("viewers.css");
        }
    }
}