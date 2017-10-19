using CloudBust.Foundation.Services;
using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment;
using Orchard.UI.Admin;

namespace CloudBust.Foundation.Shapes
{
    public class FoundationWidgetShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IFoundationService _foundationService;
        public FoundationWidgetShapes(Work<WorkContext> workContext, IFoundationService foundationService)
        {
            _workContext = workContext;
            _foundationService = foundationService;
        }
        public void Discover(ShapeTableBuilder builder)
        {                       
            builder.Describe("Widget")
                .OnDisplaying(displaying =>
                {
                    var request = _workContext.Value.HttpContext.Request;
                    if (AdminFilter.IsApplied(request.RequestContext) || _foundationService.GetDoNotEnableFrontEnd()) return;
                    var zoneItem = displaying.Shape;

                    zoneItem.Metadata.Alternates.Add("FoundationWidget");

                });
        }
    }
}