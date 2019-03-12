using System;
using System.Linq;
using Orchard.Commands;
using Orchard.ContentManagement;
using Orchard.Security;
using Orchard.Settings;
using Orchard.Widgets.Models;
using Orchard.Widgets.Services;

namespace Orchard.Widgets.Commands
{
    public class FoundationWidgetCommands : DefaultOrchardCommandHandler {
        private readonly IWidgetsService _widgetsService;
        private readonly ISiteService _siteService;
        private readonly IMembershipService _membershipService;
        private readonly IContentManager _contentManager;

        private const string LoremIpsum = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur a nibh ut tortor dapibus vestibulum. Aliquam vel sem nibh. Suspendisse vel condimentum tellus.</p>";

        public FoundationWidgetCommands(
            IWidgetsService widgetsService, 
            ISiteService siteService, 
            IMembershipService membershipService,
            IContentManager contentManager) {
            _widgetsService = widgetsService;
            _siteService = siteService;
            _membershipService = membershipService;
            _contentManager = contentManager;
        }

        [OrchardSwitch]
        public string Title { get; set; }


        [CommandName("widget delete")]
        [CommandHelp("widget delete /Title:<title>\r\n\t" + "Delete a widget")]
        [OrchardSwitches("Title")]
        public void Delete() {

            var widget = _widgetsService.GetWidgets().Where(x=>x.Title == Title).FirstOrDefault();
            if(widget!= null)
            {
                _widgetsService.DeleteWidget(widget.Id);
            }     

            Context.Output.WriteLine(T("Widget deleted successfully.").Text);
        }

        private LayerPart GetLayer(string layer) {
            var layers = _widgetsService.GetLayers();
            return layers.FirstOrDefault(layerPart => String.Equals(layerPart.Name, layer, StringComparison.OrdinalIgnoreCase));
        }
    }
}