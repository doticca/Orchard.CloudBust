using CloudBust.Application.Models;
using Orchard.Data.Migration;
using System;

namespace CloudBust.Application
{
    public class FieldsValuesMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("StringFieldValueRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Value", c => c.WithLength(4000))
                    .Column<int>("FieldRecord_id")
            );

            SchemaBuilder.CreateTable("IntegerFieldValueRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<long>("Value", c => c.Nullable())
                    .Column<int>("FieldRecord_id")
            );

            SchemaBuilder.CreateTable("DoubleFieldValueRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<double>("Value", c => c.Nullable())
                    .Column<int>("FieldRecord_id")
            );

            SchemaBuilder.CreateTable("BoolFieldValueRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<bool>("Value", c => c.Nullable())
                    .Column<int>("FieldRecord_id")
            );

            SchemaBuilder.CreateTable("DateTimeFieldValueRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<DateTime>("Value", c => c.Nullable())
                    .Column<int>("FieldRecord_id")
            );
            return 1;
        }
    }
}