using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources
{
    [OrchardFeature("CloudBust.Resources.GoogleMaps")]
    public class GoogleMapsMigrations : DataMigrationImpl {    
        public int Create()
        {
            SchemaBuilder.CreateTable(
                "GoogleMapsSettingsPartRecord",
                table => table
                             .ContentPartRecord()
                             .Column<string>("ApiKey")
                             .Column<string>("CallBack")
                             .Column<string>("Language", c => c.WithDefault("en"))
                             .Column<bool>("Sensor", c => c.WithDefault(false))
                             .Column<bool>("Async", c => c.WithDefault(true))
                             .Column<bool>("Defer", c => c.WithDefault(true))
                             .Column<bool>("AutoEnable", c => c.WithDefault(true))
                             .Column<bool>("AutoEnableAdmin", c => c.WithDefault(false))
                );
            return 1;
        }

    }
}