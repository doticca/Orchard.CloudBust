using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.Particles")]
    public class ParticlesSettingsPartRecord : ContentPartRecord
    {
        public virtual string JsonUrl { get; set; }
        public virtual bool AutoEnable { get; set; }
        public virtual bool AutoEnableAdmin { get; set; }

        public ParticlesSettingsPartRecord()
        {
            JsonUrl = "particles.json";
            AutoEnable = true;
            AutoEnableAdmin = false;
        }
    }
}