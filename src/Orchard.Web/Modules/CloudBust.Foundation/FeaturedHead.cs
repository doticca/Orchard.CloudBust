using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Core.Contents.Extensions;

namespace CloudBust.Foundation
{
    public class FeaturedHead : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("FeaturedHeadPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("BackgroundColor", column => column.WithDefault("#23b9d1"))
                    .Column<string>("ForegroundColor", column => column.WithDefault("#fff"))
                    .Column<string>("BackgroundColorMedium", column => column.WithDefault("#bcd110"))
                    .Column<string>("ForegroundColorMedium", column => column.WithDefault("#fff"))
                    .Column<string>("BackgroundColorLarge", column => column.WithDefault("#efa412"))
                    .Column<string>("ForegroundColorLarge", column => column.WithDefault("#fff"))
                    .Column<string>("BackgroundImage", column => column.WithDefault(string.Empty))
                    .Column<string>("BackgroundImageMedium", column => column.WithDefault(string.Empty))
                    .Column<string>("BackgroundImageLarge", column => column.WithDefault(string.Empty))
                );

            ContentDefinitionManager.AlterPartDefinition("FeaturedHeadPart", 
                builder => builder
                    .Attachable()
                    .WithDescription("Creates a responsive featured Header for Foundation framework.")
                );

            ContentDefinitionManager.AlterTypeDefinition("FeaturedHeadWidget",
                cfg => cfg
                    .WithPart("FeaturedHeadPart")
                    .WithPart("CommonPart")
                    .WithPart("BodyPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            return 1;
        }
    }
}