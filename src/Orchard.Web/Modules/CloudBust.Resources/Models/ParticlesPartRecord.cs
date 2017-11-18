using Orchard.ContentManagement.Records;

namespace CloudBust.Resources.Models {
    public class ParticlesPartRecord : ContentPartRecord
    {
        public virtual string JsonUrl { get; set; }
        public ParticlesPartRecord()
        {

        }
    }
}