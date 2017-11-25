using CloudBust.Application.Models;
using Orchard.Environment.Extensions;
using Orchard.Users.Models;
using System.Collections.Generic;

namespace CloudBust.Application.ViewModels
{
    public class AcceptInvitationViewModel
    {
        public string ApplicationKey { get; set; }
        public UserProfilePart user; // this should be the one who created the invitation
        public UserProfilePart friend; // this should be current user during accept invitation
    }
}