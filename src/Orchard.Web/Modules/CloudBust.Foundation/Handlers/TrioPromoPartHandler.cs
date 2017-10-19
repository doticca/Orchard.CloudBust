using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using CloudBust.Foundation.Models;

namespace CloudBust.Foundation.Handlers {
    public class TrioPromoPartHandler : ContentHandler {
        public TrioPromoPartHandler(IRepository<TrioPromoPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));

            OnInitializing<TrioPromoPart>((context, part) =>
            {
                part.FirstImage = string.Empty;
                part.SecondImage = string.Empty;
                part.ThirdImage = string.Empty;
                part.FirstTitle = string.Empty;
                part.SecondTitle = string.Empty;
                part.ThirdTitle = string.Empty;
                part.FirstLinkText = string.Empty;
                part.SecondLinkText = string.Empty;
                part.ThirdLinkText = string.Empty;
                part.FirstLinkUrl = string.Empty;
                part.SecondLinkUrl = string.Empty;
                part.ThirdLinkUrl = string.Empty;
                part.FirstPromoText = string.Empty;
                part.SecondPromoText = string.Empty;
                part.ThirdPromoText = string.Empty;
                part.FirstLinkColor = string.Empty;
                part.SecondLinkColor = string.Empty;
                part.ThirdLinkColor = string.Empty;
            });
        }
    }
}