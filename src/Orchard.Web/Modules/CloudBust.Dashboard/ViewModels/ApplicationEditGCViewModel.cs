using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudBust.Application.Models;
using Orchard.Security.Permissions;

namespace CloudBust.Dashboard.ViewModels
{
    public class ApplicationEditGCViewModel  {
        public string BundleIdentifier { get; set; }
        public string BundleIdentifierOSX { get; set; }
        public string BundleIdentifierTvOS { get; set; }
        public string BundleIdentifierWatch { get; set; }
    }
}
