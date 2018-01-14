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
                                .Column<string>("Name")
                                .Column<string>("NormalizedName")
                                .Column<string>("Description")
                                .Column<string>("FieldType", column => column.WithDefault("String"))
                                .Column<int>("Position")
            );

            SchemaBuilder.CreateTable(typeof(RowRecord).Name,
                table => table
                                .Column<int>("Id", column => column.PrimaryKey().Identity())
                                .Column<int>("Position")                                
            );


            SchemaBuilder.CreateTable(typeof(CellRecord).Name,
                        table => table
                                        .Column<int>("Id", column => column.PrimaryKey().Identity())
                                        .Column<int>("Row_id")
                                        .Column<int>("Field_id")
                                        .Column<int>("ValueId", column => column.NotNull())
            );
            return 1;
        }
    }
}