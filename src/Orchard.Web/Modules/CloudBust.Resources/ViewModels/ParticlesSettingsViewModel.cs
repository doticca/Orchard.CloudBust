using Orchard.Environment.Extensions;
using System.Collections.Generic;

namespace CloudBust.Resources.ViewModels
{
    [OrchardFeature("CloudBust.Resources.Particles")]
    public class ParticlesSettingsViewModel
    {
        public string JsonUrl { get; set; }
        public bool AutoEnable { get; set; }
        public bool AutoEnableAdmin { get; set; }
        public IEnumerable<string> JsonUrlSuggestions { get; set; }

    }
}