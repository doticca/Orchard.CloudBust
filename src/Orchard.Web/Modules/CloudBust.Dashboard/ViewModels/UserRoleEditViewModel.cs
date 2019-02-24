using Orchard.Security;

namespace CloudBust.Dashboard.ViewModels
{
    public class UserRoleEditViewModel  {
        public IUser User { get; set; }
        public string ApplicationName { get; set; }
        public string AppKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDashboard { get; set; }
        public bool IsSettings { get; set; }
        public bool IsSecurity { get; set; }
        public bool IsData { get; set; }
        public string HostUrl { get; set; }
    }
}
