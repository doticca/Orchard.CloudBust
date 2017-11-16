using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Common {
    public class FaviconMigrations : DataMigrationImpl {

        public int Create() {
            SchemaBuilder.CreateTable(
                "FaviconSettingsPartRecord",
                table => table
                             .ContentPartRecord()
                             .Column<string>("FaviconUrl")
                             .Column<string>("AppleTouchIconUrl")
                             .Column<string>("PngImageUrl")
                             .Column<string>("AndroidManifestUrl")
                             .Column<string>("SafariPinnedUrl")
                             .Column<string>("SafariPinnedMask")
                             .Column<string>("MSApplicationConfigUrl")
                             .Column<string>("ThemeColor")
                );
            return 2;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable("FaviconSettingsPartRecord", table => table
                .AddColumn<string>("AppleTouchIconUrl")
            );
            SchemaBuilder.AlterTable("FaviconSettingsPartRecord", table => table
                .AddColumn<string>("PngImageUrl")
            );
            SchemaBuilder.AlterTable("FaviconSettingsPartRecord", table => table
                .AddColumn<string>("AndroidManifestUrl")
            );
            SchemaBuilder.AlterTable("FaviconSettingsPartRecord", table => table
                .AddColumn<string>("SafariPinnedUrl")
            );
            SchemaBuilder.AlterTable("FaviconSettingsPartRecord", table => table
                .AddColumn<string>("SafariPinnedMask")
            );
            SchemaBuilder.AlterTable("FaviconSettingsPartRecord", table => table
                .AddColumn<string>("MSApplicationConfigUrl")
            );
            SchemaBuilder.AlterTable("FaviconSettingsPartRecord", table => table
                .AddColumn<string>("ThemeColor")
            );

            return 2;
        }
    }
}