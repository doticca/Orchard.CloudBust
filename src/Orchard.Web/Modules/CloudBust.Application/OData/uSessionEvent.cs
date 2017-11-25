using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.OData
{
    [DataContract]
    public class uSessionEvent
    {
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public int StimulaeType { get; set; }

        [DataMember]
        public int ObjectType { get; set; }
        
    }
}