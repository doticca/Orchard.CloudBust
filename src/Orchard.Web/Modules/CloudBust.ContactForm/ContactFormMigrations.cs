using CloudBust.ContactForm.Models;
using Orchard.Data.Migration;

namespace CloudBust.ContactForm
{
    public class ContactFormMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(ContactFormRecord).Name,
                table => table
                        .Column<int>("Id", column => column.PrimaryKey().Identity())
                        .Column<string>("Email")
                        .Column<string>("Name")
                        .Column<string>("Company")
                        .Column<string>("Note")
            );            

            return 1;
        }
    }
}