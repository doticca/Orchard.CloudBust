using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;
using System.Collections.Generic;

namespace CloudBust.Application.Models
{
    public class RowRecord
    {
        public RowRecord()
        {
            Columns = new List<RowColumnsRecord>();
        }
        public virtual int Id { get; set; }
        public virtual int Position { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<RowColumnsRecord> Columns { get; set; }
    }
}