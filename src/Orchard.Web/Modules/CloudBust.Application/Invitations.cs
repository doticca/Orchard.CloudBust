using Orchard.Data.Migration;
using System;
using CloudBust.Application.Models;

namespace CloudBust.Application
{
    public class Invitations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(InvitationPendingRecord).Name,
                                        table => table
                                                        .Column<int>("Id", column => column.PrimaryKey().Identity())                                                        
                                                        .Column<DateTime>("CreatedUtc")                                                        
                                                        .Column<string>("invitationEmail")
                                                        .Column<string>("Message")
                                                        .Column<int>("UserProfilePartRecord_id")
                                                        .Column<int>("ApplicationRecord_id")
            );
            SchemaBuilder.CreateTable(typeof(InvitationRejectedRecord).Name,
                            table => table
                                            .Column<int>("Id", column => column.PrimaryKey().Identity())
                                            .Column<DateTime>("CreatedUtc")
                                            .Column<DateTime>("ModifiedUtc")
                                            .Column<int>("UserProfilePartRecord_id")
                                            .Column<int>("ApplicationRecord_id")
                                            .Column<string>("invitationEmail")
            );
            return 1;
        }
        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable(typeof(InvitationPendingRecord).Name, table => table
                .AddColumn<string>("Hash"));
            return 2;
        }
        public int UpdateFrom2()
        {
            SchemaBuilder.CreateTable(typeof(FriendRecord).Name,
                                        table => table
                                                        .Column<int>("Id", column => column.PrimaryKey().Identity())
                                                        .Column<DateTime>("CreatedUtc")
                                                        .Column<int>("UserProfilePartRecord_id")
                                                        .Column<int>("ApplicationRecord_id")
            );
            return 3;
        }
        public int UpdateFrom3()
        {
            SchemaBuilder.AlterTable(typeof(FriendRecord).Name, table => table
                .AddColumn<string>("UserName"));
            return 4;
        }
    }
}