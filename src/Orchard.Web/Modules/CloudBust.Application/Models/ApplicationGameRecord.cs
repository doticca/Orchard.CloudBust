using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.Models
{
    public class ApplicationGameRecord 
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string NormalizedGameName { get; set; }
        public virtual string Owner { get; set; }
        public virtual DateTime? CreatedUtc { get; set; }
        public virtual DateTime? ModifiedUtc { get; set; }
        public virtual string AppKey { get; set; }
        public virtual string AppSecret { get; set; }
        public virtual double Longitude { get; set; }
        public virtual double Latitude { get; set; }
        public virtual string LogoImage { get; set; }
        public virtual string AppUrl { get; set; }
    }
}