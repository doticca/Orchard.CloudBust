using CloudBust.Resources.Models;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Highlight")]
    public class HighlightMigrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable(
                nameof(HighlightSettingsPartRecord),
                table => table
                        .ContentPartRecord()
                        .Column<bool>("AutoEnable", c => c.WithDefault(true))
                        .Column<bool>("AutoEnableAdmin", c => c.WithDefault(true))
                        .Column<bool>("FullBundle", c => c.WithDefault(false))
                        .Column<string>("Style", c => c.WithDefault("default"))
            );
            return 1;
        }
    }
}