using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;
using CloudBust.Application.Models;

namespace CloudBust.Application.OData.Application
{
    [DataContract]
    public class Application
    {
        public const string OType = "Application";

        public Application(ApplicationRecord p, string serverUrl)
        {
            Id = p.Id;
            Type = OType;
            Name = p.Name;
            Description = p.Description;

            serverUrl = serverUrl.TrimEnd('/');
            var b = new UriBuilder(serverUrl + "/v1/applications(" + Id + ")");
            Link = b.Uri;
        }

        [DataMember]
        public string Type { get; private set; }

        [DataMember]
        public Uri Link { get; private set; }

        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string Description { get; private set; }
    }
}