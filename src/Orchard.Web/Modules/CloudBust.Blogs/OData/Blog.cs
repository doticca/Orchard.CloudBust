using Orchard.Blogs.Models;
using Orchard.ContentManagement;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Blogs.Extensions;

namespace CloudBust.Blogs.OData
{
    [DataContract]
    public class Blog
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
        public int PostCount { get; private set; }
        [DataMember]
        public Uri link { get; private set; }
        [DataMember]
        public string url { get; private set; }


        public Blog(BlogPart blog, HttpRequestMessage m, UrlHelper urlHelper)
        {

            Type = "Blog";

            //// part fields
            Id = blog.Id;
            PostCount = blog.PostCount;
            Name = blog.Name;
            Description = blog.Description;

            //// computed fields
            UriBuilder b = new UriBuilder(m.RequestUri.Scheme, m.RequestUri.Host, m.RequestUri.Port);
            b.Path = "/v1/blogs('" + Name +"')";
            link = b.Uri;

            url = urlHelper.AbsoluteAction(() => urlHelper.Blog(blog));
        }
    }
}