using CloudBust.Application.Models;
using Orchard.Data.Migration;
using System;

namespace CloudBust.Application
{
    public class FieldsMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(FieldRecord).Name,
                table => table
                                .Column<int>("Id", column => column.PrimaryKey().Identity())
                                .Column<int>("Position")
                                .Column<string>("Name")
                                .Column<string>("NormalizedName")
                                .Column<string>("Description")
                                .Column<string>("FieldType", column => column.WithDefault("String"))
            );

            SchemaBuilder.CreateTable(typeof(RowRecord).Name,
                table => table
                                .Column<int>("Id", column => column.PrimaryKey().Identity())
                                .Column<int>("Position")
            );

            SchemaBuilder.CreateTable(typeof(ColumnRecord).Name,
                table => table
                                .Column<int>("Id", column => column.PrimaryKey().Identity())
                                .Column<int>("FieldRecord_Id")
            );


            SchemaBuilder.CreateTable(typeof(RowColumnsRecord).Name,
                        table => table
                                        .Column<int>("Id", column => column.PrimaryKey().Identity())
                                        .Column<int>("Row_id")
                                        .Column<int>("Column_id")
            );
            return 1;
        }
    }
}