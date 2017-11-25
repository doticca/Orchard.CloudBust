using System.Collections.Generic;
using System.Linq;
using CloudBust.Application.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Security;
using Orchard.Services;


namespace CloudBust.Application.Services
{    
    public class GamesService : IGamesService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IContentManager _contentManager;
        private readonly IClock _clock;

        private readonly IRepository<ApplicationGameRecord> _gamesRepository;
        private readonly IRepository<GameEventRecord> _gameeventsRepository;
        private readonly IApplicationsService _applicationsService;
        private readonly IRepository<GameScoreRecord> _gamescoreRepository;
        private readonly IDataNotificationService _dataNotificationService;

        public GamesService(
                                IContentManager contentManager
                                ,IOrchardServices orchardServices
                                ,IRepository<ApplicationGameRecord> gamesRepository
                                ,IRepository<GameEventRecord> gameeventsRepository
                                ,IApplicationsService applicationsService
                                ,IRepository<GameScoreRecord> gamescoreRepository
                                ,IDataNotificationService datanotificationService
                                ,IClock clock
            )
        {
            _orchardServices = orchardServices;
            _contentManager = contentManager;
            _gamesRepository = gamesRepository;
            _gameeventsRepository = gameeventsRepository;
            _applicationsService = applicationsService;
            _gamescoreRepository = gamescoreRepository;
            _dataNotificationService = datanotificationService;
            _clock = clock;
        }

        #region senseapi

        public void StoreScore(GameScoreRecord gameScoreRecord)
        {
            _gamescoreRepository.Create(gameScoreRecord);
            _dataNotificationService.ScoreUpdated(gameScoreRecord.GameName);
        }

        public IEnumerable<GameScoreRecord> GetScoreForUserInApplication(string applicationName, string gameName, string userName)
        {            
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(gameName) || string.IsNullOrWhiteSpace(applicationName))
                return null;

            var scores = from score in _gamescoreRepository.Table 
                         where  score.UserName == userName && 
                                score.GameName == gameName && 
                                score.ApplicationName == applicationName
                         select score;

