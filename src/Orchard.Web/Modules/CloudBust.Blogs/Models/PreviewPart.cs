using Orchard.ContentManagement;

namespace CloudBust.Blogs.Models
{
    public class PreviewPart : ContentPart<PreviewPartRecord>
    {
        public string PreviewText
        {
            get { return Record.PreviewText; }
            set { Record.PreviewText = value; }
        }
    }
}
