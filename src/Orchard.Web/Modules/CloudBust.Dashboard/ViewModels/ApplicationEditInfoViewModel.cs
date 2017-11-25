using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudBust.Application.Models;
using Orchard.Security.Permissions;

namespace CloudBust.Dashboard.ViewModels
{
    public class ApplicationEditInfoViewModel  {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
