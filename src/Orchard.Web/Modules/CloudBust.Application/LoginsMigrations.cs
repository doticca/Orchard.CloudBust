using Orchard.Data.Migration;
using CloudBust.Application.Models;
using System;

namespace CloudBust.Application
{
    public class LoginsMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(LoginsRecord).Name,
                    table => table
                                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                                    .Column<string>("Hash")
                                    .Column<int>("UserProfilePartRecord_id")
                                    .Column<int>("ApplicationRecord_id")
                                    .Column<DateTime>("UpdatedUtc")
                                    )
                                    .AlterTable(typeof(LoginsRecord).Name,
                                        table => table.CreateIndex("LoginHash", new string[] { "Hash" })
            );

            return 1;
        }
    }
}