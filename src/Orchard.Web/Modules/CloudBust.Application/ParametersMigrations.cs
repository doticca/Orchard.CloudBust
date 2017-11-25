using CloudBust.Application.Models;
using Orchard.Data.Migration;
using System;

namespace CloudBust.Application
{
    public class ParametersMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(ParameterRecord).Name,
                table => table
                                .Column<int>("Id", column => column.PrimaryKey().Identity())
                                .Column<int>("ApplicationRecord_id")
                                .Column<string>("Name")
                                .Column<string>("NormalizedName")
                                .Column<string>("Description")
                                .Column<string>("ParameterType", column => column.WithDefault("String"))
                                .Column<string>("ParameterValueString", column => column.Unlimited().WithDefault(string.Empty))
                                .Column<bool>("ParameterValueBool", c => c.WithDefault(null))
                                .Column<double>("ParameterValueDouble", c => c.WithDefault(null))
                                .Column<int>("ParameterValueInt", c => c.WithDefault(null))
                                .Column<DateTime>("ParameterValueDateTime", c => c.WithDefault(null))
            );

            SchemaBuilder.CreateTable(typeof(ParameterCategoryRecord).Name,
                table => table
                                .Column<int>("Id", column => column.PrimaryKey().Identity())
                                .Column<int>("ApplicationRecord_id")
                                .Column<string>("Name")
                                .Column<string>("NormalizedName")
                                .Column<string>("Description")
            );

            SchemaBuilder.CreateTable(typeof(ParameterParameterCategoriesRecord).Name,
            table => table
                            .Column<int>("Id", column => column.PrimaryKey().Identity())
                            .Column<int>("Parameter_id")
                            .Column<int>("ParameterCategory_id")
                            .Column<int>("ParameterRecord_Id")
            );
            return 1;
        }
    }
}