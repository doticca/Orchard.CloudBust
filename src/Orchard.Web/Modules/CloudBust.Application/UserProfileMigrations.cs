using System;
using CloudBust.Application.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace CloudBust.Application
{
    public class UserProfileMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(UserProfilePartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<string>("FirstName")
                    .Column<string>("LastName")
                    .Column<bool>("ShowEmail")
                    .Column<bool>("ResetPassword", column => column.WithDefault(false))
                    .Column<string>("WebSite")
                    .Column<string>("Location")
                    .Column<string>("Bio")
                    .Column<string>("FBusername")
                    .Column<string>("FBemail")
                    .Column<string>("FBtoken")
                    .Column<string>("GCuserid")
                    .Column<string>("GCdisplayname")
                    .Column<string>("GCalias")
                    .Column<string>("VendorID")
                    .Column<string>("Platform")
                    .Column<string>("Model")
                    .Column<string>("SystemName")
                    .Column<string>("SystemVersion")
            );

            ContentDefinitionManager.AlterPartDefinition(typeof(UserProfilePart).Name, part => part
                .WithDescription("Adds a Profile Part to any Content Type."));

            ContentDefinitionManager.AlterPartDefinition(typeof(UserProfilePart).Name, cfg => cfg.Attachable());

            // attach user profile part to User Content Type
            ContentDefinitionManager.AlterTypeDefinition("User",
                cfg => cfg
                    .WithPart(typeof(UserProfilePart).Name)
                    .WithSetting("TypeIndexing.Included", "True")
            );

            SchemaBuilder.CreateTable(typeof(UserApplicationRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<DateTime>("RegistrationStart")
                    .Column<DateTime>("RegistrationEnd")
                    .Column<int>("UserProfilePartRecord_id")
                    .Column<int>("ApplicationRecord_id")
            );
            SchemaBuilder.CreateTable(typeof(UserUserRoleRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("UserProfilePartRecord_id")
                    .Column<int>("UserRoleRecord_id")
            );
            return 1;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable(typeof(UserProfilePartRecord).Name,
                table => table
                    .AddColumn<string>("UniqueID")
            );
            return 2;
        }
    }
}