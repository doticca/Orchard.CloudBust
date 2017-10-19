using Orchard.ContentManagement.Records;

namespace CloudBust.Foundation.Models {
    public class QuadPromoPartRecord : ContentPartRecord
    {
        public virtual string FirstImage { get; set; }        
        public virtual string SecondImage { get; set; }
        public virtual string ThirdImage { get; set; }
        public virtual string FourthImage { get; set; }
        public virtual string FirstTitle { get; set; }
        public virtual string SecondTitle { get; set; }
        public virtual string ThirdTitle { get; set; }
        public virtual string FourthTitle { get; set; }
        public virtual string FirstPromoText { get; set; }
        public virtual string SecondPromoText { get; set; }
        public virtual string ThirdPromoText { get; set; }
        public virtual string FourthPromoText { get; set; }
        public virtual string FirstLinkText { get; set; }
        public virtual string SecondLinkText { get; set; }
        public virtual string ThirdLinkText { get; set; }
        public virtual string FourthLinkText { get; set; }
        public virtual string FirstLinkUrl { get; set; }
        public virtual string SecondLinkUrl { get; set; }
        public virtual string ThirdLinkUrl { get; set; }
        public virtual string FourthLinkUrl { get; set; }
        public virtual string FirstLinkColor { get; set; }
        public virtual string SecondLinkColor { get; set; }
        public virtual string ThirdLinkColor { get; set; }
        public virtual string FourthLinkColor { get; set; }

        public QuadPromoPartRecord()
        {
            FirstImage = string.Empty;
            SecondImage = string.Empty;
            ThirdImage = string.Empty;
            FourthImage = string.Empty;
            FirstTitle = string.Empty;
            SecondTitle = string.Empty;
            ThirdTitle = string.Empty;
            FourthTitle = string.Empty;
            FirstLinkText = string.Empty;
            SecondLinkText = string.Empty;
            ThirdLinkText = string.Empty;
            FourthLinkText = string.Empty;
            FirstLinkUrl = string.Empty;
            SecondLinkUrl = string.Empty;
            ThirdLinkUrl = string.Empty;
            FourthLinkUrl = string.Empty;
            FirstPromoText = string.Empty;
            SecondPromoText = string.Empty;
            ThirdPromoText = string.Empty;
            FourthPromoText = string.Empty;
            FirstLinkColor = string.Empty;
            SecondLinkColor = string.Empty;
            ThirdLinkColor = string.Empty;
            FourthLinkColor = string.Empty;
        }
    }
}