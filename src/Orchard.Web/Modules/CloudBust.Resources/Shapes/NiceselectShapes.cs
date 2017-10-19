using System.Linq;
using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment.Extensions;
using Orchard.UI.Resources;
using CloudBust.Resources.Services;
using Orchard.Environment;

namespace CloudBust.Resources.Shapes
{
    [OrchardFeature("CloudBust.Resources.Niceselect")]
    public class NiceselectShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly INiceselectService _niceselectService;
        public NiceselectShapes(Work<WorkContext> workContext, INiceselectService niceselectService)
        {
            _workContext = workContext;
            _niceselectService = niceselectService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_niceselectService.GetAutoEnable()) return;

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    string includejs = "Niceselect";
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