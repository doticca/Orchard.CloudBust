using CloudBust.Application.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;

namespace CloudBust.Application.Handlers
{
    [OrchardFeature("CloudBust.Application.WebApp")]
    public class ApplicationSettingsPartHandler: ContentHandler {
        public ApplicationSettingsPartHandler(IRepository<ApplicationSettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<ApplicationSettingsPart>("Site"));

            OnInitializing<ApplicationSettingsPart>((context, part) =>
            {
                //part.WebIsCloudBust = false;
                part.ApplicationKey = string.Empty;
                part.ApplicationName = string.Empty;
            });
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("CloudBust")));
        }
    }
}