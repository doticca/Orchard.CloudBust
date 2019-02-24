using CloudBust.Application.Models;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace CloudBust.Dashboard.ViewModels
{
    public class ApplicationsViewModel
    {
        public IUser User { get; set; }
        public string HostUrl { get; set; }
        public IList<ApplicationRecord> Applications { get; set; }
    }
}