using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;

namespace CloudBust.Common.OData
{
    [DataContract]
    public class uError 
    {
        [DataMember]
        public string Type { get; private set; }
        [DataMember]
        public string Description { get; private set; }
        [DataMember]
        public long Number { get; private set; }

        public uError(string description, long number, bool aserror=true)
        {
            if (aserror)
                Type = "Error";
            else
                Type = "Message";
            Description = description;
            Number = number;
        }
    }

}