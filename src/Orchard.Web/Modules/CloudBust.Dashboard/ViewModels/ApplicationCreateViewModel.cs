using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudBust.Application.Models;
using Orchard.Security.Permissions;
using Orchard.Security;

namespace CloudBust.Dashboard.ViewModels
{
    public class ApplicationCreateViewModel  {
        public IUser User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string owner { get; set; }
    }
}
