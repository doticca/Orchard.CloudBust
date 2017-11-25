using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace CloudBust.Application.Models
{
    public class ApplicationParameterCategoryRecord
    {
        public virtual int Id { get; set; }
        public virtual ApplicationRecord Application { get; set; }
        public virtual ParameterCategoryRecord ParameterCategory { get; set; }
    }
}