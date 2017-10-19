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
    [OrchardFeature("CloudBust.Resources.Notify")]
    public class NotifyShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly INotifyService _notifyService;
        public NotifyShapes(Work<WorkContext> workContext, INotifyService notifyService)
        {
            _workContext = workContext;
            _notifyService = notifyService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_notifyService.GetAutoEnable()) return;
                    if (!_notifyService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    var currentNotify = scripts
                            .Where(l => l.Name == "Notify")
                            .FirstOrDefault();

                    if (currentNotify == null)
                    {
                        resourceManager.Require("script", "Notify").AtHead();
                    }                    
                });
        }
    }
}