using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs
{
    [OrchardFeature("CloudBust.Blogs.TaxonomiesCloud")]
    public class TaxonomyCloud : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition("TaxonomyCloudPart",
                builder => builder
                    .Attachable()
                    .WithDescription("Creates a Terms view for taxonomy with name Topics.")
                );

            ContentDefinitionManager.AlterTypeDefinition("TaxonomyCloudWidget",
                cfg => cfg
                    .WithPart("TaxonomyCloudPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            return 1;
        }
    }
}