using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;

namespace CloudBust.Foundation.Models
{
    public class QuadPromoPart : ContentPart<QuadPromoPartRecord>
    {
        public string Text
        {
            get { return this.As<BodyPart>().Text; }
            set { this.As<BodyPart>().Text = value; }
        }
        public string FirstImage
        {
            get { return Record.FirstImage; }
            set { Record.FirstImage = value; }
        }
        public string SecondImage { get { return Record.SecondImage; } set { Record.SecondImage = value; } }
        public string ThirdImage { get { return Record.ThirdImage; } set { Record.ThirdImage = value; } }
        public string FourthImage { get { return Record.FourthImage; } set { Record.FourthImage = value; } }
        public string FirstTitle { get { return Record.FirstTitle; } set { Record.FirstTitle = value; } }
        public string SecondTitle { get { return Record.SecondTitle; } set { Record.SecondTitle = value; } }
        public string ThirdTitle { get { return Record.ThirdTitle; } set { Record.ThirdTitle = value; } }
        public string FourthTitle { get { return Record.FourthTitle; } set { Record.FourthTitle = value; } }
        public string FirstPromoText { get { return Record.FirstPromoText; } set { Record.FirstPromoText = value; } }
        public string SecondPromoText { get { return Record.SecondPromoText; } set { Record.SecondPromoText = value; } }
        public string ThirdPromoText { get { return Record.ThirdPromoText; } set { Record.ThirdPromoText = value; } }
        public string FourthPromoText { get { return Record.FourthPromoText; } set { Record.FourthPromoText = value; } }
        public string FirstLinkText { get { return Record.FirstLinkText; } set { Record.FirstLinkText = value; } }
        public string SecondLinkText { get { return Record.SecondLinkText; } set { Record.SecondLinkText = value; } }
        public string ThirdLinkText { get { return Record.ThirdLinkText; } set { Record.ThirdLinkText = value; } }
        public string FourthLinkText { get { return Record.FourthLinkText; } set { Record.FourthLinkText = value; } }
        public string FirstLinkUrl { get { return Record.FirstLinkUrl; } set { Record.FirstLinkUrl = value; } }
        public string SecondLinkUrl { get { return Record.SecondLinkUrl; } set { Record.SecondLinkUrl = value; } }
        public string ThirdLinkUrl { get { return Record.ThirdLinkUrl; } set { Record.ThirdLinkUrl = value; } }
        public string FourthLinkUrl { get { return Record.FourthLinkUrl; } set { Record.FourthLinkUrl = value; } }
        public string FirstLinkColor { get { return Record.FirstLinkColor; } set { Record.FirstLinkColor = value; } }
        public string SecondLinkColor { get { return Record.SecondLinkColor; } set { Record.SecondLinkColor = value; } }
        public string ThirdLinkColor { get { return Record.ThirdLinkColor; } set { Record.ThirdLinkColor = value; } }
        public string FourthLinkColor { get { return Record.FourthLinkColor; } set { Record.FourthLinkColor = value; } }

    }
}
