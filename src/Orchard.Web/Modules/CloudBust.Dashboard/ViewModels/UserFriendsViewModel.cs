using CloudBust.Application.Models;
using System.Collections.Generic;

namespace CloudBust.Dashboard.ViewModels
{
    public class UserFriendsViewModel
    {
        public UserProfilePart UserProfile { get; }
        public ApplicationRecord Application { get; }
        public IEnumerable<FriendRecord> Friends { get; set; }

        public UserFriendsViewModel(ApplicationRecord app, UserProfilePart profile)
        {
            this.Application = app;
            this.UserProfile = profile;
        }
    }
}