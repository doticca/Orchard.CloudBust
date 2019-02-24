using System;
using System.Runtime.Serialization;
using CloudBust.Application.Models;

namespace CloudBust.Application.OData.Profile
{
    [DataContract]
    public class Invitation
    {
        public Invitation(InvitationPendingRecord r)
        {
            Type = "PendingInviation";
            RejectedFlag = false;
            CreatedUtc = r.CreatedUtc;
            Message = r.Message;
            InvitationEmail = r.invitationEmail;
            ApplicationId = r.ApplicationRecord.Id;
            ProfileId = r.UserProfilePartRecord.Id;
        }

        public Invitation(InvitationRejectedRecord r)
        {
            Type = "RejectedInviation";
            RejectedFlag = true;
            CreatedUtc = r.CreatedUtc;
            ModifiedUtc = r.ModifiedUtc;
            Message = string.Empty;
            InvitationEmail = r.invitationEmail;
            ApplicationId = r.ApplicationRecord.Id;
            ProfileId = r.UserProfilePartRecord.Id;
        }

        [DataMember]
        public string Type { get; private set; }

        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public string Hash { get; private set; }

        [DataMember]
        public int ProfileId { get; private set; }

        [DataMember]
        public int ApplicationId { get; private set; }

        [DataMember]
        public string InvitationEmail { get; private set; }

        [DataMember]
        public string Message { get; private set; }

        [DataMember]
        public DateTime? CreatedUtc { get; private set; }

        [DataMember]
        public DateTime? ModifiedUtc { get; private set; }

        [DataMember]
        public bool RejectedFlag { get; private set; }
    }
}