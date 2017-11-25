using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudBust.Application.Models;
using Orchard.Security.Permissions;

namespace CloudBust.Dashboard.ViewModels
{
    public class ApplicationEditSmtpViewModel  {
        public bool internalEmail { get; set; }
        public string senderEmail { get; set; }
        public string mailServer { get; set; }
        public int mailPort { get; set; }
        public string mailUsername { get; set; }
        public string mailPassword { get; set; }
    }
}