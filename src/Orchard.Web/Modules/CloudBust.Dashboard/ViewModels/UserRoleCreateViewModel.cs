using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudBust.Application.Models;
using Orchard.Security.Permissions;
using Orchard.Security;

namespace CloudBust.Dashboard.ViewModels
{
    public class UserRoleCreateViewModel  {
        public IUser User { get; set; }
        public string ApplicationName { get; set; }
        public string AppKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool isDefault { get; set; }
        public string HostUrl { get; set; }
    }
}
