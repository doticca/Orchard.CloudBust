using CloudBust.Resources.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Resources.Handlers
{
    [OrchardFeature("CloudBust.Resources.Masonry")]
    public class MasonrySettingsPartHandler: ContentHandler {
        public MasonrySettingsPartHandler(IRepository<MasonrySettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<MasonrySettingsPart>("Site"));

            OnInitializing<MasonrySettingsPart>((context, part) =>
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
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Masonry")));
        }
    }
}