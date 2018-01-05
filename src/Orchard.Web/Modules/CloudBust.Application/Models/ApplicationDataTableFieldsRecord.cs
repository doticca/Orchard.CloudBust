using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.Models
{
    public class ApplicationDataTableFieldsRecord
    {
        public virtual int Id { get; set; }
        public virtual ApplicationDataTableRecord ApplicationDataTable { get; set; }
        public virtual FieldRecord Field { get; set; }
    }
}