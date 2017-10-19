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
    [OrchardFeature("CloudBust.Resources.Magnific")]
    public class MagnificShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IMagnificService _magnificService;
        public MagnificShapes(Work<WorkContext> workContext, IMagnificService magnificService)
        {
            _workContext = workContext;
            _magnificService = magnificService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadLinks")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_magnificService.GetAutoEnable()) return;
                    if (!_magnificService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }


                        var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                        var scripts = resourceManager.GetRequiredResources("stylesheet");

                        string includecss = "Magnific";
                        // check to see if another module declared a script foundation. If so, we do nothing
                        var currentMagnific = scripts
                                .Where(l => l.Name == includecss)
                                .FirstOrDefault();

                        if (currentMagnific == null)
                        {
                            // temp untill we fix the logic
                            resourceManager.Require("stylesheet", includecss).AtHead();

                        }
                    
                });

            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_magnificService.GetAutoEnable()) return;
                    if (!_magnificService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    string includejs = "Magnific";
                    var currentMagnific = scripts
                            .Where(l => l.Name == includejs)
                            .FirstOrDefault();

                    if (currentMagnific == null)
                    {
                        resourceManager.Require("script", includejs).AtFoot();
                    }       
             
                });
        }
    }
}