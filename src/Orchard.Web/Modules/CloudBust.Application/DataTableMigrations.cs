using CloudBust.Application.Models;
using CloudBust.Application.Services;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using System;

namespace CloudBust.Application
{
    public class DataTableMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(ApplicationDataTableRecord).Name,
                            table => table
                                            .Column<int>("Id", column => column.PrimaryKey().Identity())
                                            .Column<string>("Name")
                                            .Column<string>("NormalizedTableName")
                                            .Column<string>("Description")
                                            .Column<DateTime>("CreatedUtc")
                                            .Column<DateTime>("ModifiedUtc")
            );

            SchemaBuilder.CreateTable(typeof(ApplicationApplicationDataTablesRecord).Name,
                        table => table
                                        .Column<int>("Id", column => column.PrimaryKey().Identity())
                                        .Column<int>("Application_id")
                                        .Column<int>("ApplicationDataTable_id")
            );

            SchemaBuilder.CreateTable(typeof(ApplicationDataTableFieldsRecord).Name,
                        table => table
                                        .Column<int>("Id", column => column.PrimaryKey().Identity())
                                        .Column<int>("ApplicationDataTable_id")
                                        .Column<int>("Field_id")
            );

            SchemaBuilder.CreateTable(typeof(ApplicationDataTableRowsRecord).Name,
                        table => table
                                        .Column<int>("Id", column => column.PrimaryKey().Identity())
                                        .Column<int>("ApplicationDataTable_id")
                                        .Column<int>("Row_id")
            );
            return 1;
        }
    }
}