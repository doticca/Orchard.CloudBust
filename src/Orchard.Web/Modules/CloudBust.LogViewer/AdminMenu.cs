using CloudBust.LogViewer.CustomUtils;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace CloudBust.LogViewer
{
    public class AdminMenu : INavigationProvider
    {
        public string MenuName
        {
            get { return "admin"; }
        }

        public AdminMenu()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder

                // Image set
                .AddImageSet("logviewer")

                // Menu item
                .Add(item => item

                    .Caption(T("Log viewer"))
                    .Position("19")
                    .Action("Index", "Admin", new { area = LogConstants.ModulePath })
                );
        }
    }
}