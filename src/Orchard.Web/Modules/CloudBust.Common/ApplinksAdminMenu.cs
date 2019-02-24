using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace CloudBust.Common {
    [OrchardFeature("CloudBust.Common.SEO")]
    public class ApplinksAdminMenu : INavigationProvider {
        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder) {
            builder.Add(T("apple-app-site-association"), "50",
                menu => menu.Add(T("apple-app-site-association"), "20", item => item
                                                                               .Action("Index", "AdminApplinks",
                                                                                    new {area = "CloudBust.Common"})
                                                                               .Permission(ApplinksPermissions
                                                                                   .ConfigureApplinksTextFile)));
        }

        public string MenuName => "admin";
    }
}