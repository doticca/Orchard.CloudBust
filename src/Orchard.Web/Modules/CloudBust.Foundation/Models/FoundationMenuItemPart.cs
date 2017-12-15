using Orchard.ContentManagement;

namespace CloudBust.Foundation.Models {
    public class FoundationMenuItemPart : ContentPart<FoundationMenuItemPartRecord> {
        
        public bool DisplayAsButton {
            get { return Record.DisplayAsButton; }
            set { Record.DisplayAsButton = value; }
        }
        public bool isRoot
        {
            get { return Record.isRoot; }
            set { Record.isRoot = value; }
        }
        public bool LeftSide
        {
            get { return Record.LeftSide; }
            set { Record.LeftSide = value; }
        }
        public bool Divider
        {
            get { return Record.Divider; }
            set { Record.Divider = value; }
        }
        public bool RightSide
        {
            get { return Record.RightSide; }
            set { Record.RightSide = value; }
        }
        public string CustomCss
        {
            get { return Record.CustomCss; }
            set { Record.CustomCss = value; }
        }
    }
}
