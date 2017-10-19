using CloudBust.Resources.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;


namespace CloudBust.Resources.Handlers
{
    [OrchardFeature("CloudBust.Resources.ElegantIcon")]
    public class ElegantIconSettingsPartHandler : ContentHandler {
        public ElegantIconSettingsPartHandler(IRepository<ElegantIconSettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<ElegantIconSettingsPart>("Site"));

            OnInitializing<ElegantIconSettingsPart>((context, part) =>
            {
                part.AutoEnable = true;
                part.AutoEnableAdmin = false;
            });
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("ElegantIcon")));
        }
    }
}