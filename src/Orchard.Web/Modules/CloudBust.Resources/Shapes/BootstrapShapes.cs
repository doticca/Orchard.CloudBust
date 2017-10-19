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
    [OrchardFeature("CloudBust.Resources.Bootstrap")]
    public class BootstrapShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IBootstrapService _bootstrapService;
        public BootstrapShapes(Work<WorkContext> workContext, IBootstrapService bootstrapService)
        {
            _workContext = workContext;
            _bootstrapService = bootstrapService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadLinks")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_bootstrapService.GetAutoEnable()) return;
                    if (!_bootstrapService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }


                        var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                        var scripts = resourceManager.GetRequiredResources("stylesheet");

                        string includecss = "Bootstrap";
                        // check to see if another module declared a script bootstrap. If so, we do nothing
                        var currentBootstrap = scripts
                                .Where(l => l.Name == includecss)
                                .FirstOrDefault();

                        if (currentBootstrap == null)
                        {
                            // temp untill we fix the logic
                            resourceManager.Require("stylesheet", includecss).AtHead();

                        }
                    
                });

            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_bootstrapService.GetAutoEnable()) return;
                    if (!_bootstrapService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    string includejs = "Bootstrap";
                    var currentBootstrap = scripts
                            .Where(l => l.Name == includejs)
                            .FirstOrDefault();

                    if (currentBootstrap == null)
                    {
                        resourceManager.Require("script", includejs).AtFoot();
                    }       
             
                });
        }
    }
}