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
    [OrchardFeature("CloudBust.Resources.Underscore")]
    public class UnderscoreShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IUnderscoreService _underscoreService;
        public UnderscoreShapes(Work<WorkContext> workContext, IUnderscoreService underscoreService)
        {
            _workContext = workContext;
            _underscoreService = underscoreService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_underscoreService.GetAutoEnable()) return;
                    if (!_underscoreService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    var currentUnderscore = scripts
                            .Where(l => l.Name == "Underscore")
                            .FirstOrDefault();

                    if (currentUnderscore == null)
                    {
                        resourceManager.Require("script", "Underscore").AtHead();
                    }                    
                });
        }
    }
}