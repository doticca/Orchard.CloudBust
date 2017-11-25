using Orchard.ContentManagement;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace  CloudBust.Application.Models
{
    public class UserProfilePart : ContentPart<UserProfilePartRecord>
    {
        public string UserName
        {
            get { return this.As<IUser>().UserName; }
        }
        public string Email
        {
            get { return this.As<IUser>().Email; }
        }
        public string FirstName 
        {
            get { return Record.FirstName; }
            set { Record.FirstName = value; } 
        }
        public string LastName
        {
            get { return Record.LastName; }
            set { Record.LastName = value; }
        }
        public string WebSite
        {
            get { return Record.WebSite; }
            set { Record.WebSite = value; }
        }
        public string Location
        {
            get { return Record.Location; }
            set { Record.Location = value; }
        }
        public string Bio
        {
            get { return Record.Bio; }
            set { Record.Bio = value; }
        }
        public bool ShowEmail
        {
            get { return Record.ShowEmail; }
            set { Record.ShowEmail = value; }
        }

        public string FBusername
        {
            get { return Record.FBusername; }
            set { Record.FBusername = value; }
        }

        public string FBemail
        {
            get { return Record.FBemail; }
            set { Record.FBemail = value; }
        }
        public string FBtoken
        {
            get { return Record.FBtoken; }
            set { Record.FBtoken = value; }
        }

        public string GCuserid
        {
            get { return Record.GCuserid; }
            set { Record.GCuserid = value; }
        }
        public string GCdisplayname
        {
            get { return Record.GCdisplayname; }
            set { Record.GCdisplayname = value; }
        }
        public string GCalias
        {
            get { return Record.GCalias; }
            set { Record.GCalias = value; }
        }
        public string VendorID
        {
            get { return Record.VendorID; }
            set { Record.VendorID = value; }
        }
        public string Platform
        {
            get { return Record.Platform; }
            set { Record.Platform = value; }
        }
        public string Model
        {
            get { return Record.Model; }
            set { Record.Model = value; }
        }
        public string SystemName
        {
            get { return Record.SystemName; }
            set { Record.SystemName = value; }
        }
        public string SystemVersion
        {
            get { return Record.SystemVersion; }
            set { Record.SystemVersion = value; }
        }
        public IList<InvitationPendingRecord> InvitationsPending
        {
            get { return Record.InvitationsPending; }
        }
        public IList<InvitationRejectedRecord> InvitationsRejected
        {
            get { return Record.InvitationsRejected; }
        }
        public IList<FriendRecord> Friends
        {
            get { return Record.Friends; }
        }
        public string UniqueID
        {
            get { return Record.UniqueID; }
            set { Record.UniqueID = value; }
        }
    }
}