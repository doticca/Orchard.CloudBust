using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;
using System.Collections.Generic;

namespace CloudBust.Application.Models
{
    public class FieldRecord
    {
        public FieldRecord()
        {
            ColumnRecord = new List<ColumnRecord>();
        }
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string NormalizedName { get; set; }
        public virtual string FieldType { get; set; }
        public virtual int Position { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<ColumnRecord> ColumnRecord { get; set; }
    }
}