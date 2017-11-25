using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudBust.Application.Models;
using Orchard.Security.Permissions;

namespace CloudBust.Dashboard.ViewModels
{
    public class ApplicationEditBlogsViewModel  {
        public bool blogPerUser { get; set; }
        public bool blogSecurity { get; set; }
    }
}