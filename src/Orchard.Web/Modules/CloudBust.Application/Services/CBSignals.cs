using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Application.Services
{
    public static class CBSignals
    {
        public const string SignalServer = "CloudBust.Application.Server";
        public const string SignalParameters = "CloudBust.Application.ParametersService";
        public const string SignalApplications = "CloudBust.Application.ApplicationsService";
        public const string SignalCategories = "CloudBust.Application.ApplicationsService.Categories";
        public const string SignalScores = "CloudBust.Application.ApplicationsService.Scores";
        public const string SignalWebApp = "CloudBust.WebApp.Changed";
    }
}