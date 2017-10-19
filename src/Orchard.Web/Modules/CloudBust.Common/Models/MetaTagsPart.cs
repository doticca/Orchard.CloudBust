using Orchard.ContentManagement;
using Orchard.Environment.Extensions;


namespace CloudBust.Common.Models
{
    [OrchardFeature("CloudBust.Common.SEO")]
    public class MetaTagsPart : ContentPart<MetaTagsRecord>
	{
		public string Keywords
		{
			get { return Record.Keywords; }
			set { Record.Keywords = value; }
		}

		public bool KeywordsInherited
		{
			get { return Record.KeywordsInherited; }
			set { Record.KeywordsInherited = value; }
		}


		public string Description
		{
			get { return Record.Description; }
			set { Record.Description = value; }
		}

		public bool DescriptionInherited
		{
			get { return Record.DescriptionInherited; }
			set { Record.DescriptionInherited = value; }
		}


		public string MetaTag1Name
		{
			get { return Record.MetaTag1Name; }
			set { Record.MetaTag1Name = value; }
		}

		public string MetaTag1Value
		{
			get { return Record.MetaTag1Value; }
			set { Record.MetaTag1Value = value; }
		}

		public bool MetaTag1Inherited
		{
			get { return Record.MetaTag1Inherited; }
			set { Record.MetaTag1Inherited = value; }
		}

		public string MetaTag2Name
		{
			get { return Record.MetaTag2Name; }
			set { Record.MetaTag2Name = value; }
		}

		public string MetaTag2Value
		{
			get { return Record.MetaTag2Value; }
			set { Record.MetaTag2Value = value; }
		}

		public bool MetaTag2Inherited
		{
			get { return Record.MetaTag2Inherited; }
			set { Record.MetaTag2Inherited = value; }
		}
	}
}