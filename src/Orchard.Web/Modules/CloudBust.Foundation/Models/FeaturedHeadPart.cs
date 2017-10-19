using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;

namespace CloudBust.Foundation.Models {
    public class FeaturedHeadPart : ContentPart<FeaturedHeadPartRecord>
    {
        public string Text
        {
            get { return this.As<BodyPart>().Text; }
            set { this.As<BodyPart>().Text = value; }
        }
        public string BackgroundColor
        {
            get { return Record.BackgroundColor; }
            set { Record.BackgroundColor = value; }
        }
        public string ForegroundColor
        {
            get { return Record.ForegroundColor; }
            set { Record.ForegroundColor = value; }
        }
        public string BackgroundColorMedium
        {
            get { return Record.BackgroundColorMedium; }
            set { Record.BackgroundColorMedium = value; }
        }
        public string ForegroundColorMedium
        {
            get { return Record.ForegroundColorMedium; }
            set { Record.ForegroundColorMedium = value; }
        }
        public string BackgroundColorLarge
        {
            get { return Record.BackgroundColorLarge; }
            set { Record.BackgroundColorLarge = value; }
        }
        public string ForegroundColorLarge
        {
            get { return Record.ForegroundColorLarge; }
            set { Record.ForegroundColorLarge = value; }
        }
        public string BackgroundImageMedium
        {
            get { return Record.BackgroundImageMedium; }
            set { Record.BackgroundImageMedium = value; }
        }
        public string BackgroundImageLarge
        {
            get { return Record.BackgroundImageLarge; }
            set { Record.BackgroundImageLarge = value; }
        }
        public int HeightLarge
        {
            get { return Record.HeightLarge; }
            set { Record.HeightLarge = value; }
        }
        public int HeightMedium
        {
            get { return Record.HeightMedium; }
            set { Record.HeightMedium = value; }
        }

    }
}
