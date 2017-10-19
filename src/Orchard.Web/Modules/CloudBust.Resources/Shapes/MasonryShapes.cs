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
    [OrchardFeature("CloudBust.Resources.Masonry")]
    public class MasonryShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IMasonryService _masonryService;
        public MasonryShapes(Work<WorkContext> workContext, IMasonryService masonryService)
        {
            _workContext = workContext;
            _masonryService = masonryService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_masonryService.GetAutoEnable()) return;
                    if (!_masonryService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    string includejs = "Masonry";
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