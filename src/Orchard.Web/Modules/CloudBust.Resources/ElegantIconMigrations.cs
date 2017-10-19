using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources
{
    [OrchardFeature("CloudBust.Resources.ElegantIcon")]
    public class ElegantIconMigrations : DataMigrationImpl {    
        public int Create()
        {
            SchemaBuilder.CreateTable(
                "ElegantIconSettingsPartRecord",
                table => table
                             .ContentPartRecord()
                             .Column<bool>("AutoEnable", c => c.WithDefault(true))
                             .Column<bool>("AutoEnableAdmin", c => c.WithDefault(false))
                );
            return 1;
        }

    }
}