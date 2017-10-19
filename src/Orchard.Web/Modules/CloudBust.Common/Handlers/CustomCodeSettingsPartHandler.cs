using CloudBust.Common.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Handlers {
    [OrchardFeature("CloudBust.Common.CustomCode")]
    public class CustomCodeSettingsPartHandler : ContentHandler {
        public CustomCodeSettingsPartHandler() {
            T = NullLocalizer.Instance;
            Filters.Add(new ActivatingFilter<CustomCodeSettingsPart>("Site"));
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Custom code")) { Id = "CustomCode", Position = "15" } );
        }
    }
}