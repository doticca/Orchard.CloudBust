using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudBust.Application.Models;
using Orchard.Security.Permissions;

namespace CloudBust.Dashboard.ViewModels
{
    public class ApplicationEditAppStoreViewModel
    {
        public int ServerBuild { get; set; }
        public int MinimumClientBuild { get; set; }
        public string UpdateUrl { get; set; }
        public string UpdateUrlOSX { get; set; }
        public string UpdateUrlTvOS { get; set; }
        public string UpdateUrlWatch { get; set; }
        public string UpdateUrlDeveloper { get; set; }
    }
}
