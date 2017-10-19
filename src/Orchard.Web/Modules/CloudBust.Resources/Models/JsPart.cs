using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.Ace")]
    public class JsPart : ContentPart<JsPartRecord>
    {
        public string Js
        {
            get { return Record.Js; }
            set { Record.Js = value; }
        }
    }
}