            return scores;
        }

        #endregion

        // APPLICATION GAMES
        #region Application Games

        public ApplicationGameRecord GetGame(int Id)
        {
            try
            {
                var game = _gamesRepository.Get(Id);
                return game;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<ApplicationGameRecord> GetGames()
        {
            try
            {
                var games = from game in _gamesRepository.Table select game;

                // token check
                foreach (ApplicationGameRecord game in games)
                {
                    if (string.IsNullOrWhiteSpace(game.AppKey))
                    {
                        CreateKeysForGame(game.Id);
                    }
                }

                return games.ToList();
            }
            catch
            {
                return new List<ApplicationGameRecord>();
            }
        }
        public IEnumerable<ApplicationGameRecord> GetApplicationGames(ApplicationRecord applicationRecord)
        {
            var games = new List<ApplicationGameRecord>();

            if (applicationRecord == null)
                return games;

            foreach (ApplicationApplicationGamesRecord game in applicationRecord.Games)
            {
                games.Add(GetGame(game.ApplicationGame.Id));
            }
            return games;
        }
        public IEnumerable<ApplicationGameRecord> GetNonApplicationGames(IUser user, ApplicationRecord applicationRecord)
        {
            if (applicationRecord == null)
                return null;

            // user games
            var games = from game in _gamesRepository.Table where game.owner == user.UserName select game;
            // app games
            //var appgames = GetApplicationGames(applicationRecord);

            var newgames = new List<ApplicationGameRecord>();
            foreach (ApplicationGameRecord game in games)
            {
                bool found = false;
                foreach (ApplicationApplicationGamesRecord gameref in applicationRecord.Games)
                {
                    if (gameref.ApplicationGame.AppKey == game.AppKey)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    newgames.Add(game);
                }
            }
            return newgames;
        }
        public IEnumerable<ApplicationGameRecord> GetApplicationGames(int applicationId)
        {
            ApplicationRecord applicationRecord =  _applicationsService.GetApplication(applicationId);
            return GetApplicationGames(applicationRecord);
        }
        public IEnumerable<ApplicationGameRecord> GetUserGames(IUser user)
        {
            try
            {
                var games = from game in _gamesRepository.Table where game.owner == user.UserName select game;
                foreach (ApplicationGameRecord game in games)
                {
                    if (string.IsNullOrWhiteSpace(game.AppKey))
                    {
                        CreateKeysForGame(game.Id);
                    }
                }
                return games.ToList();
            }
            catch
            {
                return new List<ApplicationGameRecord>();
            }
        }
        public ApplicationGameRecord GetGameByName(string gameName)
        {
            try
            {
                return _gamesRepository.Get(x => x.Name == gameName);
            }
            catch
            {
                return null;
            }
        }
        public ApplicationGameRecord GetGameByKey(string key)
        {
            return _gamesRepository.Get(x => x.AppKey == key);
        }
        public ApplicationGameRecord CreateGame(string gameName, string gameDescription, string owner)
        {
            var utcNow = _clock.UtcNow;
            ApplicationGameRecord r = new ApplicationGameRecord();
            r.Name = gameName;
            r.Description = gameDescription;
            r.NormalizedGameName = gameName.ToLowerInvariant();
            r.owner = owner;
            r.CreatedUtc = utcNow;
            r.ModifiedUtc = utcNow;

            _gamesRepository.Create(r);            

            r = GetGameByName(gameName);
            CreateKeysForGame(r.Id);

            return r;
        }
        // we should check if we gonna use this as it is
        private bool DeleteGame(int Id)
        {
            ApplicationGameRecord game = GetGame(Id);
            if (game == null) return false;
            string gameName = game.NormalizedGameName;
            _gamesRepository.Delete(game);
            _dataNotificationService.GameUpdated(gameName);
            return true;
        }
        public bool UpdateGame(int id, string gameName, string gameDescription)
        {
            ApplicationGameRecord gameRecord = GetGame(id);

            gameRecord.Name = gameName;
            gameRecord.Description = gameDescription;
            gameRecord.NormalizedGameName = gameName.ToLowerInvariant();

            _dataNotificationService.GameUpdated(gameName, gameRecord);
            return true;
        }

        public bool UpdateGameLocation(int id, double latitude, double longitude)
        {
            ApplicationGameRecord gameRecord = GetGame(id);

            gameRecord.Latitude = latitude;
            gameRecord.Longitude = longitude;

            _dataNotificationService.GameUpdated(gameRecord.NormalizedGameName, gameRecord);
            return true;
        }
        public bool CreateGameForApplication(string applicationName, int gameId)
        {
            ApplicationGameRecord gameRecord = GetGame(gameId);
            if (gameRecord == null) return false;

            ApplicationRecord moduleRecord = _applicationsService.GetApplicationByName(applicationName);
            if (moduleRecord == null) return false;

            moduleRecord.Games.Add(new ApplicationApplicationGamesRecord { Application = moduleRecord, ApplicationGame = gameRecord });

            _dataNotificationService.ApplicationUpdated(moduleRecord);
            return true;
        }
        public bool RemoveGameFromApplication(string applicationName, int gameId)
        {
            ApplicationRecord moduleRecord = _applicationsService.GetApplicationByName(applicationName);
            if (moduleRecord == null) return false;

            foreach (ApplicationApplicationGamesRecord gameRecord in moduleRecord.Games)
            {
                if (gameRecord.ApplicationGame.Id == gameId)
                {
                    moduleRecord.Games.Remove(gameRecord);
                    break;
                }
            }

            _dataNotificationService.ApplicationUpdated(moduleRecord);
            return true;
        }

        public bool CreateKeysForGame(int id)
        {
            ApplicationGameRecord gameRecord = GetGame(id);
            gameRecord.AppKey = CBDataTypes.GenerateIdentifier(16, true);
            gameRecord.AppSecret = CBDataTypes.GenerateIdentifier(32);

            _dataNotificationService.GameUpdated(gameRecord.NormalizedGameName, gameRecord);
            return true;
        }
        public GameEventRecord GetGameEvent(int Id)
        {
            try
            {
                var ev = _gameeventsRepository.Get(Id);
                return ev;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<GameEventRecord> GetGameEvents(ApplicationGameRecord applicationGameRecord)
        {
            if (applicationGameRecord == null)
                return new List<GameEventRecord>();

            try
            {
                var gameevents = from gameevent in _gameeventsRepository.Table where gameevent.ApplicationGameRecord.Id == applicationGameRecord.Id select gameevent;
                return gameevents.OrderBy(x=>x.BusinessProcess).ToList();
            }
            catch
            {
                return new List<GameEventRecord>();
            }
        }
        public GameEventRecord CreateGameEvent(int applicationgameId, string eventName, string eventDescription, GameEventType eventType)
        {
            ApplicationGameRecord applicationGameRecord = GetGame(applicationgameId);
            return CreateGameEvent(applicationGameRecord, eventName, eventDescription, eventType);

        }
        public GameEventRecord CreateGameEvent(ApplicationGameRecord applicationGameRecord, string eventName, string eventDescription, GameEventType eventType)
        {
            if (applicationGameRecord == null) return null;
            GameEventRecord r = new GameEventRecord();
            r.ApplicationGameRecord = applicationGameRecord;
            r.Name = eventName;
            r.NormalizedEventName = eventName.ToLowerInvariant();
            r.Description = eventDescription;
            r.BusinessProcess = GetGameLastBusinessProcess(applicationGameRecord)+1;
            r.GameEventType = (int)eventType;

            _gameeventsRepository.Create(r);

            _dataNotificationService.GameUpdated(applicationGameRecord.NormalizedGameName, applicationGameRecord);
            return GetGameEventByName(applicationGameRecord, eventName);
        }
        public int GetGameLastBusinessProcess(ApplicationGameRecord applicationGameRecord)
        {
            if (applicationGameRecord == null) return 0;
            try
            {
                var item = _gameeventsRepository.Table.Max<GameEventRecord>(i => i.BusinessProcess);
                return item;
            }
            catch
            {
                return 0;
            }
        }
        public GameEventRecord GetGameEventByName(ApplicationGameRecord applicationGameRecord, string eventName)
        {
            try
            {
                return _gameeventsRepository.Get(x => x.ApplicationGameRecord.Id == applicationGameRecord.Id && x.Name == eventName);
            }
            catch
            {
                return null;
            }
        }
        public void GameEventBusinessProcessUp(int Id)
        {
            GameEventRecord r = GetGameEvent(Id);
            if (r == null) return;
            try
            {
                GameEventRecord s = _gameeventsRepository.Get(x => x.BusinessProcess == r.BusinessProcess - 1);
                s.BusinessProcess = r.BusinessProcess;
                r.BusinessProcess = r.BusinessProcess - 1;
                _dataNotificationService.GameUpdated(s.ApplicationGameRecord.NormalizedGameName, s.ApplicationGameRecord);
            }
            catch
            {
                return;
            }
        }
        public void GameEventBusinessProcessDown(int Id)
        {
            GameEventRecord r = GetGameEvent(Id);
            if (r == null) return;
            try
            {
                GameEventRecord s = _gameeventsRepository.Get(x => x.BusinessProcess == r.BusinessProcess + 1);
                s.BusinessProcess = r.BusinessProcess;
                r.BusinessProcess = r.BusinessProcess + 1;
                _dataNotificationService.GameUpdated(s.ApplicationGameRecord.NormalizedGameName, s.ApplicationGameRecord);
            }
            catch
            {
                return;
            }
        }
        public bool UpdateGameEvent(int Id, string eventName, string eventDescription, GameEventType eventType)
        {
            GameEventRecord r = GetGameEvent(Id);

            r.Name = eventName;
            r.NormalizedEventName = eventName.ToLowerInvariant();
            r.Description = eventDescription;
            r.GameEventType = (int)eventType;

            _dataNotificationService.GameUpdated(r.ApplicationGameRecord.NormalizedGameName, r.ApplicationGameRecord);
            return true;
        }
        public bool DeleteGameEvent(int Id)
        {
            GameEventRecord r = GetGameEvent(Id);
            if (r == null) return false;

            int bp = r.BusinessProcess;
            int gid = r.ApplicationGameRecord.Id;
            try
            {
                _gameeventsRepository.Delete(r);
            }
            catch
            {
                return false;
            }
            var gameevents = from gameevent in _gameeventsRepository.Table where gameevent.ApplicationGameRecord.Id == gid && gameevent.BusinessProcess > bp select gameevent;
            var gevs = gameevents.OrderBy(x=>x.BusinessProcess).ToList();
            foreach (GameEventRecord ge in gevs)
            {
                ge.BusinessProcess = ge.BusinessProcess -1;
            }

            _dataNotificationService.GameUpdated(r.ApplicationGameRecord.NormalizedGameName, r.ApplicationGameRecord);
            return true;
        }

        #endregion
    }
}