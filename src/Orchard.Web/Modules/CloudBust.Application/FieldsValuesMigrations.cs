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
                    .Column<int>("ColumnRecord_Id")
            );

            SchemaBuilder.CreateTable("IntegerFieldValueRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<long>("Value")
                    .Column<int>("ColumnRecord_Id")
            );

            SchemaBuilder.CreateTable("DoubleFieldValueRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<double>("Value")
                    .Column<int>("ColumnRecord_Id")
            );

            SchemaBuilder.CreateTable("BoolFieldValueRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<bool>("Value")
                    .Column<int>("ColumnRecord_Id")
            );

            SchemaBuilder.CreateTable("DateTimeFieldValueRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<DateTime>("Value")
                    .Column<int>("ColumnRecord_Id")
            );
            return 1;
        }
    }
}