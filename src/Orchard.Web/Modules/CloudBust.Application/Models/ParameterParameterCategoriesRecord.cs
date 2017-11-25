using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.Models
{
    public class ParameterParameterCategoriesRecord
    {
        public virtual int Id { get; set; }
        public virtual ParameterRecord Parameter { get; set; }
        public virtual ParameterCategoryRecord ParameterCategory { get; set; }
    }
}