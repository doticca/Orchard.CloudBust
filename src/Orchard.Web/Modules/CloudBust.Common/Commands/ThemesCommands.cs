using System;
using System.Linq;
using Orchard.Commands;
using Orchard.Data.Migration;
using Orchard.Environment.Descriptor.Models;
using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Models;
using Orchard.Themes.Services;
using CloudBust.Common.Services;

using Orchard.Themes.Models;
using System.Collections.Generic;

namespace Orchard.Themes.Commands {
    public class ThemesCommands : DefaultOrchardCommandHandler     {
        private readonly IDataMigrationManager _dataMigrationManager;
        private readonly ISiteThemeService _siteThemeService;
        private readonly IExtensionManager _extensionManager;
        private readonly ShellDescriptor _shellDescriptor;
        private readonly IThemeService _themeService;
        private readonly ISettingsService _settingsService;
        private readonly IEnumerable<IThemeSelectionRule> _rules;

        public ThemesCommands(IDataMigrationManager dataMigrationManager,
                             ISiteThemeService siteThemeService,
                             IExtensionManager extensionManager,
                             ShellDescriptor shellDescriptor,
                             IThemeService themeService,
                             ISettingsService settingsService,
                             IEnumerable<IThemeSelectionRule> rules) {
            _dataMigrationManager = dataMigrationManager;
            _siteThemeService = siteThemeService;
            _extensionManager = extensionManager;
            _shellDescriptor = shellDescriptor;
            _themeService = themeService;
            _settingsService = settingsService;
            _rules = rules;
        }

        [OrchardSwitch]
        public string ThemeName { get; set; }
        [OrchardSwitch]
        public string Name { get; set; }
        [OrchardSwitch]
        public string Criterion { get; set; }

        [CommandName("theme enable")]
        [CommandHelp("theme enable /ThemeName:<themename>\r\n\t" + "Enable a theme")]
        [OrchardSwitches("ThemeName")]
        public void Enable() {
            var theme = _extensionManager.AvailableExtensions().FirstOrDefault(x => x.Name.Trim().Equals(ThemeName, StringComparison.OrdinalIgnoreCase));
            if (theme == null) {
                Context.Output.WriteLine(T("Could not find theme {0}", ThemeName));
                return;
            }

            if (!_shellDescriptor.Features.Any(sf => sf.Name == theme.Id)) {
                Context.Output.WriteLine(T("Enabling theme \"{0}\"...", ThemeName));
                _themeService.EnableThemeFeatures(theme.Id);
            }

            Context.Output.WriteLine(T("Theme \"{0}\" enabled", ThemeName));
        }

        [CommandName("themepicker add")]
        [CommandHelp("themepicker add <type> /Name:<name> /Criterion:criterion /ThemeName:themename\r\n\t" + "Add a new themepicker rule")]
        [OrchardSwitches("Name,Criterion,ThemeName")]
        public void PickerRule(string type)
        {
            IEnumerable<string> ThemeSelectionRules = _rules.Select(r => r.GetType().Name);
            IEnumerable<string> Themes = _extensionManager.AvailableExtensions()
                    .Where(d => DefaultExtensionTypes.IsTheme(d.ExtensionType) &&
                                _shellDescriptor.Features.Any(sf => sf.Name == d.Id))
                    .Select(d => d.Id)
                    .OrderBy(n => n);

            if (!ThemeSelectionRules.Contains(type))
            {
                Context.Output.WriteLine(T("Add themepicker rule failed : type {0} was not found. Supported rules are: {1}.",
                    type,
                    string.Join(" ", ThemeSelectionRules)));
                return;
            }

            if (!Themes.Contains(ThemeName))
            {
                Context.Output.WriteLine(T("Add themepicker rule failed : theme {0} was not found. Available themes are: {1}.",
                    ThemeName,
                    string.Join(" ", Themes)));
                return;
            }
            _settingsService.Add(Name, type, Criterion, ThemeName, 10, "Hidden", "1");

            Context.Output.WriteLine(T("Theme Picker Rule \"{0}\" enabled", Name));
        }
    }
}
