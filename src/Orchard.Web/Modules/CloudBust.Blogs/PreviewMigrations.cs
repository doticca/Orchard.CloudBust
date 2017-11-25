using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace CloudBust.Blogs
{
    public class PreviewMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(
                "PreviewPartRecord",
                table => table
                            .ContentPartRecord()
                            .Column<string>("PreviewText", column => column.Unlimited())
                            );

            ContentDefinitionManager.AlterPartDefinition("PreviewPart", builder => builder
                .Attachable()
                .WithDescription("Attach a Preview Part to show on Summary."));

            ContentDefinitionManager.AlterTypeDefinition("BlogPost",
                cfg => cfg
                    .WithPart("PreviewPart")
                );

            return 1;
        }
    }
}