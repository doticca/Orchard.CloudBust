using System;
using System.Net.Http;
using System.Runtime.Serialization;
using CloudBust.Application.Models;

namespace CloudBust.Application.OData
{
    [DataContract]
    public class ApplicationGameOwner
    {
        private const string OType = "Owner";

        public ApplicationGameOwner(ApplicationGameRecord p, string serverUrl)
        {
            Type = OType;
            Owner = p.Owner;
            Secret = p.AppSecret;
            AppKey = p.AppKey;
            Longitude = p.Longitude;
            Latitude = p.Latitude;

            serverUrl = serverUrl.TrimEnd('/');
            var b = new UriBuilder(serverUrl + "/v1/weapps('" + AppKey + "')");
            Link = b.Uri;
        }

        [DataMember]
        public string Type { get; private set; }

        [DataMember]
        public Uri Link { get; private set; }

        [DataMember]
        public string Owner { get; set; }

        [DataMember]
        public string Secret { get; set; }

        [DataMember]
        public string AppKey { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        [DataMember]
        public double Latitude { get; set; }
    }
}