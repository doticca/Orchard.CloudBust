using System.Linq;
using CloudBust.Application.Services;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;
using Orchard;
using CloudBust.Application;
using CloudBust.Application.Models;

namespace CloudBust.Dashboard {
    public class AdminMenu : INavigationProvider {
        private readonly ISettingsService _settingsService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IWorkContextAccessor _workContextAccessor;

        public AdminMenu(
            ISettingsService settingsService,
            IAuthorizationService authorizationService, 
            IWorkContextAccessor workContextAccessor) {
            _settingsService = settingsService;
            _authorizationService = authorizationService;
            _workContextAccessor = workContextAccessor;
        }

        public Localizer T { get; set; }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder//.AddImageSet("cloudbust")
                .Add(T("CloudBust"), "1", BuildMenu);
        }

        private void BuildMenu(NavigationItemBuilder menu) {
            ApplicationRecord app = _settingsService.GetWebApplication();

            menu.Add(T("Dashboard"), "1.0",
                item => item.Action("index", "Dashboard", new { area = "CloudBust.Dashboard" }).Permission(Permissions.ManageApps));
            menu.Add(T("Applications"), "1.1",
                item => item.Action("applications", "Dashboard", new { area = "CloudBust.Dashboard" }).Permission(Permissions.ManageApps));
            menu.Add(T("Games"), "1.2",
                item => item.Action("games", "Dashboard", new { area = "CloudBust.Dashboard" }).Permission(Permissions.ManageApps));

            if(app!=null)

                menu.Add(T("Manage " + app.Name), "1.3",
                    item => item.Action("Application", "Dashboard", new { area = "CloudBust.Dashboard", appID = app.AppKey }).Permission(Permissions.ManageOwnApps));

            
        }
    }
}