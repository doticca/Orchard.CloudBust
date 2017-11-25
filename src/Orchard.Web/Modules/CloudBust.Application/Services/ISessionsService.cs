using CloudBust.Application.Models;
using Orchard;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Application.Services
{
    public interface ISessionsService : IDependency
    {
        // sessions
        int StartSession(string applicationName, string gameName, string userName);
        void EndSession(int Id);
        SessionRecord GetSession(int Id);
        IEnumerable<SessionRecord> GetSessionsForUserInApplication(string applicationName, string gameName, string userName);

        bool StartSessionEvent(int Id, string eventName, int stimulaeType, int objectType);

        void StoreSessionScore(SessionScoreRecord sessionScoreRecord);
        SessionScoreRecord GetScoreForSession(SessionRecord sessionRecord);
        SessionScoreRecord GetScoreForSession(int SessionId);
    }
}