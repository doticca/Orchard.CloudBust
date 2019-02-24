using CloudBust.Application.Models;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Dashboard.ViewModels
{
    public class GamesViewModel
    {
        public ApplicationRecord Application { get; set; }
        public IUser User { get; set; }
        public string HostUrl { get; set; }
        public IList<ApplicationGameRecord> Games { get; set; }
    }
}