using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;


namespace CloudBust.Common.Models
{
    [OrchardFeature("CloudBust.Common.SEO")]
    public class MetaTagsRecord : ContentPartRecord
	{
		public virtual string Keywords { get; set; }
		
		public virtual bool KeywordsInherited { get; set; }

		
		public virtual string Description { get; set; }
		
		public virtual bool DescriptionInherited { get; set; }
		
		
		public virtual string MetaTag1Name { get; set; }

		public virtual string MetaTag1Value { get; set; }

		public virtual bool MetaTag1Inherited { get; set; }


		public virtual string MetaTag2Name { get; set; }

		public virtual string MetaTag2Value { get; set; }

		public virtual bool MetaTag2Inherited { get; set; }
	}
}