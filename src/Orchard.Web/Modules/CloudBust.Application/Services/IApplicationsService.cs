using CloudBust.Application.Models;
using Orchard;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Application.Services
{

    public interface IApplicationsService : IDependency
    {
        //void StoreScore(GameScoreRecord gameScoreRecord);
        //IEnumerable<GameScoreRecord> GetScoreForUserInApplication(string applicationName, string gameName, string userName);

        // application categories (many to many)
        ApplicationCategoryRecord GetCategory(int Id);
        IEnumerable<ApplicationCategoryRecord> GetCategories();
        ApplicationCategoryRecord GetCategoryByName(string categoryName);
        ApplicationCategoryRecord CreateCategory(string categoryName, string categoryDescription);
        bool DeleteCategory(int Id);
        bool UpdateCategory(int id, string categoryName, string categoryDescription);

        //// application games (many to many)
        //ApplicationGameRecord GetGame(int Id);
        //IEnumerable<ApplicationGameRecord> GetGames();
        //IEnumerable<ApplicationGameRecord> GetApplicationGames(ApplicationRecord applicationRecord);
        //IEnumerable<ApplicationGameRecord> GetApplicationGames(int applicationId);
        //ApplicationGameRecord GetGameByName(string gameName);
        //ApplicationGameRecord GetGameByKey(string key);
        //ApplicationGameRecord CreateGame(string gameName, string gameDescription, string owner);
        //bool DeleteGame(int Id);
        //bool UpdateGame(int id, string gameName, string gameDescription);
        //bool UpdateGameLocation(int id, double latitude, double longitude);
        //IEnumerable<ApplicationGameRecord> GetUserGames(IUser user);
        //bool CreateGameForApplication(string applicationName, int gameId);
        //bool CreateKeysForGame(int id);
        //IEnumerable<ApplicationGameRecord> GetNonApplicationGames(IUser user, ApplicationRecord applicationRecord);
        //bool RemoveGameFromApplication(string applicationName, int gameId);

        //// game events
        //IEnumerable<GameEventRecord> GetGameEvents(ApplicationGameRecord applicationGameRecord);
        //GameEventRecord CreateGameEvent(int applicationgameId, string eventName, string eventDescription, GameEventType eventType);
        //GameEventRecord CreateGameEvent(ApplicationGameRecord applicationGameRecord, string eventName, string eventDescription, GameEventType eventType);
        //GameEventRecord GetGameEvent(int Id);
        //GameEventRecord GetGameEventByName(ApplicationGameRecord applicationGameRecord, string eventName);
        //int GetGameLastBusinessProcess(ApplicationGameRecord applicationGameRecord);
        //void GameEventBusinessProcessUp(int Id);
        //void GameEventBusinessProcessDown(int Id);
        //bool UpdateGameEvent(int Id, string eventName, string eventDescription, GameEventType eventType);
        //bool DeleteGameEvent(int Id);

        // user roles (one to many)
        UserRoleRecord GetUserRole(int Id);
        IEnumerable<UserRoleRecord> GetUserRoles(ApplicationRecord applicationRecord);
        IEnumerable<UserRoleRecord> GetUserRoles(string applicationName);
        IEnumerable<UserRoleRecord> GetUserRoles(int applicationId);
        UserRoleRecord GetUserRoleByName(string applicationName, string userroleName); //NOT UNIQUE
        UserRoleRecord GetUserRoleByName(int applicationId, string userroleName);
        UserRoleRecord GetUserRoleByName(ApplicationRecord applicationRecord, string userroleName);
        UserRoleRecord CreateUserRole(string applicationName, string userroleName, string userroleDescription); // NOT UNIQUE 
        UserRoleRecord CreateUserRole(int applicationId, string userroleName, string userroleDescription);
        UserRoleRecord CreateUserRole(ApplicationRecord applicationRecord, string userroleName, string userroleDescription);
        void SetDefaultRole(ApplicationRecord applicationRecord, string userroleName);
        void SetDefaultRole(ApplicationRecord applicationRecord, int Id);
        UserRoleRecord GetDefaultRole(ApplicationRecord applicationRecord);
        bool DeleteUserRole(int Id);
        bool DeleteUserRole(int applicationId, string userroleName);
        bool DeleteUserRole(string applicationName, string userroleName);
        bool DeleteUserRole(ApplicationRecord applicationRecord, string userroleName);
        bool UpdateUserRole(int Id, string userroleName, string userroleDescription);

        // applications
        ApplicationRecord GetApplication(int Id);
        ApplicationRecord GetApplication(string applicationName);

        IEnumerable<ApplicationRecord> GetApplications();
        IEnumerable<ApplicationRecord> GetUserApplications(IUser user);
        ApplicationRecord GetApplicationByName(string name);
        ApplicationRecord GetApplicationByKey(string key);
        ApplicationRecord CreateApplication(string applicationName, string applicationDescription, string owner);
        bool CreateCategoryForApplication(string applicationName, int categoryId);
        //bool CreateStartParameterForApplication(string applicationName, int parameterId);
        //bool CreateEndParameterForApplication(string applicationName, int parameterId);
        //bool DeleteApplication(int Id);
        bool UpdateApplication(int id, string applicationName, string applicationDescription, string owner, IEnumerable<ApplicationCategoryRecord> categoryRecords);
        bool UpdateApplication(int id, string applicationName, string applicationDescription);
        bool UpdateApplicationFacebook(int id, string fbAppKey, string fbAppSecret);
        bool UpdateApplicationGameCenter(int id, string bundleIdentifier, string bundleIdentifierOSX, string bundleIdentifierTvOS, string bundleIdentifierWatch);
        bool UpdateApplicationAppStore(int id, int serverBuild, int minimumClientBuild, string updateUrl, string updateUrlOSX, string updateUrlTvOS, string updateUrlWatch, string updateUrlDeveloper);
        bool UpdateApplicationSmtp(int id, bool internalEmail, string senderEmail, string mailServer, int mailPort, string mailUsername, string mailPassword);
        bool UpdateApplicationBlogs(int id, bool blogPerUser, bool blogSecurity);
        string GetApplicationProtocol(int Id);
        IEnumerable<ApplicationCategoryRecord> GetCategoriesForApplication(int id);
        //IEnumerable<ParameterRecord> GetStartParametersForApplication(int id);
        //bool DeleteStartParameterFromApplication(string applicationName, int parameterId);
        //IEnumerable<ParameterRecord> GetEndParametersForApplication(int id);
        //bool DeleteEndParameterFromApplication(string applicationName, int parameterId);
        bool CreateKeysForApplication(int id);

    }
}