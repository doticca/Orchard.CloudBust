using Orchard.Security;

namespace CloudBust.Dashboard.ViewModels
{
    public class TableCreateViewModel  {
        public IUser User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationKey { get; set; }

    }
}
