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
    [OrchardFeature("CloudBust.Resources.Highlight")]
    public class HighlightShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IHighlightService _highlightService;
        public HighlightShapes(Work<WorkContext> workContext, IHighlightService highlightService)
        {
            _workContext = workContext;
            _highlightService = highlightService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadLinks")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_highlightService.GetAutoEnable()) return;
                    if (!_highlightService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }


                        var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                        var scripts = resourceManager.GetRequiredResources("stylesheet");

                    string srv = _highlightService.GetStyle();
                    if (string.IsNullOrWhiteSpace(srv)) srv = "default";
                        string includecss = "Highlight_" + srv;
                        // check to see if another module declared a script highligh. If so, we do nothing
                        var currentHighlight = scripts
                                .Where(l => l.Name == includecss)
                                .FirstOrDefault();

                        if (currentHighlight == null)
                        {
                            // temp untill we fix the logic
                            resourceManager.Require("stylesheet", includecss).AtHead();

                        }
                    
                });

            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_highlightService.GetAutoEnable()) return;
                    if (!_highlightService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    string includejs = _highlightService.GetFullBundle() ? "Highlight_Full" : "Highlight";
                    var currentHighlight = scripts
                            .Where(l => l.Name == includejs)
                            .FirstOrDefault();

                    if (currentHighlight == null)
                    {
                        resourceManager.Require("script", includejs).AtHead();
                    }                    
                });
        }
    }
}