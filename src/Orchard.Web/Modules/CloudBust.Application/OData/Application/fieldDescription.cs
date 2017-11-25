using CloudBust.Application.Models;
using System.Net.Http;
using System.Runtime.Serialization;

namespace CloudBust.Application.OData.Application
{
    [DataContract]
    public class fieldDescription
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public string Type { get; private set; }
        [DataMember]
        public string Description { get; private set; }

        public const string OType = "Field";

        public fieldDescription()
        { }

        public fieldDescription(ApplicationRecord p, HttpRequestMessage m)
        {            
            this.Id = p.Id;
            this.Description = p.Description;
            this.Type = OType;
        }

        public void UpdateDescription(ApplicationRecord p)
        {
            p.Description = this.Description;
        }
    }
}