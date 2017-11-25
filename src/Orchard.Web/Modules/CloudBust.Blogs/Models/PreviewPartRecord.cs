using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace CloudBust.Blogs.Models
{
    public class PreviewPartRecord : ContentPartRecord
    {
        [StringLengthMax]
        public virtual string PreviewText { get; set; }
    }
}