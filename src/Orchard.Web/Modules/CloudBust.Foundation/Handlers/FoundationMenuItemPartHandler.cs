using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using CloudBust.Foundation.Models;

namespace CloudBust.Foundation.Handlers {
    public class FoundationMenuItemPartHandler : ContentHandler {
        public FoundationMenuItemPartHandler(IRepository<FoundationMenuItemPartRecord> repository)
        {
            Filters.Add(new ActivatingFilter<FoundationMenuItemPart>("MenuItem"));
            Filters.Add(StorageFilter.For(repository));
        }
    }
}