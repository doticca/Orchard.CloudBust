using CloudBust.Application.Models;
using Orchard;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Application.Services
{

    public interface IGamesService : IDependency
    {
        void StoreScore(GameScoreRecord gameScoreRecord);
        IEnumerable<GameScoreRecord> GetScoreForUserInApplication(string applicationName, string gameName, string userName);

        // application games (many to many)
        ApplicationGameRecord GetGame(int Id);
        IEnumerable<ApplicationGameRecord> GetGames();
        IEnumerable<ApplicationGameRecord> GetApplicationGames(ApplicationRecord applicationRecord);
        IEnumerable<ApplicationGameRecord> GetApplicationGames(int applicationId);
        ApplicationGameRecord GetGameByName(string gameName);
        ApplicationGameRecord GetGameByKey(string key);
        ApplicationGameRecord CreateGame(string gameName, string gameDescription, string owner);
        bool UpdateGame(int id, string gameName, string gameDescription);
        bool UpdateGameLocation(int id, double latitude, double longitude);
        IEnumerable<ApplicationGameRecord> GetUserGames(IUser user);
        bool CreateGameForApplication(string applicationName, int gameId);
        bool CreateKeysForGame(int id);
        IEnumerable<ApplicationGameRecord> GetNonApplicationGames(IUser user, ApplicationRecord applicationRecord);
        bool RemoveGameFromApplication(string applicationName, int gameId);

        // game events
        IEnumerable<GameEventRecord> GetGameEvents(ApplicationGameRecord applicationGameRecord);
        GameEventRecord CreateGameEvent(int applicationgameId, string eventName, string eventDescription, GameEventType eventType);
        GameEventRecord CreateGameEvent(ApplicationGameRecord applicationGameRecord, string eventName, string eventDescription, GameEventType eventType);
        GameEventRecord GetGameEvent(int Id);
        GameEventRecord GetGameEventByName(ApplicationGameRecord applicationGameRecord, string eventName);
        int GetGameLastBusinessProcess(ApplicationGameRecord applicationGameRecord);
        void GameEventBusinessProcessUp(int Id);
        void GameEventBusinessProcessDown(int Id);
        bool UpdateGameEvent(int Id, string eventName, string eventDescription, GameEventType eventType);
      
        bool DeleteGameEvent(int Id);
    }
}