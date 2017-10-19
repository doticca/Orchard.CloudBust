using CloudBust.Resources.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;

namespace CloudBust.Resources.Handlers
{
    [OrchardFeature("CloudBust.Resources.GoogleMaps")]
    public class GoogleMapsSettingsPartHandler: ContentHandler {
        public GoogleMapsSettingsPartHandler(IRepository<GoogleMapsSettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<GoogleMapsSettingsPart>("Site"));

            OnInitializing<GoogleMapsSettingsPart>((context, part) =>
            {
                part.AutoEnable = true;
                part.AutoEnableAdmin = false;
                part.ApiKey = string.Empty;
                part.Async = false;
                part.CallBack = string.Empty;
                part.Defer = false;
                part.Language = "en";
                part.Sensor = false;             
            });
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("GoogleMaps")));
        }
    }
}