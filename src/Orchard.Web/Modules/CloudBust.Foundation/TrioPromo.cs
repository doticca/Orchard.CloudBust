using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Core.Contents.Extensions;

namespace CloudBust.Foundation
{
    public class TrioPromo : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("TrioPromoPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("FirstImage", column => column.WithDefault(string.Empty))
                    .Column<string>("SecondImage", column => column.WithDefault(string.Empty))
                    .Column<string>("ThirdImage", column => column.WithDefault(string.Empty))
                    .Column<string>("FirstTitle", column => column.WithDefault(string.Empty))
                    .Column<string>("SecondTitle", column => column.WithDefault(string.Empty))
                    .Column<string>("ThirdTitle", column => column.WithDefault(string.Empty))
                    .Column<string>("FirstPromoText", column => column.Unlimited())
                    .Column<string>("SecondPromoText", column => column.Unlimited())
                    .Column<string>("ThirdPromoText", column => column.Unlimited())
                    .Column<string>("FirstLinkText", column => column.WithDefault(string.Empty))
                    .Column<string>("SecondLinkText", column => column.WithDefault(string.Empty))
                    .Column<string>("ThirdLinkText", column => column.WithDefault(string.Empty))
                    .Column<string>("FirstLinkUrl", column => column.WithDefault(string.Empty))
                    .Column<string>("SecondLinkUrl", column => column.WithDefault(string.Empty))
                    .Column<string>("ThirdLinkUrl", column => column.WithDefault(string.Empty))
                    .Column<string>("FirstLinkColor", column => column.WithDefault(string.Empty))
                    .Column<string>("SecondLinkColor", column => column.WithDefault(string.Empty))
                    .Column<string>("ThirdLinkColor", column => column.WithDefault(string.Empty))
                );

            // trio promo part
            ContentDefinitionManager.AlterPartDefinition("TrioPromoPart", builder => builder
                .Attachable()
                .WithDescription("Creates a responsive Trio Promo banner for Foundation framework.")
                );
            // trio promo widget
            ContentDefinitionManager.AlterTypeDefinition("TrioPromoWidget",
                cfg => cfg
                    .WithPart("TrioPromoPart")
                    .WithPart("CommonPart")
                    .WithPart("BodyPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            return 1;
        }
    }
}