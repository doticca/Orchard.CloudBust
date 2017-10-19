using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Resources.ViewModels
{
    [OrchardFeature("CloudBust.Resources.Masonry")]
    public class MasonrySettingsViewModel
    {
        public bool AutoEnable { get; set; }
        public bool AutoEnableAdmin { get; set; }
    }
}