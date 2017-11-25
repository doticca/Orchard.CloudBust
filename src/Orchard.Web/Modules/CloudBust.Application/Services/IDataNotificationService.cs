using CloudBust.Application.Models;
using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Application.Services
{
    public interface IDataNotificationService : IDependency
    {
        void ApplicationUpdated(ApplicationRecord app);
        void GameUpdated(string gameName, ApplicationGameRecord gameRecord = null);
        void ScoreUpdated(string gameName);
        void CategoryUpdated();
        void ServerUpdated();
    }
}