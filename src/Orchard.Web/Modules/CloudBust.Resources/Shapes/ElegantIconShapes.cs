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
    [OrchardFeature("CloudBust.Resources.ElegantIcon")]
    public class ElegantIconShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IElegantIconService _eleganticonService;
        public ElegantIconShapes(Work<WorkContext> workContext, IElegantIconService eleganticonService)
        {
            _workContext = workContext;
            _eleganticonService = eleganticonService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadLinks")
            .OnDisplaying(shapeDisplayingContext =>
            {
                if (!_eleganticonService.GetAutoEnable()) return;
                if (!_eleganticonService.GetAutoEnableAdmin())
                {
                    var request = _workContext.Value.HttpContext.Request;
                    if (AdminFilter.IsApplied(request.RequestContext))
                    {
                        return;
                    }
                }


                var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                var scripts = resourceManager.GetRequiredResources("stylesheet");

                string includecss = "ElegantIcon";
                            // check to see if another module declared a script foundation. If so, we do nothing
                            var currentElegantIcon = scripts
                                    .Where(l => l.Name == includecss)
                                    .FirstOrDefault();

                if (currentElegantIcon == null)
                {
                                // temp untill we fix the logic
                                resourceManager.Require("stylesheet", includecss).AtHead();

                }

            });
        }
    }
}