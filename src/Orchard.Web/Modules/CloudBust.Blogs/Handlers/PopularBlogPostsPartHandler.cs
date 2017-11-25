using CloudBust.Blogs.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using System.Web.Routing;

namespace CloudBust.Blogs.Handlers {
    [OrchardFeature("CloudBust.Blogs.Stats")]
    public class PopularBlogPostsPartHandler : ContentHandler {
        public PopularBlogPostsPartHandler(IRepository<PopularBlogPostsPartRecord> repository, RequestContext requestContext) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}