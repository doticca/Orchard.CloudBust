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
    [OrchardFeature("CloudBust.Resources.IziToast")]
    public class IziToastShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IIziToastService _izitoastService;
        public IziToastShapes(Work<WorkContext> workContext, IIziToastService izitoastService)
        {
            _workContext = workContext;
            _izitoastService = izitoastService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadLinks")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_izitoastService.GetAutoEnable()) return;
                    if (!_izitoastService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }


                        var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                        var scripts = resourceManager.GetRequiredResources("stylesheet");

                        string includecss = "IziToast";
                        var currentIziToast = scripts
                                .Where(l => l.Name == includecss)
                                .FirstOrDefault();

                        if (currentIziToast == null)
                        {
                            // temp untill we fix the logic
                            resourceManager.Require("stylesheet", includecss).AtHead();

                        }
                    
                });

            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_izitoastService.GetAutoEnable()) return;
                    if (!_izitoastService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    string includejs = "IziToast";
                    var currentIziToast = scripts
                            .Where(l => l.Name == includejs)
                            .FirstOrDefault();

                    if (currentIziToast == null)
                    {
                        resourceManager.Require("script", includejs).AtFoot();
                    }       
             
                });
        }
    }
}