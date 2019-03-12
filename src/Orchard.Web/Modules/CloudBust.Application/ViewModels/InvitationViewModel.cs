using CloudBust.Application.Models;
using Orchard.Users.Models;

namespace CloudBust.Application.ViewModels
{
    public class InvitationViewModel
    {
        public InvitationPendingRecord invitation;
        public UserPart inviter;
        public UserProfilePart user;
    }
}