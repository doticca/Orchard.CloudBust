using CloudBust.Application.Models;
using System.Collections.Generic;

namespace CloudBust.Dashboard.ViewModels
{
    public class UserInvitesViewModel
    {
        public UserProfilePart UserProfile { get; }
        public ApplicationRecord Application { get; }
        public IEnumerable<InvitationPendingRecord> Invitations { get; set; }

        public UserInvitesViewModel(ApplicationRecord app, UserProfilePart profile)
        {
            this.Application = app;
            this.UserProfile = profile;
        }
    }
}