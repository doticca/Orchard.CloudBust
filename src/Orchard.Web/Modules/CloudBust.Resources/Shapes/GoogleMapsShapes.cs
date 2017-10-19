using System.Linq;
using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment.Extensions;
using Orchard.UI.Resources;
using CloudBust.Resources.Services;
using Orchard.Environment;
using Orchard.UI.Admin;


namespace CloudBust.Resources.Shapes
{
    [OrchardFeature("CloudBust.Resources.GoogleMaps")]
    public class GoogleMapsShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IGoogleMapsService _googlemapsService;
        public GoogleMapsShapes(Work<WorkContext> workContext, IGoogleMapsService googlemapsService)
        {
            _workContext = workContext;
            _googlemapsService = googlemapsService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_googlemapsService.GetAutoEnable()) return;
                    if (!_googlemapsService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    string includejs = "GoogleMaps";
                    var currentHighlight = scripts
                            .Where(l => l.Name == includejs)
                            .FirstOrDefault();

                    if (currentHighlight == null)
                    {
                        resourceManager.Require("script", includejs).AtHead();
                    }       
             
                });
        }
    }
}