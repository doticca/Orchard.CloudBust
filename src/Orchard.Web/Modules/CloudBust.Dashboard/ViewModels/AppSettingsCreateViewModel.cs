using Orchard.Security;
using CloudBust.Application.Services;
using System;

namespace CloudBust.Dashboard.ViewModels
{
    public class AppSettingsCreateViewModel  {
        public IUser User { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParameterType { get; set; }
        public CBType ParameterTypeI { get; set; }
        public string ParameterValueString { get; set; }
        public int ParameterValueInt { get; set; }
        public double ParameterValueDouble { get; set; }
        public bool ParameterValueBool { get; set; }
        public DateTime ParameterValueDateTime { get; set; }
    }

    
}
