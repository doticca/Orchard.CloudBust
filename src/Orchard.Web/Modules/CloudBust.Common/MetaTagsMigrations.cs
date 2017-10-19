using System.Data;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using CloudBust.Common.Models;

namespace CloudBust.Common
{
    [OrchardFeature("CloudBust.Common.SEO")]
    public class Migrations : DataMigrationImpl
	{
		public int Create()
		{
			SchemaBuilder.CreateTable(typeof (MetaTagsRecord).Name,
			                          table => table
				                                   .ContentPartRecord()
				                                   .Column("Keywords", DbType.String)
												   .Column("KeywordsInherited", DbType.Boolean)
				                                   .Column("Description", DbType.String)
												   .Column("DescriptionInherited", DbType.Boolean)
				                                   .Column("MetaTag1Name", DbType.String)
				                                   .Column("MetaTag1Value", DbType.String)
				                                   .Column("MetaTag1Inherited", DbType.Boolean)
				                                   .Column("MetaTag2Name", DbType.String)
				                                   .Column("MetaTag2Value", DbType.String)
				                                   .Column("MetaTag2Inherited", DbType.Boolean)
				);

			ContentDefinitionManager.AlterPartDefinition(
				typeof(MetaTagsPart).Name, cfg => cfg.Attachable().WithDescription("Adds Keywords, Description and 2 custom meta tags to your content item."));

			return 1;
		}
	}
}