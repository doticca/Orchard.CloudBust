using CloudBust.Application.Models;
using Orchard.Data.Migration;
using System;

namespace CloudBust.Application
{
    public class ApplicationMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(ApplicationRecord).Name,
                                        table => table
                                                        .Column<int>("Id", column => column.PrimaryKey().Identity())
                                                        .Column<string>("Name")
                                                        .Column<string>("NormalizedName")
                                                        .Column<string>("Description")
                                                        .Column<string>("AppKey")
                                                        .Column<string>("AppSecret")
                                                        .Column<string>("fbAppKey")
                                                        .Column<string>("fbAppSecret")
                                                        .Column<string>("owner", column => column.WithDefault("admin"))
                                                        .Column<DateTime>("CreatedUtc")
                                                        .Column<DateTime>("ModifiedUtc")
                                                        .Column<DateTime>("LastLoginUtc")
                                                        .Column<bool>("internalEmail", column => column.WithDefault(true))
                                                        .Column<string>("senderEmail")
                                                        .Column<string>("mailServer")
                                                        .Column<int>("mailPort", column => column.WithDefault(465))
                                                        .Column<string>("mailUsername")
                                                        .Column<string>("mailPassword")
                                                        .Column<string>("BundleIdentifier")
                                                        .Column<string>("BundleIdentifierOSX")
                                                        .Column<string>("BundleIdentifierTvOS")
                                                        .Column<string>("BundleIdentifierWatch")
                                                        .Column<int>("ServerBuild")
                                                        .Column<int>("MinimumClientBuild")
                                                        .Column<string>("UpdateUrl")
                                                        .Column<string>("UpdateUrlOSX")
                                                        .Column<string>("UpdateUrlTvOS")
                                                        .Column<string>("UpdateUrlWatch")
                                                        .Column<string>("UpdateUrlDeveloper")
            );

            SchemaBuilder.CreateTable(typeof(ApplicationCategoryRecord).Name,
                            table => table
                                            .Column<int>("Id", column => column.PrimaryKey().Identity())
                                            .Column<string>("Name")
                                            .Column<string>("NormalizedName")
                                            .Column<string>("Description")
            );

            SchemaBuilder.CreateTable(typeof(ApplicationApplicationCategoriesRecord).Name,
                        table => table
                                        .Column<int>("Id", column => column.PrimaryKey().Identity())
                                        .Column<int>("Application_id")
                                        .Column<int>("ApplicationCategory_id")
                                        .Column<int>("ApplicationRecord_Id")
            );
            return 1;
        }     
        
        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable(typeof(ApplicationRecord).Name, table => table
                .AddColumn<bool>("blogPerUser", column => column.WithDefault(false)));
            return 2;
        }
        public int UpdateFrom2()
        {
            SchemaBuilder.AlterTable(typeof(ApplicationRecord).Name, table => table
                .AddColumn<bool>("blogSecurity", column => column.WithDefault(false)));
            return 3;
        }
    }


}