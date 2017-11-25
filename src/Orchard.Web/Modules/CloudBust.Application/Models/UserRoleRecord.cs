using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.Models
{
    public class UserRoleRecord 
    {
        public virtual int Id { get; set; }
        public virtual ApplicationRecord ApplicationRecord { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string NormalizedRoleName { get; set; }
        public virtual bool IsDefaultRole { get; set; }
        public virtual bool IsDashboardRole { get; set; }
        public virtual bool IsSettings { get; set; }
        public virtual bool IsSecurity { get; set; }
        public virtual bool IsData { get; set; }
    }
}