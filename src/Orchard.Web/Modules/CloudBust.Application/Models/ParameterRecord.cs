using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;
using System.Collections.Generic;

namespace CloudBust.Application.Models
{
    public class ParameterRecord
    {
        public ParameterRecord()
        {
            Categories = new List<ParameterParameterCategoriesRecord>();
        }
        public virtual int Id { get; set; }
        public virtual ApplicationRecord ApplicationRecord { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string NormalizedName { get; set; }
        public virtual string ParameterType { get; set; }
        [StringLengthMax]
        public virtual string ParameterValueString { get; set; }
        public virtual int? ParameterValueInt { get; set; }
        public virtual double? ParameterValueDouble { get; set; }
        public virtual bool? ParameterValueBool { get; set; }
        public virtual DateTime? ParameterValueDateTime { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<ParameterParameterCategoriesRecord> Categories { get; set; }
    }
}