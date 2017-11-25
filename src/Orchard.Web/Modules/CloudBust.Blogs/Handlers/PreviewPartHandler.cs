using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using CloudBust.Blogs.Models;

namespace CloudBust.Blogs.Handlers
{
    public class PreviewPartHandler : ContentHandler
    {

        public PreviewPartHandler(IRepository<PreviewPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
