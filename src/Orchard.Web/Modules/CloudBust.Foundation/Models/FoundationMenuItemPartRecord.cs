using Orchard.ContentManagement.Records;

namespace CloudBust.Foundation.Models {
    public class FoundationMenuItemPartRecord : ContentPartRecord {
        public virtual bool DisplayAsButton { get; set; }
        public virtual bool isRoot { get; set; }
        public virtual bool LeftSide { get; set; }
        public virtual bool Divider { get; set; }
        public virtual bool RightSide { get; set; }
        public virtual string CustomCss { get; set; }
    }
}