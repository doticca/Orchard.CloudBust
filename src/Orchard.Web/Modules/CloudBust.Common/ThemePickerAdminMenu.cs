using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace CloudBust.Common.Menus {

    [OrchardFeature("CloudBust.Common.ThemePicker")]
    public class ThemePickerAdminMenu : INavigationProvider {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder.AddImageSet("themes")
                .Add(
                    T("Themes"), "10",
                    menu => menu.Action("Index", "Admin", new {area = "Orchard.Themes"})
                                .Permission(Orchard.Themes.Permissions.ApplyTheme)
                                .Add(
                                    T("Picker"), "4",
                                    item => item.Action("Index", "Admin", new {area = "CloudBust.Common"})
                                                .Permission(Orchard.Themes.Permissions.ApplyTheme).LocalNav()));
        }
    }
}