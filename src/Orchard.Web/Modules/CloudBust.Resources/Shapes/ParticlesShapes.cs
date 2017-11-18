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
    [OrchardFeature("CloudBust.Resources.Particles")]
    public class ParticlesShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IParticlesService _particlesService;
        public ParticlesShapes(Work<WorkContext> workContext, IParticlesService particlesService)
        {
            _workContext = workContext;
            _particlesService = particlesService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadScripts")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    if (!_particlesService.GetAutoEnable()) return;
                    if (!_particlesService.GetAutoEnableAdmin())
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                    }

                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");


                    var currentHighlight = scripts
                            .Where(l => l.Name == "Particles")
                            .FirstOrDefault();

                    if (currentHighlight == null)
                    {
                        resourceManager.Require("script", "Particles").AtHead();
                    }                    
                });
        }
    }
}