using Orchard.Users.Models;
using System;

namespace CloudBust.Application.Models
{
    public class FriendRecord
    {
        public FriendRecord()
        {
        }
        public virtual int Id { get; set; }
        public virtual UserProfilePartRecord UserProfilePartRecord { get; set; }
        public virtual ApplicationRecord ApplicationRecord { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime? CreatedUtc { get; set; }
       
    }
}