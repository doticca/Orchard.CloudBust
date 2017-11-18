using CloudBust.Foundation.Services;
using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment;
using Orchard.Environment.Extensions;
using Orchard.UI.Admin;

namespace CloudBust.Foundation.Shapes
{
    [OrchardFeature("CloudBust.Foundation.Zones")]
    public class FoundationZoneShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IFoundationService _foundationService;
        public FoundationZoneShapes(Work<WorkContext> workContext, IFoundationService foundationService)
        {
            _workContext = workContext;
            _foundationService = foundationService;
        }
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Zone")
                .OnDisplaying(displaying =>
                {
                    var request = _workContext.Value.HttpContext.Request;
                    if (AdminFilter.IsApplied(request.RequestContext) && !_foundationService.GetAutoEnableAdmin()) return;
                    if (!AdminFilter.IsApplied(request.RequestContext) && _foundationService.GetDoNotEnableFrontEnd()) return;
                    var zoneItem = displaying.Shape;

                    zoneItem.Metadata.Alternates.Add("FoundationZone");

                });
        }
    }
}