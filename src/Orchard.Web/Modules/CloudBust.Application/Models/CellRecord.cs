using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.Models
{
    public class CellRecord
    {
        public virtual int Id { get; set; }
        public virtual RowRecord Row { get; set; }
        public virtual FieldRecord Field { get; set; }
        public virtual int ValueId { get; set; }
    }
}