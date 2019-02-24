using CloudBust.Application.Models;
using Orchard;
using Orchard.MediaLibrary.Models;
using Orchard.Security;
using Orchard.Users.Models;
using System;
using System.Collections.Generic;

namespace CloudBust.Application.Services
{
    public interface IProfileService : IDependency
    {
        //ContentItem Get(int id);
        UserPart GetParent(UserProfilePartRecord record);

        UserPart GetParent(UserProfilePart part);

        UserProfilePart Get(IUser owner);

        UserProfilePart Get(string username);

        UserProfilePart Get(UserProfilePartRecord record);

        UserProfilePart Get(int id);

        IEnumerable<UserProfilePartRecord> GetUsersForApplication(ApplicationRecord appRecord);

        IUser GetUserForOrphanedVendorId(string vendorId);

        IUser GetGameCenterUserForVendorId(string vendorId);

        bool IsUserInApplication(UserProfilePart profilePart, ApplicationRecord appRecord);

        bool IsUserInApplication(IUser user, ApplicationRecord appRecord);

        IEnumerable<int> GetUserIDsForApplication(ApplicationRecord appRecord);

        bool CreateUserForApplicationRecord(UserProfilePart profilePart, ApplicationRecord appRecord);

        IEnumerable<UserRoleRecord> GetUserRoles(UserProfilePart profilePart, ApplicationRecord appRecord);

        string CreateNonce(IUser user, TimeSpan delay, string appkey);

        bool DecryptNonce(string nonce, out string username, out DateTime validateByUtc, out string appkey, out string error);

        bool VerifyUserUnicity(string userName, string email);

        bool SendChallengeMail(ApplicationRecord app, IUser user, Func<string, string> createUrl);

        bool SendLostPasswordEmail(ApplicationRecord app, string usernameOrEmail, Func<string, string> createUrl);

        IUser ValidateChallenge(string nonce, out string appKey, out string description);

        IUser ValidateLostPassword(string nonce, out string appKey);

        void InviteWithEmail(ApplicationRecord applicationRecord, UserProfilePart user, string email, string message);

        InvitationPendingRecord GetPendingInvitation(int id);

        void PendingInvitationProcessed(int id);

        bool IsPendingInvitationProcessed(int id);

        bool DecryptInvitationNonce(string nonce, out int userid, out int appid, out string email, out DateTime validateByUtc);

        InvitationPendingRecord ValidateInvitationChallenge(string nonce, out UserProfilePart invitee, out ApplicationRecord application);

        IEnumerable<InvitationPendingRecord> GetPendingInvitations(UserProfilePart profilePart, ApplicationRecord applicationRecord);

        IEnumerable<InvitationPendingRecord> GetPendingInvitationsSent(UserProfilePart profilePart, ApplicationRecord applicationRecord);

        bool AcceptInvitation(int id, IUser user, out UserProfilePart inviter, out UserProfilePart friend, out string appkey);

        bool IsFriendOfUser(UserProfilePart profilePart, ApplicationRecord appRecord, string username);

        IEnumerable<UserProfilePart> GetInvitersOfUser(UserProfilePart userProfilePart, ApplicationRecord appRecord);

        IEnumerable<UserProfilePart> GetInvitersOfUser(string username, ApplicationRecord appRecord);

        string GetMediaFolder();

        int GetMediaFilesCount();

        string GetMediaFolder(UserProfilePart profilePart, ApplicationRecord appRecord);

        int GetMediaFilesCount(UserProfilePart profilePart, ApplicationRecord appRecord);

        IEnumerable<MediaPart> GetMediaFiles();

        IEnumerable<MediaPart> GetMediaFiles(UserProfilePart profilePart, ApplicationRecord appRecord);

        UserProfilePart ResetPassword(UserProfilePart profilePart);

        void RemoveAllUserRoles(UserProfilePartRecord profileRecord, ApplicationRecord applicationRecord);

        bool SetUserRole(UserProfilePart profilePart, ApplicationRecord applicationRecord, string roleName, bool onlyThis = false);
    }
}