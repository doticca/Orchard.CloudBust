using Orchard.Security;
using CloudBust.Application.Services;
using System;

namespace CloudBust.Dashboard.ViewModels
{
    public class AppSettingsCategoriesCreateViewModel  {
        public IUser User { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    
}
