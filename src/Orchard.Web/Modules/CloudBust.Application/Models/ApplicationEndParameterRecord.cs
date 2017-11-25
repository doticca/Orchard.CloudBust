using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.Models
{
    public class ApplicationEndParameterRecord
    {
        public virtual int Id { get; set; }
        public virtual ApplicationRecord Application { get; set; }
        public virtual ParameterRecord Parameter { get; set; }
    }
}