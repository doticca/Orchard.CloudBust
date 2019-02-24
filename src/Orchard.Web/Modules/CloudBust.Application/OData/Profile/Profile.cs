using System;
using System.Runtime.Serialization;
using CloudBust.Application.Models;
using Orchard.ContentManagement;
using Orchard.Security;

namespace CloudBust.Application.OData.Profile
{
    [DataContract]
    public class Profile
    {
        public Profile(IUser user, string serverUrl, string hash, bool newUser, bool publicData = false, bool allowPublic = true)
        {
            Type = "Profile";
            Username = user.UserName;

            // public data
            if (publicData && !allowPublic) return;
            var profile = user.As<UserProfilePart>();
            if (profile != null)
            {
                if (profile.ShowEmail)
                    Email = user.Email;
                FirstName = profile.FirstName;
                LastName = profile.LastName;
                Location = profile.Location;
                WebSite = profile.WebSite;
                Bio = profile.Bio;
            }

            // private data
            if (publicData) return;

            Id = user.Id;
            RegisterFlag = newUser;

            var b = new UriBuilder(serverUrl + "/v1/user/profile('" + Username + "')");
            Link = b.Uri;


            Hash = hash;
        }

        [DataMember]
        public string Type { get; private set; }

        [DataMember]
        public Uri Link { get; private set; }

        [DataMember]
        public int? Id { get; private set; }

        [DataMember]
        public string Username { get; private set; }

        [DataMember]
        public string Email { get; private set; }

        [DataMember]
        public string FirstName { get; private set; }

        [DataMember]
        public string LastName { get; private set; }

        [DataMember]
        public string WebSite { get; private set; }

        [DataMember]
        public string Bio { get; private set; }

        [DataMember]
        public string Location { get; private set; }

        [DataMember]
        public string Hash { get; private set; }

        [DataMember]
        public bool? RegisterFlag { get; private set; }
    }
}