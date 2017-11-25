using Orchard.Blogs.Models;
using Orchard.ContentManagement;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;
using Orchard.Utility.Extensions;
using System.Web.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Blogs.Extensions;
using Orchard.Mvc.Html;

namespace CloudBust.Blogs.OData
{
    [DataContract]
    public class BlogPost
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public string Type { get; private set; }
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public string Preview { get; private set; }
        [DataMember]
        public Uri link { get; private set; }
        [DataMember]
        public Uri url { get; private set; }

        public BlogPost(BlogPostPart blogPost, HttpRequestMessage m, UrlHelper urlHelper)
        {
            Type = "BlogPost";

            //// part fields
            Id = blogPost.Id;
            Name = blogPost.Title;

            ContentItem articleitem = blogPost.ContentItem;
            dynamic cc = articleitem.Parts.FirstOrDefault(a => a.TypePartDefinition.PartDefinition.Name == "PreviewPart");

            Preview = cc.PreviewText;
            
            Preview = Preview.RemoveTags().Ellipsize(150, " ...", true);

            //// computed fields
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + appPath;
            UriBuilder b = new UriBuilder(baseUrl + "/v1/blogs('" + blogPost.BlogPart.Name + "')/posts('" + Id + "')");

            link = b.Uri;

            string aurl = urlHelper.ItemDisplayUrl(articleitem);
            b.Path = aurl;
            url = b.Uri;

        }
    }
}