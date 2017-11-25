using CloudBust.Application.Models;
using CloudBust.Application.Services;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using System;

namespace CloudBust.Application
{
    public class UserRolesMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(UserRoleRecord).Name,
            table => table
                            .Column<int>("Id", column => column.PrimaryKey().Identity())
                            .Column<int>("ApplicationRecord_id")
                            .Column<string>("Name")
                            .Column<string>("NormalizedRoleName")
                            .Column<string>("Description")
                            .Column<bool>("IsDefaultRole", column => column.WithDefault(false))
                            .Column<bool>("IsDashboardRole", column => column.WithDefault(false))
                            .Column<bool>("IsSettings", column => column.WithDefault(false))
                            .Column<bool>("IsSecurity", column => column.WithDefault(false))
                            .Column<bool>("IsData", column => column.WithDefault(false))
            );
            return 1;
        }
    }
}