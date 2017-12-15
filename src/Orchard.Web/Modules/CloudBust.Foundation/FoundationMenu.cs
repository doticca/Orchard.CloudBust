using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace CloudBust.Foundation
{
    public class FoundationMenu : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("FoundationMenuItemPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<bool>("DisplayAsButton", column => column.WithDefault(false))
                    .Column<bool>("isRoot")
                    .Column<bool>("LeftSide", column => column.WithDefault(false))
                    .Column<bool>("Divider", column => column.WithDefault(false))
                );



            // foundation content menu item
            ContentDefinitionManager.AlterTypeDefinition("ContentMenuItem", cfg => cfg
                .WithPart("FoundationMenuItemPart")
                );



            return 1;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable("FoundationMenuItemPartRecord",
                table => table
                    .AddColumn<bool>("RightSide", column => column.WithDefault(false))
                );
            SchemaBuilder.AlterTable("FoundationMenuItemPartRecord",
                table => table
                    .AddColumn<string>("CustomCss")
                );

            return 2;
        }
    }


}