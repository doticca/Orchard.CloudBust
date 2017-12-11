using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Core.Contents.Extensions;

namespace CloudBust.Foundation
{
    public class QuadPromo : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("QuadPromoPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("FirstImage", column => column.WithDefault(string.Empty))
                    .Column<string>("SecondImage", column => column.WithDefault(string.Empty))
                    .Column<string>("ThirdImage", column => column.WithDefault(string.Empty))
                    .Column<string>("FourthImage", column => column.WithDefault(string.Empty))
                    .Column<string>("FirstTitle", column => column.WithDefault(string.Empty))
                    .Column<string>("SecondTitle", column => column.WithDefault(string.Empty))
                    .Column<string>("ThirdTitle", column => column.WithDefault(string.Empty))
                    .Column<string>("FourthTitle", column => column.WithDefault(string.Empty))
                    .Column<string>("FirstPromoText", column => column.WithDefault(string.Empty))
                    .Column<string>("SecondPromoText", column => column.WithDefault(string.Empty))
                    .Column<string>("ThirdPromoText", column => column.WithDefault(string.Empty))
                    .Column<string>("FourthPromoText", column => column.WithDefault(string.Empty))
                    .Column<string>("FirstLinkText", column => column.WithDefault(string.Empty))
                    .Column<string>("SecondLinkText", column => column.WithDefault(string.Empty))
                    .Column<string>("ThirdLinkText", column => column.WithDefault(string.Empty))
                    .Column<string>("FourthLinkText", column => column.WithDefault(string.Empty))
                    .Column<string>("FirstLinkUrl", column => column.WithDefault(string.Empty))
                    .Column<string>("SecondLinkUrl", column => column.WithDefault(string.Empty))
                    .Column<string>("ThirdLinkUrl", column => column.WithDefault(string.Empty))
                    .Column<string>("FourthLinkUrl", column => column.WithDefault(string.Empty))
                    .Column<string>("FirstLinkColor", column => column.WithDefault(string.Empty))
                    .Column<string>("SecondLinkColor", column => column.WithDefault(string.Empty))
                    .Column<string>("ThirdLinkColor", column => column.WithDefault(string.Empty))
                    .Column<string>("FourthLinkColor", column => column.WithDefault(string.Empty))
                );

            // quad promo part
            ContentDefinitionManager.AlterPartDefinition("QuadPromoPart", builder => builder
                .Attachable()
                .WithDescription("Creates a responsive Quad Promo banner for Foundation framework.")
                );
            // quad promo widget
            ContentDefinitionManager.AlterTypeDefinition("QuadPromoWidget",
                cfg => cfg
                    .WithPart("QuadPromoPart")
                    .WithPart("CommonPart")
                    .WithPart("BodyPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            return 1;
        }
    }
}