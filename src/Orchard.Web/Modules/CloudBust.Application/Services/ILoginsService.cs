using CloudBust.Application.Models;
using Orchard;
using Orchard.Security;
using System;

namespace CloudBust.Application.Services
{
    public interface ILoginsService : IDependency
    {
        void SetSessionAppId(int applicationID);
        void ClearSessionAppId();
        LoginsRecord LoginWithHash(string Hash);
        string CreateHash(UserProfilePart profilePart, ApplicationRecord applicationRecord);
        string GetHash(UserProfilePart profilePart, ApplicationRecord applicationRecord);
        void CleanupHashes(UserProfilePart profilePart, ApplicationRecord applicationRecord);
        void DeleteHash(string Hash);
        IUser ValidateHash(string Hash, string ApiKey, out string reason);
        int GetSessionAppId(IUser user);
        IUser GetUser(string Username);
        IUser GetUser(int Id);
        DateTime GetServerTime();
        IUser CheckUser();
    }
}