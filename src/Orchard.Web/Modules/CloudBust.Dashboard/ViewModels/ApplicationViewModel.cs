using CloudBust.Application.Models;
using Orchard.Security;
using Orchard.Users.Models;
using System.Collections.Generic;

namespace CloudBust.Dashboard.ViewModels
{
    public class ApplicationViewModel
    {
        public IUser User { get; set; }
        public string HostUrl { get; set; }
        public int Page { get; set; }
        public bool IsWebApp { get; set; }
        public ApplicationRecord Application { get; set; }
        public bool AfterPost { get; set; }
        public dynamic Pager { get; set; }

        public UserRoleRecord DefaultRole { get; set; }
        public IEnumerable<UserRoleRecord> Roles { get; set; }

        public IEnumerable<ApplicationGameRecord> Games { get; set; }

        public IEnumerable<ApplicationDataTableRecord> DataTables { get; set; }
        
        public IList<UserProfileEntry> Users { get; set; }

        public IList<ParameterRecord> Settings { get; set; }
        public IList<ParameterCategoryRecord> SettingsCategories { get; set; }
    }

    public class UserProfileEntry
    {
        public UserPartRecord User { get; set; }
        public UserProfilePartRecord Profile { get; set; }
        public bool IsChecked { get; set; }
    }
}