using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudBust.Application.Models;
using Orchard.Security.Permissions;
using Orchard.Security;

namespace CloudBust.Dashboard.ViewModels
{
    public class UserRoleEditViewModel  {
        public IUser User { get; set; }
        public string ApplicationName { get; set; }
        public string AppKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool isDefault { get; set; }
        public bool isDashboard { get; set; }
        public bool isSettings { get; set; }
        public bool isSecurity { get; set; }
        public bool isData { get; set; }
    }
}
