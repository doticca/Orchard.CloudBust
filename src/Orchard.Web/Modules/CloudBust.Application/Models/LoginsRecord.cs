using CloudBust.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace  CloudBust.Application.Models
{
    public class LoginsRecord
    {
        public virtual int Id { get; set; }
        public virtual string Hash { get; set; }
        public virtual UserProfilePartRecord UserProfilePartRecord { get; set; }
        public virtual ApplicationRecord ApplicationRecord { get; set; }
        public virtual DateTime? UpdatedUtc { get; set; }
    }
}