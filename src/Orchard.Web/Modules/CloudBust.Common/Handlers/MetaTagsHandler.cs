using CloudBust.Common.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;


namespace CloudBust.Common.Handlers
{
    [OrchardFeature("CloudBust.Common.SEO")]
    public class MetaTagsHandler : ContentHandler
	{
        public Localizer T { get; set; }

		public MetaTagsHandler(IRepository<MetaTagsRecord> repository)
		{
            T = NullLocalizer.Instance;

            Filters.Add(new ActivatingFilter<MetaTagsPart>("Site"));
			Filters.Add(StorageFilter.For(repository));
		}

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site") {
                return;
            }

            base.GetItemMetadata(context);
            
            var groupInfo = new GroupInfo(T("Meta Tags"));
            groupInfo.Id = "MetaTags";

            context.Metadata.EditorGroupInfo.Add(groupInfo);
        }
	}
}