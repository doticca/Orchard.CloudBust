using System;

namespace CloudBust.Application.Models
{
    public class InvitationRejectedRecord
    {
        public InvitationRejectedRecord()
        {
        }
        public virtual int Id { get; set; }
        public virtual UserProfilePartRecord UserProfilePartRecord { get; set; }
        public virtual ApplicationRecord ApplicationRecord { get; set; }
        public virtual string invitationEmail { get; set; }
        public virtual DateTime? CreatedUtc { get; set; }
        public virtual DateTime? ModifiedUtc { get; set; }
        
    }
}