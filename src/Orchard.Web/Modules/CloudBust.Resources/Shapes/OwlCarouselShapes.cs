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
    [OrchardFeature("CloudBust.Resources.OwlCarousel")]
    public class OwlCarouselShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IOwlCarouselService _owlcarouselService;
        public OwlCarouselShapes(Work<WorkContext> workContext, IOwlCarouselService owlcarouselService)
        {
            _workContext = workContext;
            _owlcarouselService = owlcarouselService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadLinks")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_owlcarouselService.GetAutoEnable()) return;
                    if (!_owlcarouselService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }


                        var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                        var scripts = resourceManager.GetRequiredResources("stylesheet");

                        string includecss = "OwlCarousel";
                        var currentOwlCarousel = scripts
                                .Where(l => l.Name == includecss)
                                .FirstOrDefault();

                        if (currentOwlCarousel == null)
                        {
                            // temp untill we fix the logic
                            resourceManager.Require("stylesheet", includecss).AtHead();

                        }
                    
                });

            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_owlcarouselService.GetAutoEnable()) return;
                    if (!_owlcarouselService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    string includejs = "OwlCarousel";
                    var currentOwlCarousel = scripts
                            .Where(l => l.Name == includejs)
                            .FirstOrDefault();

                    if (currentOwlCarousel == null)
                    {
                        resourceManager.Require("script", includejs).AtFoot();
                    }       
             
                });
        }
    }
}