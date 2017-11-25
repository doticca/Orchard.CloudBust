using CloudBust.Application.Models;
using CloudBust.Application.Services;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using System;

namespace CloudBust.Application
{
    public class SessionMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(SessionRecord).Name,
                            table => table
                                .Column<int>("Id", column => column.PrimaryKey().Identity())
                                .Column<string>("ApplicationName")
                                .Column<string>("GameName")
                                .Column<string>("UserName")
                                .Column<string>("DeviceName")
                                .Column<DateTime>("StartDate")
                                .Column<DateTime>("EndDate")
            );

            SchemaBuilder.CreateTable(typeof(SessionScoreRecord).Name,
            table => table
                                .Column<int>("Id", column => column.PrimaryKey().Identity())
                                .Column<int>("SessionRecord_id")
                                .Column<double>("ShortTermMemory")
                                .Column<double>("InvoluntaryAttention")
                                .Column<double>("MotorLearningStability")
                                .Column<double>("MotorLearningAgility")
                                .Column<double>("MotorLearningSmoothness")
                                .Column<double>("MotorReflexScore")
                                .Column<double>("MotorLearning")
            );
            SchemaBuilder.CreateTable(typeof(SessionEventCoreRecord).Name,
            table => table
                                .Column<int>("Id", column => column.PrimaryKey().Identity())
                                .Column<int>("SessionRecord_id")
                                .Column<int>("GameEventRecord_id")
                                .Column<int>("Stimulae")
                                .Column<int>("Object")
                                .Column<DateTime>("Timestamp")
            );
            return 1;
        }
    }


}