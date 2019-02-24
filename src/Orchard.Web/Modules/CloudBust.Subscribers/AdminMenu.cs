using System.Linq;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;
using Orchard;
using Orchard.Core.Contents;

namespace CloudBust.Subscribers {
    public class AdminMenu : INavigationProvider {
        private readonly IAuthorizationService _authorizationService;
        private readonly IWorkContextAccessor _workContextAccessor;

        public AdminMenu(
            IAuthorizationService authorizationService, 
            IWorkContextAccessor workContextAccessor) {
            _authorizationService = authorizationService;
            _workContextAccessor = workContextAccessor;
        }

        public Localizer T { get; set; }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder
                .Add(T("CloudBust"), "1", BuildMenu);
        }

        private void BuildMenu(NavigationItemBuilder menu) {
            menu.Add(T("Subscribers"), "1.4",
                item => item.Action("index", "Admin", new {area = "CloudBust.Subscribers"}).Permission(Permissions.PublishContent));            
        }
    }
}