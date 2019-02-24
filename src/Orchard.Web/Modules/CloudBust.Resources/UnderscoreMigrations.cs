using CloudBust.Resources.Models;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Underscore")]
    public class UnderscoreMigrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable(
                nameof(UnderscoreSettingsPartRecord),
                table => table
                        .ContentPartRecord()
                        .Column<bool>("AutoEnable", c => c.WithDefault(true))
                        .Column<bool>("AutoEnableAdmin", c => c.WithDefault(true))
            );
            return 1;
        }
    }
}