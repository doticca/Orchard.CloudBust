using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Common {

    [OrchardFeature("CloudBust.Common.SEO")]
    public class RobotsMigrations : DataMigrationImpl {
		public int Create() {
			SchemaBuilder.CreateTable("RobotsFileRecord",
				table => table
					.Column<int>("Id", col => col.PrimaryKey().Identity())
					.Column<string>("FileContent", col => col.Nullable().Unlimited().WithDefault(@"User-agent: *
Allow: /"))
				);
			return 1;
		}
	}
}