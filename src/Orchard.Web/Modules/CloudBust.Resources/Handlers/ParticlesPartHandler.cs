using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using CloudBust.Resources.Models;

namespace CloudBust.Resources.Handlers {
    [OrchardFeature("CloudBust.Resources.Particles")]
    public class FeaturedHeadPartHandler : ContentHandler {
        public FeaturedHeadPartHandler(IRepository<ParticlesPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}