using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources
{
    [OrchardFeature("CloudBust.Resources.Ace")]
    public class AceCssMigrations : DataMigrationImpl
    {

        public int Create()
        {
            SchemaBuilder.CreateTable(
                "CssPartRecord",
                table => table
                            .ContentPartRecord()
                            .Column<string>("Css", column => column.Unlimited())
                            );

            ContentDefinitionManager.AlterPartDefinition("CssPart", builder => builder
                .Attachable()
                .WithDescription("Allows the inline editing of Css using Ace editor."));

            ContentDefinitionManager.AlterTypeDefinition("HtmlWidget", 
                cfg => cfg
                    .WithPart("CssPart")
                );

            ContentDefinitionManager.AlterTypeDefinition("Page", 
                cfg => cfg
                    .WithPart("CssPart")
                );

            return 1;
        }
    }
}