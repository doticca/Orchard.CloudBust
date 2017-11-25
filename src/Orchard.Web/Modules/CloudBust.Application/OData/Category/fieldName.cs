using CloudBust.Application.Models;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;

namespace CloudBust.Application.OData.Category
{
    [DataContract]
    public class fieldName
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public string Type { get; private set; }
        [DataMember]
        public string Name { get; private set; }

        public const string OType = "Field";

        public fieldName()
        { }

        public fieldName(ApplicationCategoryRecord p, HttpRequestMessage m)
        {            
            this.Id = p.Id;
            this.Name = p.Name;
            this.Type = OType;
        }

        public void UpdateName(ApplicationCategoryRecord p)
        {
            p.Name = this.Name;
        }
    }
}