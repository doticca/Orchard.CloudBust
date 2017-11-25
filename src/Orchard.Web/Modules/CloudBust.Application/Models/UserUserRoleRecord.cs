using CloudBust.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace  CloudBust.Application.Models
{
    public class UserUserRoleRecord
    {
        public virtual int Id { get; set; }
        public virtual UserProfilePartRecord UserProfilePartRecord { get; set; }
        public virtual UserRoleRecord UserRoleRecord { get; set; }
    }
}