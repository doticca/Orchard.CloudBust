using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;
using System.Collections.Generic;

namespace CloudBust.Application.Models
{
    public class UserProfilePartRecord : ContentPartRecord
    {
        public UserProfilePartRecord()
        {
            Applications = new List<UserApplicationRecord>();
            Roles = new List<UserUserRoleRecord>();
            InvitationsPending = new List<InvitationPendingRecord>();
            InvitationsRejected = new List<InvitationRejectedRecord>();
            Friends = new List<FriendRecord>();
        }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual bool ShowEmail { get; set; }
        public virtual bool ResetPassword { get; set; }
        public virtual string WebSite { get; set; }
        public virtual string Location { get; set; }
        public virtual string Bio { get; set; }
        public virtual string FBusername { get; set; }
        public virtual string FBemail { get; set; }
        public virtual string FBtoken { get; set; }
        public virtual string GCuserid { get; set; }
        public virtual string GCdisplayname { get; set; }
        public virtual string GCalias { get; set; }
        public virtual string VendorID { get; set; }
        public virtual string Platform { get; set; }
        public virtual string Model { get; set; }
        public virtual string SystemName { get; set; }
        public virtual string SystemVersion { get; set; }
        public virtual string UniqueID { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<UserApplicationRecord> Applications { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<FriendRecord> Friends { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<UserUserRoleRecord> Roles { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<InvitationRejectedRecord> InvitationsRejected { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<InvitationPendingRecord> InvitationsPending { get; set; }
    }
}