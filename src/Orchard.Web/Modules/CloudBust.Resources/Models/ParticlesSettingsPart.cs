using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.Particles")]
    public class ParticlesSettingsPart : ContentPart<ParticlesSettingsPartRecord> {    
        public string JsonUrl
        {
            get { return Record.JsonUrl; }
            set { Record.JsonUrl = value; }
        }
        public bool AutoEnable
        {
            get { return Record.AutoEnable; }
            set { Record.AutoEnable = value; }
        }
        public bool AutoEnableAdmin
        {
            get { return Record.AutoEnableAdmin; }
            set { Record.AutoEnableAdmin = value; }
        }
    }
}