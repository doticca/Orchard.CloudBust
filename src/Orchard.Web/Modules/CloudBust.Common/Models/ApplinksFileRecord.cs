using Orchard.Environment.Extensions;

namespace CloudBust.Common.Models {

    [OrchardFeature("CloudBust.Common.SEO")]
    public class ApplinksFileRecord {
		public virtual int Id { get; set; }
		public virtual string FileContent { get; set; }
	}
}