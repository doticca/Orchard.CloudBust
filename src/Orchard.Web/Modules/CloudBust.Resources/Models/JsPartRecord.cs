using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.Ace")]
    public class JsPartRecord : ContentPartRecord
    {
        [StringLengthMax]
        public virtual string Js { get; set; }
    }
}