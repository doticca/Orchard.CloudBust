using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.Models
{
    public class SessionRecord 
    {
        public virtual int Id { get; set; }
        public virtual string ApplicationName { get; set; }
        public virtual string GameName { get; set; }
        public virtual string UserName { get; set; }
        public virtual string DeviceName { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
    }
}