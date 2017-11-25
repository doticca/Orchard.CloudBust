using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudBust.Application.Models;
using Orchard.Security.Permissions;

namespace CloudBust.Dashboard.ViewModels
{
    public class ApplicationEditFBViewModel  {
        public string fbAppKey { get; set; }
        public string fbAppSecret { get; set; }
    }
}
