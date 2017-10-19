using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Common {

    [OrchardFeature("CloudBust.Common.SEO")]
    public class ApplinksMigrations : DataMigrationImpl {
		public int Create() {
			SchemaBuilder.CreateTable("ApplinksFileRecord",
				table => table
					.Column<int>("Id", col => col.PrimaryKey().Identity())
					.Column<string>("FileContent", col => col.Nullable().Unlimited().WithDefault(@"{}"))
				);
			return 1;
		}
	}
}