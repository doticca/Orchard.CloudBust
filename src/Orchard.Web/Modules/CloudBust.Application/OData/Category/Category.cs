using CloudBust.Application.Models;
using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;

namespace CloudBust.Application.OData.Category
{
    [DataContract]
    public class Category 
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public string Type { get; private set; }
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public string Description { get; private set; }                   
        [DataMember]
        public Uri link { get; private set; }

        private const string OType = "Category";
        public Category(HttpRequestMessage m)
        {
            // create a default category filter
            this.Id = 0;
            this.Type = OType;
            this.Description = "Browse all Applications";
            this.Name = "All Applications";

            string appPath = HttpContext.Current.Request.ApplicationPath;
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + appPath;
            UriBuilder b = new UriBuilder(baseUrl + "/v1/categories(" + Id + ")");

            link = b.Uri;
        }

        public Category(ApplicationCategoryRecord p, HttpRequestMessage m)
        {
            this.Id = p.Id;
            this.Type = OType;
            this.Name = p.Name;
            this.Description = p.Description;

            string appPath = HttpContext.Current.Request.ApplicationPath;
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + appPath;
            if (baseUrl != null) baseUrl = baseUrl.TrimEnd('/', '\\');

            UriBuilder b = new UriBuilder(baseUrl + "/v1/categories(" + Id + ")");

            link = b.Uri;
        }
    }
}