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
    [OrchardFeature("CloudBust.Resources.IziModal")]
    public class IziModalShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IIziModalService _izimodalService;
        public IziModalShapes(Work<WorkContext> workContext, IIziModalService izimodalService)
        {
            _workContext = workContext;
            _izimodalService = izimodalService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadLinks")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_izimodalService.GetAutoEnable()) return;
                    if (!_izimodalService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }


                        var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                        var scripts = resourceManager.GetRequiredResources("stylesheet");

                        string includecss = "IziModal";
                        var currentIziModal = scripts
                                .Where(l => l.Name == includecss)
                                .FirstOrDefault();

                        if (currentIziModal == null)
                        {
                            // temp untill we fix the logic
                            resourceManager.Require("stylesheet", includecss).AtHead();

                        }
                    
                });

            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_izimodalService.GetAutoEnable()) return;
                    if (!_izimodalService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    string includejs = "IziModal";
                    var currentIziModal = scripts
                            .Where(l => l.Name == includejs)
                            .FirstOrDefault();

                    if (currentIziModal == null)
                    {
                        resourceManager.Require("script", includejs).AtFoot();
                    }       
             
                });
        }
    }
}