using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs.Models
{    
    public class PostTypePart : ContentPart
    {
        public int PostType { get; set; }
    }
}