using CloudBust.Application.Models;
using CloudBust.Application.Services;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using System;

namespace CloudBust.Application
{
    public class SenseApiMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(ApplicationGameRecord).Name,
                            table => table
                                            .Column<int>("Id", column => column.PrimaryKey().Identity())
                                            .Column<string>("Name")
                                            .Column<string>("NormalizedGameName")
                                            .Column<string>("Description")
                                            .Column<string>("owner", column => column.WithDefault("admin"))
                                            .Column<DateTime>("CreatedUtc")
                                            .Column<DateTime>("ModifiedUtc")
                                            .Column<string>("AppKey")
                                            .Column<string>("AppSecret")
            );

            SchemaBuilder.CreateTable(typeof(ApplicationApplicationGamesRecord).Name,
                        table => table
                                        .Column<int>("Id", column => column.PrimaryKey().Identity())
                                        .Column<int>("Application_id")
                                        .Column<int>("ApplicationGame_id")
                                        .Column<int>("ApplicationRecord_Id")
            );
            SchemaBuilder.CreateTable(typeof(GameScoreRecord).Name,
                table => table
                                .Column<int>("Id", column => column.PrimaryKey().Identity())
                                .Column<string>("ApplicationName")
                                .Column<string>("GameName")
                                .Column<string>("UserName")
                                .Column<DateTime>("StartDate")
                                .Column<DateTime>("EndDate")
                                .Column<double>("Agility")
                                .Column<int>("AgilityScore")
                                .Column<string>("AgilityText")
                                .Column<double>("Stability")
                                .Column<int>("StabilityScore")
                                .Column<string>("StabilityText")
                                .Column<double>("Smoothness")
                                .Column<int>("SmoothnessScore")
                                .Column<string>("SmoothnessText")
                                .Column<double>("Accuracy")
                                .Column<int>("AccuracyScore")
                                .Column<string>("AccuracyText")
                                .Column<int>("Attention")
                                .Column<int>("Spatial")
                                .Column<int>("Executive")
                                .Column<int>("Score")
            );
            SchemaBuilder.CreateTable(typeof(GameEventRecord).Name,
                table => table
                                .Column<int>("Id", column => column.PrimaryKey().Identity())
                                .Column<int>("ApplicationGameRecord_id")
                                .Column<string>("Name")
                                .Column<string>("NormalizedEventName")
                                .Column<string>("Description")
                                .Column<int>("BusinessProcess")
                                .Column<int>("GameEventType")
            );
            return 1;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable(typeof(ApplicationGameRecord).Name, table => table
                .AddColumn<double>("longitude"));
            SchemaBuilder.AlterTable(typeof(ApplicationGameRecord).Name, table => table
                .AddColumn<double>("latitude"));
            return 2;
        }
    }
}