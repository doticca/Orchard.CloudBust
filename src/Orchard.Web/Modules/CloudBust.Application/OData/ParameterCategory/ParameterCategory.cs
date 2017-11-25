using CloudBust.Application.Models;
using CloudBust.Application.Services;
using Orchard;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;

namespace CloudBust.Application.OData.ParameterCategory
{
    [DataContract]
    public class ParameterCategory 
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public int ApplicationId { get; private set; }
        [DataMember]
        public string Type { get; private set; }
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public string Description { get; private set; }                   
        [DataMember]
        public Uri link { get; private set; }
        [DataMember]
        public Uri linkalt { get; private set; }


        public const string OType = "Parameter Category";
        public ParameterCategory(int applicationId, HttpRequestMessage m)
        {
            // create a default category filter
            this.Id = 0;
            this.ApplicationId = applicationId;
            this.Type = OType;
            this.Description = "Browse all Parameters";
            this.Name = "All Parameters";

            buildLinks();
        }

        public ParameterCategory(ParameterCategoryRecord p, HttpRequestMessage m)
        {
            this.Id = p.Id;
            this.ApplicationId = p.ApplicationRecord.Id;
            this.Type = OType;
            this.Name = p.Name;
            this.Description = p.Description;

            buildLinks();
        }
        private void buildLinks()
        {
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + appPath;
            if (baseUrl != null) baseUrl = baseUrl.TrimEnd('/', '\\');

            UriBuilder b = new UriBuilder(baseUrl + "/v1/applications(" + this.ApplicationId + ")/parameters/categories('" + this.Name + "')");
            UriBuilder b2 = new UriBuilder(baseUrl + "/v1/parameters/categories(" + this.Id + ")");
            link = b.Uri;
            linkalt = b2.Uri;
        }
    }
}