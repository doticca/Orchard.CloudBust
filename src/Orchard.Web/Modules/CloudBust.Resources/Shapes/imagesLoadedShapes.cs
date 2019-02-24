using System.Linq;
using CloudBust.Resources.Services;
using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment;
using Orchard.Environment.Extensions;
using Orchard.UI.Admin;
using Orchard.UI.Resources;

namespace CloudBust.Resources.Shapes {
    [OrchardFeature("CloudBust.Resources.ImagesLoaded")]
    public class ImagesLoadedShapes : IShapeTableProvider {
        private readonly IImagesLoadedService _imagesLoadedService;
        private readonly Work<WorkContext> _workContext;

        public ImagesLoadedShapes(Work<WorkContext> workContext, IImagesLoadedService imagesLoadedService) {
            _workContext = workContext;
            _imagesLoadedService = imagesLoadedService;
        }

        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("HeadScripts")
                   .OnDisplaying(shapeDisplayingContext => {
                        if (!_imagesLoadedService.GetAutoEnable()) return;
                        if (!_imagesLoadedService.GetAutoEnableAdmin()) {
                            var request = _workContext.Value.HttpContext.Request;
                            if (AdminFilter.IsApplied(request.RequestContext)) return;
                        }

                        var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                        var scripts = resourceManager.GetRequiredResources("script");


                        const string includejs = "ImagesLoaded";
                        var currentHighlight = scripts.FirstOrDefault(l => l.Name == includejs);

                        if (currentHighlight == null) resourceManager.Require("script", includejs).AtFoot();
                    });
        }
    }
}