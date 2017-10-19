using CloudBust.Resources.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;


namespace CloudBust.Resources.Handlers
{
    [OrchardFeature("CloudBust.Resources.OwlCarousel")]
    public class OwlCarouselSettingsPartHandler : ContentHandler {
        public OwlCarouselSettingsPartHandler(IRepository<OwlCarouselSettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<OwlCarouselSettingsPart>("Site"));

            OnInitializing<OwlCarouselSettingsPart>((context, part) =>
            {
                part.AutoEnable = true;
                part.AutoEnableAdmin = true;
            });
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("OwlCarousel")));
        }
    }
}