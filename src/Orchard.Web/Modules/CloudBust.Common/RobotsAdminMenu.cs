using Orchard.Localization;
using Orchard.UI.Navigation;
using Orchard.Environment.Extensions;

namespace CloudBust.Common {

    [OrchardFeature("CloudBust.Common.SEO")]
    public class RobotsAdminMenu : INavigationProvider {
		public Localizer T { get; set; }
		public string MenuName { get { return "admin"; } }

		public void GetNavigation(NavigationBuilder builder) {
			builder.Add(T("Robots.txt"), "50",
				menu => menu.Add(T("Robots.txt"), "20", item => item.Action("Index", "AdminRobots", new { area = "CloudBust.Common" })
					.Permission(RobotsPermissions.ConfigureRobotsTextFile)));
		}
	}
}
