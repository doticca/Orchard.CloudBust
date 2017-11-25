using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs.Models
{
    [OrchardFeature("CloudBust.Blogs.Stats")]
    public class ViewersPart : ContentPart
    {
        public bool ShowVoter { get; set; }

        public bool AllowAnonymousRatings { get; set; }

        public double ResultValue { get; set; }

        public double Count { get; set; }

        public double UserRating { get; set; }
    }
}