using Orchard.ContentManagement;
using Orchard.Security;
using System.Collections.Generic;

namespace CloudBust.Application.Models
{
    public class UserProfilePart : ContentPart<UserProfilePartRecord>
    {
        public string UserName => this.As<IUser>().UserName;

        public string Email => this.As<IUser>().Email;

        public string FirstName 
        {
            get => Record.FirstName;
            set => Record.FirstName = value;
        }
        public string LastName
        {
            get => Record.LastName;
            set => Record.LastName = value;
        }
        public string WebSite
        {
            get => Record.WebSite;
            set => Record.WebSite = value;
        }
        public string Location
        {
            get => Record.Location;
            set => Record.Location = value;
        }
        public string Bio
        {
            get => Record.Bio;
            set => Record.Bio = value;
        }
        public bool ShowEmail
        {
            get => Record.ShowEmail;
            set => Record.ShowEmail = value;
        }
        public bool ResetPassword
        {
            get => Record.ResetPassword;
            set => Record.ResetPassword = value;
        }
        public string FBusername
        {
            get => Record.FBusername;
            set => Record.FBusername = value;
        }

        public string FBemail
        {
            get => Record.FBemail;
            set => Record.FBemail = value;
        }
        public string FBtoken
        {
            get => Record.FBtoken;
            set => Record.FBtoken = value;
        }

        public string GCuserid
        {
            get => Record.GCuserid;
            set => Record.GCuserid = value;
        }
        public string GCdisplayname
        {
            get => Record.GCdisplayname;
            set => Record.GCdisplayname = value;
        }
        public string GCalias
        {
            get => Record.GCalias;
            set => Record.GCalias = value;
        }
        public string VendorID
        {
            get => Record.VendorID;
            set => Record.VendorID = value;
        }
        public string Platform
        {
            get => Record.Platform;
            set => Record.Platform = value;
        }
        public string Model
        {
            get => Record.Model;
            set => Record.Model = value;
        }
        public string SystemName
        {
            get => Record.SystemName;
            set => Record.SystemName = value;
        }
        public string SystemVersion
        {
            get => Record.SystemVersion;
            set => Record.SystemVersion = value;
        }
        public IList<InvitationPendingRecord> InvitationsPending => Record.InvitationsPending;

        public IList<InvitationRejectedRecord> InvitationsRejected => Record.InvitationsRejected;

        public IList<FriendRecord> Friends => Record.Friends;

        public string UniqueID
        {
            get => Record.UniqueID;
            set => Record.UniqueID = value;
        }
    }
}