using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace CloudBust.Application.Models
{
    public class ApplicationDataTableRecord 
    {
        public ApplicationDataTableRecord()
        {
            Fields = new List<ApplicationDataTableFieldsRecord>();
            Rows = new List<ApplicationDataTableRowsRecord>();
        }
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string NormalizedTableName { get; set; }
        public virtual DateTime? CreatedUtc { get; set; }
        public virtual DateTime? ModifiedUtc { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<ApplicationDataTableFieldsRecord> Fields { get; set; }
        [CascadeAllDeleteOrphan]
        public virtual IList<ApplicationDataTableRowsRecord> Rows { get; set; }
    }
}