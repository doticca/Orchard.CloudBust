using CloudBust.Application.Models;
using Orchard.Security;
using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;

namespace CloudBust.Application.OData
{
    [DataContract]
    public class UserRole
    {
        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public int UserId { get; private set; }

        [DataMember]
        public string Type { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string Description { get; private set; }

        [DataMember]
        public bool IsDefault { get; private set; }

        [DataMember]
        public Uri link { get; private set; }

        public UserRole(IUser User, UserRoleRecord UserRole, HttpRequestMessage m)
        {
            Type = "Role";

            // part fields
            Id = UserRole.Id;
            UserId = User.Id;

            Name = UserRole.Name;
            Description = UserRole.Description;
            IsDefault = UserRole.IsDefaultRole;

            string Username = User.UserName;
            // computed fields

            string appPath = HttpContext.Current.Request.ApplicationPath;
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + appPath;
            UriBuilder b = new UriBuilder(baseUrl + "/v1/user/profile('" + Username + "')/roles/('" + Name + "')");
            link = b.Uri;
        }
    }
}