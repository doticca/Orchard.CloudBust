using System.Collections.Generic;
using Orchard.Environment.Extensions;
using CloudBust.Common.Models;

namespace CloudBust.Common.ViewModels {

    [OrchardFeature("CloudBust.Common.ThemePicker")]
    public class ThemePickerIndexViewModel {
        public IEnumerable<ThemePickerSettingsRecord> ThemeSelectionSettings { get; set; }
        public IEnumerable<string> ThemeSelectionRules { get; set; }
        public IEnumerable<string> Themes { get; set; }
    }
}