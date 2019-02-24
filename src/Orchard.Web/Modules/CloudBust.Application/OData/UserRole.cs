using System;
using System.Runtime.Serialization;
using CloudBust.Application.Models;
using Orchard.Security;

namespace CloudBust.Application.OData
{
    [DataContract]
    public class UserRole
    {
        public UserRole(IUser user, UserRoleRecord userRole, string serverUrl)
        {
            Type = "Role";

            // part fields
            Id = userRole.Id;
            UserId = user.Id;

            Name = userRole.Name;
            Description = userRole.Description;
            IsDefault = userRole.IsDefaultRole;

            var username = user.UserName;

            // computed fields
            var b = new UriBuilder(serverUrl + "/v1/user/profile('" + username + "')/roles/('" + Name + "')");
            Link = b.Uri;
        }

        [DataMember]
        public string Type { get; private set; }

        [DataMember]
        public Uri Link { get; private set; }

        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public int UserId { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string Description { get; private set; }

        [DataMember]
        public bool IsDefault { get; private set; }
    }
}