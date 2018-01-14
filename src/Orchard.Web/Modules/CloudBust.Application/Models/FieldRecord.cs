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
            StringFieldValueRecord = new List<StringFieldValueRecord>();
            DoubleFieldValueRecord = new List<DoubleFieldValueRecord>();
            BoolFieldValueRecord = new List<BoolFieldValueRecord>();
            IntegerFieldValueRecord = new List<IntegerFieldValueRecord>();
            DateTimeFieldValueRecord = new List<DateTimeFieldValueRecord>();
        }
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string NormalizedName { get; set; }
        public virtual string FieldType { get; set; }
        public virtual int Position { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<StringFieldValueRecord> StringFieldValueRecord { get; set; }
        [CascadeAllDeleteOrphan]
        public virtual IList<DoubleFieldValueRecord> DoubleFieldValueRecord { get; set; }
        [CascadeAllDeleteOrphan]
        public virtual IList<BoolFieldValueRecord> BoolFieldValueRecord { get; set; }
        [CascadeAllDeleteOrphan]
        public virtual IList<IntegerFieldValueRecord> IntegerFieldValueRecord { get; set; }
        [CascadeAllDeleteOrphan]
        public virtual IList<DateTimeFieldValueRecord> DateTimeFieldValueRecord { get; set; }
    }
}