using CloudBust.Application.Models;
using System;

namespace  CloudBust.Application.Models
{
    public class UserApplicationRecord
    {
        public virtual int Id { get; set; }
        public virtual DateTime? RegistrationStart { get; set; }
        public virtual DateTime? RegistrationEnd { get; set; }
        public virtual UserProfilePartRecord UserProfilePartRecord { get; set; }
        public virtual ApplicationRecord ApplicationRecord { get; set; }
    }
}