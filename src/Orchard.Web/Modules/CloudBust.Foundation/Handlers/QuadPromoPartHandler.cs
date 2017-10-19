using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using CloudBust.Foundation.Models;

namespace CloudBust.Foundation.Handlers {
    public class QuadPromoPartHandler : ContentHandler {
        public QuadPromoPartHandler(IRepository<QuadPromoPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));

            OnInitializing<QuadPromoPart>((context, part) =>
            {
                part.FirstImage = string.Empty;
                part.SecondImage = string.Empty;
                part.ThirdImage = string.Empty;
                part.FourthImage = string.Empty;
                part.FirstTitle = string.Empty;
                part.SecondTitle = string.Empty;
                part.ThirdTitle = string.Empty;
                part.FourthTitle = string.Empty;
                part.FirstLinkText = string.Empty;
                part.SecondLinkText = string.Empty;
                part.ThirdLinkText = string.Empty;
                part.FourthLinkText = string.Empty;
                part.FirstLinkUrl = string.Empty;
                part.SecondLinkUrl = string.Empty;
                part.ThirdLinkUrl = string.Empty;
                part.FourthLinkUrl = string.Empty;
                part.FirstPromoText = string.Empty;
                part.SecondPromoText = string.Empty;
                part.ThirdPromoText = string.Empty;
                part.FourthPromoText = string.Empty;
                part.FirstLinkColor = string.Empty;
                part.SecondLinkColor = string.Empty;
                part.ThirdLinkColor = string.Empty;
                part.FourthLinkColor = string.Empty;
            });
        }
    }
}