using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using CloudBust.Foundation.Models;

namespace CloudBust.Foundation.Handlers {
    public class FeaturedHeadPartHandler : ContentHandler {
        public FeaturedHeadPartHandler(IRepository<FeaturedHeadPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));

            OnInitializing<FeaturedHeadPart>((context, part) =>
            {
                part.BackgroundColor = "#23b9d1";
                part.BackgroundColorMedium = "#bcd110";
                part.BackgroundColorLarge = "#efa412";
                part.ForegroundColor = "#fff";
                part.ForegroundColorMedium = "#fff";
                part.ForegroundColorLarge = "#fff";
            });
        }
    }
}