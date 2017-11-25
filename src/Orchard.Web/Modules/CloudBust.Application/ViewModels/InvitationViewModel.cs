using CloudBust.Application.Models;
using Orchard.Environment.Extensions;
using Orchard.Users.Models;
using System.Collections.Generic;

namespace CloudBust.Application.ViewModels
{
    public class InvitationViewModel
    {
        public InvitationPendingRecord invitation;
        public UserPart inviter;
        public UserProfilePart user;
    }
}