using CloudBust.Application.Models;
using Orchard.ContentManagement;
using Orchard.Security;
using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;

namespace CloudBust.Application.OData.Profile
{
    [DataContract]
    public class Profile
    {
        [DataMember]
        public int? Id { get; private set; }

        [DataMember]
        public string Type { get; private set; }

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
        public Uri link { get; private set; }
        [DataMember]
        public bool? RegisterFlag { get; private set; }

        public Profile(IUser User, HttpRequestMessage m, string hash, bool newUser, bool publicData = false, bool allowPublic = true)
        {
            Type = "Profile";

            // part fields

            Username = User.UserName;

            // public data
            if (publicData && !allowPublic) return;
            var profile = User.As<UserProfilePart>();
            if (profile != null)
            {
                if(profile.ShowEmail)
                    Email = User.Email;
                FirstName = profile.FirstName;
                LastName = profile.LastName;
                Location = profile.Location;
                WebSite = profile.WebSite;
                Bio = profile.Bio;
            }

            // private data
            if (publicData) return;

            Id = User.Id;
            RegisterFlag = newUser;
            // computed fields
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + appPath;
            if (baseUrl != null) baseUrl = baseUrl.TrimEnd('/','\\');

            UriBuilder b = new UriBuilder(baseUrl + "/v1/user/profile('" + Username + "')");
            link = b.Uri;


            Hash = hash;
        }
    }
}