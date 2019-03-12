using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.UI.Resources;
using Orchard.UI.Admin;
using Orchard.Environment;
using Orchard.Localization;
using Orchard.Modules.Services;

namespace CloudBust.Dashboard.Shapes
{
    public class DashboardShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IModuleService _moduleService;
        public DashboardShapes(Work<WorkContext> workContext, IModuleService moduleService)
        {
            _workContext = workContext;
            _moduleService = moduleService;
            T = NullLocalizer.Instance;
        }
        public Localizer T { get; set; }
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadLinks")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    var request = _workContext.Value.HttpContext.Request;
                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("stylesheet");                    


                        if (!AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                        resourceManager.Include("stylesheet", "~/modules/cloudbust.dashboard/styles/cloudbust.css", "~/modules/cloudbust.dashboard/styles/cloudbust.css").AtHead();
                    
                });
        }
    }
}