using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;

namespace CloudBust.Resources.Models {
    public class ParticlesPart : ContentPart<ParticlesPartRecord>
    {
        public string Text
        {
            get { return this.As<BodyPart>().Text; }
            set { this.As<BodyPart>().Text = value; }
        }

        public string JsonUrl
        {
            get { return Record.JsonUrl; }
            set { Record.JsonUrl = value; }
        }
    }
}
