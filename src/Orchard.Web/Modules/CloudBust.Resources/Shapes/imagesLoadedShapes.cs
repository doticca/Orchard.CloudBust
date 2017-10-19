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
    [OrchardFeature("CloudBust.Resources.ImagesLoaded")]
    public class imagesLoadedShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IimagesLoadedService _imagesLoadedService;
        public imagesLoadedShapes(Work<WorkContext> workContext, IimagesLoadedService imagesLoadedService)
        {
            _workContext = workContext;
            _imagesLoadedService = imagesLoadedService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_imagesLoadedService.GetAutoEnable()) return;
                    if (!_imagesLoadedService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    string includejs = "imagesLoaded";
                    var currentHighlight = scripts
                            .Where(l => l.Name == includejs)
                            .FirstOrDefault();

                    if (currentHighlight == null)
                    {
                        resourceManager.Require("script", includejs).AtFoot();
                    }       
             
                });
        }
    }
}