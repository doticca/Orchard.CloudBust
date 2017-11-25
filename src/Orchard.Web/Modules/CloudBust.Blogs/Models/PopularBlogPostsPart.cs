using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs.Models
{
    [OrchardFeature("CloudBust.Blogs.Stats")]
    public class PopularBlogPostsPart : ContentPart<PopularBlogPostsPartRecord> {

        public int BlogId {
            get { return Record.BlogId; }
            set { Record.BlogId = value; }
        }
    }
}
