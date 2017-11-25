using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;
using CloudBust.Application.Models;
using System.Net.Http;

namespace CloudBust.Application.OData
{
    [DataContract]
    public class ApplicationGame
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Type { get; private set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string AppKey { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public double Latitude { get; set; }

        private const string OType = "Game";

        public ApplicationGame(ApplicationGameRecord p, HttpRequestMessage m)
        {
            this.Id = p.Id;
            this.Type = OType;
            this.Name = p.Name;
            this.Description = p.Description;
            this.AppKey = p.AppKey;
            this.Longitude = p.Longitude;
            this.Latitude = p.Latitude;
        }
    }
}