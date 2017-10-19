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
    [OrchardFeature("CloudBust.Resources.FontAwesome")]
    public class FontAwesomeShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IFontAwesomeService _fontawesomeService;
        public FontAwesomeShapes(Work<WorkContext> workContext, IFontAwesomeService fontawesomeService)
        {
            _workContext = workContext;
            _fontawesomeService = fontawesomeService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadLinks")
            .OnDisplaying(shapeDisplayingContext =>
            {
                if (!_fontawesomeService.GetAutoEnable()) return;
                if (!_fontawesomeService.GetAutoEnableAdmin())
                {
                    var request = _workContext.Value.HttpContext.Request;
                    if (AdminFilter.IsApplied(request.RequestContext))
                    {
                        return;
                    }
                }


                var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                var scripts = resourceManager.GetRequiredResources("stylesheet");

                string includecss = "FontAwesome";
                            // check to see if another module declared a script foundation. If so, we do nothing
                            var currentFontAwesome = scripts
                                    .Where(l => l.Name == includecss)
                                    .FirstOrDefault();

                if (currentFontAwesome == null)
                {
                                // temp untill we fix the logic
                                resourceManager.Require("stylesheet", includecss).AtHead();

                }

            });
        }
    }
}