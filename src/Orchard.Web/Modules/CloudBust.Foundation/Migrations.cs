using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace CloudBust.Foundation
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("FoundationSettingsPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<bool>("AutoEnableAdmin", c => c.WithDefault(false))
                    .Column<bool>("DoNotEnableFrontEnd", column => column.WithDefault(false))
                    .Column<bool>("UseDatePicker", column => column.WithDefault(true))
                    .Column<bool>("UseSelect", column => column.WithDefault(true))
                    .Column<bool>("UseIcons", column => column.WithDefault(true))
                    .Column<bool>("UsePlaceholder", column => column.WithDefault(true))
                    .Column<bool>("UseNicescroll", column => column.WithDefault(true))
                );
            SchemaBuilder.CreateTable("FoundationMenuItemPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<bool>("DisplayAsButton", column => column.WithDefault(false))
                    .Column<bool>("isRoot")
                    .Column<bool>("LeftSide", column => column.WithDefault(false))
                    .Column<bool>("Divider", column => column.WithDefault(false))
                );
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
            SchemaBuilder.CreateTable("FeaturedHeadPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<int>("HeightLarge", column => column.WithDefault(650))
                    .Column<int>("HeightMedium", column => column.WithDefault(525))
                    .Column<string>("BackgroundColor", column => column.WithDefault("#23b9d1"))
                    .Column<string>("ForegroundColor", column => column.WithDefault("#fff"))
                    .Column<string>("BackgroundColorMedium", column => column.WithDefault("#bcd110"))
                    .Column<string>("ForegroundColorMedium", column => column.WithDefault("#fff"))
                    .Column<string>("BackgroundColorLarge", column => column.WithDefault("#efa412"))
                    .Column<string>("ForegroundColorLarge", column => column.WithDefault("#fff"))
                    .Column<string>("BackgroundImageMedium", column => column.WithDefault(string.Empty))
                    .Column<string>("BackgroundImageLarge", column => column.WithDefault(string.Empty))
                );
            // foundation content menu item
            ContentDefinitionManager.AlterTypeDefinition("ContentMenuItem", cfg => cfg
                .WithPart("FoundationMenuItemPart")
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
            // featured head
            ContentDefinitionManager.AlterPartDefinition("FeaturedHeadPart", builder => builder
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