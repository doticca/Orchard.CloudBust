using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.Ace")]
    public class CssPart : ContentPart<CssPartRecord>
    {
        public string Css
        {
            get { return Record.Css; }
            set { Record.Css = value; }
        }
    }
}
