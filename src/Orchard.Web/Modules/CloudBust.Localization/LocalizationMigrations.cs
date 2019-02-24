using CloudBust.Localization.Models;
using Orchard.Data.Migration;

namespace CloudBust.Localization
{
    public class LocalizationMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(TranslationRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name", column => column.NotNull())
                    .Column<string>("TwoDigitCode", column => column.NotNull())
            );
            SchemaBuilder.CreateTable(typeof(CurrencyRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name", column => column.NotNull())
                    .Column<string>("ThreeDigitCode", column => column.NotNull())
                    .Column<string>("Sign")
                    .Column<bool>("ShowSignAfter", column => column.WithDefault(false))
            );
            SchemaBuilder.CreateTable(typeof(CountryRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name", column => column.NotNull())
                    .Column<string>("ThreeDigitCode", column => column.NotNull())
                    .Column<string>("TwoDigitCode", column => column.NotNull())
                    .Column<int>("Currency_id", column => column.NotNull())
                    .Column<int>("Taxation", column => column.NotNull())
                    .Column<int>("Translation_id", column => column.NotNull())
            );

            SchemaBuilder.CreateTable(typeof(CountryTranslationRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name", column => column.NotUnique().Nullable())
                    .Column<int>("Country_id", column => column.NotNull())
                    .Column<int>("Translation_id", column => column.NotNull())
                    .Column<int>("Position", column => column.NotNull())
            );

            return 1;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder
                .AlterTable(typeof(CountryRecord).Name, 
                    table => table.CreateIndex("IDX_Country_Name", "Name"))
                .AlterTable(typeof(CountryRecord).Name, 
                    table => table.AddUniqueConstraint("UC_Country_Name", "Name"))
                .AlterTable(typeof(CountryRecord).Name,
                    table => table.CreateIndex("IDX_Country_TwoDigitCode", "TwoDigitCode"))
                .AlterTable(typeof(CountryRecord).Name,
                    table => table.AddUniqueConstraint("UC_Country_TwoDigitCode", "TwoDigitCode"))
                .AlterTable(typeof(CountryRecord).Name,
                    table => table.CreateIndex("IDX_Country_ThreeDigitCode", "ThreeDigitCode"))
                .AlterTable(typeof(CountryRecord).Name,
                    table => table.AddUniqueConstraint("UC_Country_ThreeDigitCode", "ThreeDigitCode"))
                .AlterTable(typeof(CurrencyRecord).Name, 
                    table => table.CreateIndex("IDX_Currency_Name", "Name"))
                .AlterTable(typeof(CurrencyRecord).Name, 
                    table => table.AddUniqueConstraint("UC_Currency_Name", "Name"))
                //.AlterTable(typeof(CurrencyRecord).Name,
                //    table => table.CreateIndex("IDX_Currency_ThreeDigitCode", "ThreeDigitCode"))
                .AlterTable(typeof(CurrencyRecord).Name,
                    table => table.AddUniqueConstraint("UC_Currency_ThreeDigitCode", "ThreeDigitCode"))
                //.AlterTable(typeof(TranslationRecord).Name, 
                //    table => table.CreateIndex("IDX_Translation_Name", "Name"))
                .AlterTable(typeof(TranslationRecord).Name,
                    table => table.AddUniqueConstraint("UC_Translation_Name", "Name"))
                //.AlterTable(typeof(TranslationRecord).Name,
                //    table => table.CreateIndex("IDX_Translation_ThreeDigitCode", "TwoDigitCode"))
                .AlterTable(typeof(TranslationRecord).Name,
                    table => table.AddUniqueConstraint("UC_Translation_TwoDigitCode", "TwoDigitCode"))
                .AlterTable(typeof(CountryTranslationRecord).Name,
                    table => table.AddUniqueConstraint("UC_CountryID_TranslationID", "Country_id", "Translation_id"))
                .AlterTable(typeof(CountryTranslationRecord).Name,
                    table => table.AddUniqueConstraint("UC_CountryID_Position", "Country_id", "Position"));
            return 2;
        }
    }
}