using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace CloudBust.Foundation.Models {
    public class TrioPromoPartRecord : ContentPartRecord
    {
        public virtual string FirstImage { get; set; }        
        public virtual string SecondImage { get; set; }
        public virtual string ThirdImage { get; set; }
        public virtual string FirstTitle { get; set; }
        public virtual string SecondTitle { get; set; }
        public virtual string ThirdTitle { get; set; }
        [StringLengthMax]
        public virtual string FirstPromoText { get; set; }
        [StringLengthMax]
        public virtual string SecondPromoText { get; set; }
        [StringLengthMax]
        public virtual string ThirdPromoText { get; set; }
        public virtual string FirstLinkText { get; set; }
        public virtual string SecondLinkText { get; set; }
        public virtual string ThirdLinkText { get; set; }
        public virtual string FirstLinkUrl { get; set; }
        public virtual string SecondLinkUrl { get; set; }
        public virtual string ThirdLinkUrl { get; set; }
        public virtual string FirstLinkColor { get; set; }
        public virtual string SecondLinkColor { get; set; }
        public virtual string ThirdLinkColor { get; set; }

        public TrioPromoPartRecord()
        {
            FirstImage = string.Empty;
            SecondImage = string.Empty;
            ThirdImage = string.Empty;
            FirstTitle = string.Empty;
            SecondTitle = string.Empty;
            ThirdTitle = string.Empty;
            FirstLinkText = string.Empty;
            SecondLinkText = string.Empty;
            ThirdLinkText = string.Empty;
            FirstLinkUrl = string.Empty;
            SecondLinkUrl = string.Empty;
            ThirdLinkUrl = string.Empty;
            FirstPromoText = string.Empty;
            SecondPromoText = string.Empty;
            ThirdPromoText = string.Empty;
            FirstLinkColor = string.Empty;
            SecondLinkColor = string.Empty;
            ThirdLinkColor = string.Empty;
        }
    }
}