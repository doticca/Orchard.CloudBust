using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;

namespace CloudBust.Common.OData
{
    [DataContract]
    public class Totals 
    {
        [DataMember]
        public string Type { get; private set; }       
        [DataMember]
        public long Count { get; private set; }

        public Totals(long total, string type)
        {          
            Type = type;
            Count = total;
        }
    }
}