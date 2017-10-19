using CloudBust.Resources.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;

namespace CloudBust.Resources.Handlers
{
    [OrchardFeature("CloudBust.Resources.Niceselect")]
    public class NiceselectSettingsPartHandler: ContentHandler {
        public NiceselectSettingsPartHandler(IRepository<NiceselectSettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<NiceselectSettingsPart>("Site"));

            OnInitializing<NiceselectSettingsPart>((context, part) =>
            {
                part.AutoEnable = true;
            });
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Niceselect")));
        }
    }
}