using CloudBust.Subscribers.Models;
using Orchard.Data.Migration;

namespace CloudBust.Subscribers
{
    public class SubscribersMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(SubscriberRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Email", column => column.NotNull())
            );            

            return 1;
        }
    }
}