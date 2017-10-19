using Orchard.ContentManagement.Records;

namespace CloudBust.Foundation.Models {
    public class FeaturedHeadPartRecord : ContentPartRecord
    {
        public virtual string BackgroundColor { get; set; }
        public virtual string ForegroundColor { get; set; }
        public virtual string BackgroundColorMedium { get; set; }
        public virtual string ForegroundColorMedium { get; set; }
        public virtual string BackgroundColorLarge { get; set; }
        public virtual string ForegroundColorLarge { get; set; }
        public virtual string BackgroundImageMedium { get; set; }        
        public virtual string BackgroundImageLarge { get; set; }
        public virtual int HeightLarge { get; set; }
        public virtual int HeightMedium { get; set; }

        public FeaturedHeadPartRecord()
        {
            HeightLarge = 650;
            HeightMedium = 525;
            BackgroundColor = "#23b9d1";
            BackgroundColorMedium = "#bcd110";
            BackgroundColorLarge = "#efa412";
            ForegroundColor = "#fff";
            ForegroundColorMedium = "#fff";
            ForegroundColorLarge = "#fff";
        }
    }
}