using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.Models
{
    public class ParameterCategoryRecord 
    {
        public virtual int Id { get; set; }
        public virtual ApplicationRecord ApplicationRecord { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string NormalizedName { get; set; }
    }
}