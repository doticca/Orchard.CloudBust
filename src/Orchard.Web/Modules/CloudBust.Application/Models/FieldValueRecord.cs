using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Application.Models
{
    public abstract class FieldValueRecord
    {
        public virtual int Id { get; set; }
    }

    public class StringFieldValueRecord : FieldValueRecord
    {
        public virtual string Value { get; set; }
    }

    public class IntegerFieldValueRecord : FieldValueRecord
    {
        public virtual long? Value { get; set; }
    }

    public class DoubleFieldValueRecord : FieldValueRecord
    {
        public virtual double? Value { get; set; }
    }

    public class BoolFieldValueRecord : FieldValueRecord
    {
        public virtual bool? Value { get; set; }
    }
    public class DateTimeFieldValueRecord : FieldValueRecord
    {
        public virtual bool? Value { get; set; }
    }
}