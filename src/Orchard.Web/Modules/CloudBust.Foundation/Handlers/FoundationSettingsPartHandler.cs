using CloudBust.Foundation.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;

namespace CloudBust.Foundation.Handlers
{
    public class FoundationSettingsPartHandler: ContentHandler {
        public FoundationSettingsPartHandler(IRepository<FoundationSettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<FoundationSettingsPart>("Site"));

            OnInitializing<FoundationSettingsPart>((context, part) =>
            {
                part.AutoEnableAdmin = true;
                part.DoNotEnableFrontEnd = false;
                part.UseDatePicker = true;
                part.UseSelect = true;
                part.UseIcons = true;
                part.UseNicescroll = true;
                part.UsePlaceholder = true;
            });
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Foundation")));
        }
    }
}