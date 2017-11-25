using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs.Models
{    
    public class SharePart : ContentPart
    {
        public bool ShowFacebook { get; set; }
        public bool ShowTwitter { get; set; }
        public bool ShowMail { get; set; }
    }
}