using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Models {

    [OrchardFeature("CloudBust.Common.CustomCode")]
    public class CustomCodeSettingsPart : ContentPart {
        public string HeadCode {
            get { return this.Retrieve(x => x.HeadCode); }
            set { this.Store(x => x.HeadCode, value); }
        }

        public string FootCode {
            get { return this.Retrieve(x => x.FootCode); }
            set { this.Store(x => x.FootCode, value); }
        }

    }
}