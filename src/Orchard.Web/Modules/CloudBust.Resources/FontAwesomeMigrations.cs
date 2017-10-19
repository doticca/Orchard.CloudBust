using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources
{
    [OrchardFeature("CloudBust.Resources.FontAwesome")]
    public class FontAwesomeMigrations : DataMigrationImpl {    
        public int Create()
        {
            SchemaBuilder.CreateTable(
                "FontAwesomeSettingsPartRecord",
                table => table
                             .ContentPartRecord()
                             .Column<bool>("AutoEnable", c => c.WithDefault(true))
                             .Column<bool>("AutoEnableAdmin", c => c.WithDefault(false))
                );
            return 1;
        }

    }
}