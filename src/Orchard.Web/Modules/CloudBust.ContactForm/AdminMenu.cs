using System.Linq;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;
using Orchard;
using Orchard.Core.Contents;

namespace CloudBust.ContactForm {
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
            menu.Add(T("Contact Form"), "1.5",
                item => item.Action("index", "Admin", new {area = "CloudBust.ContactForm"}).Permission(Permissions.PublishContent));            
        }
    }
}