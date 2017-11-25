using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using CloudBust.Application.Extensions;
using CloudBust.Application.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment.Configuration;
using Orchard.Logging;
using Orchard.Messaging.Services;
using Orchard.Security;
using Orchard.Services;
using Orchard.Caching;

namespace CloudBust.Application.Services
{    
    public class SessionsService : ISessionsService
    {
        private readonly ISignals _signals;
        private readonly IOrchardServices _orchardServices;
        private readonly IContentManager _contentManager;
        private readonly IClock _clock;
        public const string SignalName = "CloudBust.Application.ApplicationsService.Sessions";
        public const string SignalNameEvents = "CloudBust.Application.ApplicationsService.Sessions.Events";

        private readonly IRepository<SessionRecord> _sessionsRepository;
        private readonly IRepository<SessionScoreRecord> _sessionScoreRepository;
        private readonly IRepository<SessionEventCoreRecord> _sessionEventsRepository;
        private readonly IGamesService _gamesService;

        public SessionsService(
                                IContentManager contentManager
                                ,IOrchardServices orchardServices
                                ,IRepository<SessionRecord> sessionsRepository
                                ,IRepository<SessionEventCoreRecord> sessionEventsRepository
                                ,IRepository<SessionScoreRecord> sessionScoreRepository
                                , IGamesService gamesService
                                , ISignals signals
                                ,IClock clock
            )
        {
            _orchardServices = orchardServices;
            _contentManager = contentManager;
            _sessionsRepository = sessionsRepository;
            _sessionScoreRepository = sessionScoreRepository;
            _sessionEventsRepository = sessionEventsRepository;
            _gamesService = gamesService;
            _signals = signals;
            Logger = NullLogger.Instance;
            _clock = clock;
        }

        public ILogger Logger { get; set; }

        private void TriggerSignal()
        {
            _signals.Trigger(SignalName);
        }

        private void TriggerSignalEvents()
        {
            _signals.Trigger(SignalNameEvents);
        }

        public int StartSession(string applicationName, string gameName, string userName)
        {
            SessionRecord sr = new SessionRecord();
            var utcNow = _clock.UtcNow;
            sr.ApplicationName = applicationName;
            sr.GameName = gameName;
            sr.StartDate = utcNow;
            sr.UserName = userName;
            _sessionsRepository.Create(sr);
            TriggerSignal();
            return sr.Id;
        }
        public void EndSession(int Id)
        {
            var utcNow = _clock.UtcNow;
            try
            {
                var sr = _sessionsRepository.Get(Id);
                sr.EndDate = utcNow;
                TriggerSignal();
            }
            catch
            {
                return;
            }
        }
        public SessionRecord GetSession(int Id)
        {
            try
            {
                var sr = _sessionsRepository.Get(Id);
                return sr;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<SessionRecord> GetSessionsForUserInApplication(string applicationName, string gameName, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(gameName) || string.IsNullOrWhiteSpace(applicationName))
                return null;

            var sessions = from session in _sessionsRepository.Table
                           where session.UserName == userName &&
                                session.GameName == gameName &&
                                session.ApplicationName == applicationName
                           select session;

            return sessions;
        }

        public bool StartSessionEvent(int Id, string eventName, int stimulaeType, int objectType)
        {
            if (string.IsNullOrWhiteSpace(eventName) || Id <= 0 || stimulaeType<=0 || objectType<=0)
                return false;

            // check if event of same businessprocess, stimuly and object already exists
            List<SessionEventCoreRecord> sevent;
            try
            {
                sevent = (from e in _sessionEventsRepository.Table
                              where e.GameEventRecord.Name == eventName &&
                                     e.Object == objectType &&
                                     e.Stimulae == stimulaeType
                              select e).ToList();
            }
            catch { sevent = null; }

            if (sevent != null && sevent.Count()>0) return true;

            SessionRecord session = GetSession(Id);
            if (session == null) return false;
            ApplicationGameRecord game = _gamesService.GetGameByName(session.GameName);
            if (game == null) return false;
            GameEventRecord gameevent = _gamesService.GetGameEventByName(game, eventName);
            if (gameevent == null) return false;

            var utcNow = _clock.UtcNow;
            SessionEventCoreRecord eventcore = new SessionEventCoreRecord();
            eventcore.SessionRecord = session;
            eventcore.GameEventRecord = gameevent;
            eventcore.Object = objectType;
            eventcore.Stimulae = stimulaeType;
            eventcore.Timestamp = utcNow;
            _sessionEventsRepository.Create(eventcore);
            TriggerSignalEvents();

            return true;
        }

        public void StoreSessionScore(SessionScoreRecord sessionScoreRecord)
        {
            _sessionScoreRepository.Create(sessionScoreRecord);
            TriggerSignal();
        }
        public SessionScoreRecord GetScoreForSession(SessionRecord sessionRecord)
        {
            return _sessionScoreRepository.Get(x => x.Session.Id == sessionRecord.Id);
        }
        public SessionScoreRecord GetScoreForSession(int SessionId)
        {
            return _sessionScoreRepository.Get(x => x.Session.Id == SessionId);
        }
    }
}