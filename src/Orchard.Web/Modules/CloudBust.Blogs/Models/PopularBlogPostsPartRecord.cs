using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs.Models
{
    [OrchardFeature("CloudBust.Blogs.Stats")]
    public class PopularBlogPostsPartRecord : ContentPartRecord {
        public PopularBlogPostsPartRecord()
        {
        }

        public virtual int BlogId { get; set; }
    }
}
