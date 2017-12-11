using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace CloudBust.Foundation
{
    public class Foundation : DataMigrationImpl
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
                    .Column<int>("GridStyle", column => column.WithDefault(0))
                );
            return 1;
        }
    }


}