using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CloudBust.Common.OData
{
    [DataContract]
    public class oResponse
    {
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public int EventNumber { get; set; }

        public oResponse(string message, int eventNumber)
        {
            Message = message;
            EventNumber = eventNumber;
        }
        public oResponse(string message)
        {
            Message = message;
            EventNumber = 200;
        }
        public oResponse()
        {
            Message = string.Empty;
            EventNumber = 200;
        }
    }
}