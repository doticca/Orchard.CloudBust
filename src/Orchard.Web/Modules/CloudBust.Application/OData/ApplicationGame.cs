using System;
using System.Net.Http;
using System.Runtime.Serialization;
using CloudBust.Application.Models;

namespace CloudBust.Application.OData
{
    [DataContract]
    public class ApplicationGame
    {
        private const string OType = "WeApp";

        public ApplicationGame(ApplicationGameRecord p, string serverUrl, bool showOwner = false, bool showKey = false)
        {
            Type = OType;

            if (p == null) return;

            Id = p.Id;            
            Name = p.Name;
            Description = p.Description;
            LogoImage = p.LogoImage;
            ApplicationUrl = p.AppUrl;
            if (showOwner) {
                Owner = new ApplicationGameOwner(p, serverUrl);
            }

            if (showKey) {
                ApplicationKey = p.AppKey;
            }
        }

        [DataMember]
        public string Type { get; private set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string LogoImage { get; set; }

        [DataMember]
        public string ApplicationUrl { get; set; }

        [DataMember]
        public ApplicationGameOwner Owner { get; set; }

        [DataMember]
        public string ApplicationKey { get; set; }
    }
}