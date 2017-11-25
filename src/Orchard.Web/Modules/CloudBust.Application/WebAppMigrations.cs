using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Application
{
    [OrchardFeature("CloudBust.Application.WebApp")]
    public class WebAppMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("ApplicationSettingsPartRecord",
            table => table
                            .ContentPartRecord()
                            //.Column<bool>("WebIsCloudBust", c => c.WithDefault(false))
                            .Column<string>("ApplicationKey")
             );

            return 1;
        }
        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable("ApplicationSettingsPartRecord", table => table
                .AddColumn<string>("ApplicationName"));
            return 2;
        }
    }


}