using CloudBust.Application.Models;
using Orchard.Environment.Extensions;
using System.Collections.Generic;

namespace CloudBust.Application.ViewModels
{
    [OrchardFeature("CloudBust.Application.WebApp")]
    public class ApplicationSettingsViewModel
    {
        //public bool WebIsCloudBust { get; set; }
        public string ApplicationKey { get; set; }
        public string ApplicationName { get; set; }
        public IEnumerable<ApplicationRecord> Applications { get; set; }
    }
}