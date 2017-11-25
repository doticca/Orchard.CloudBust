using Contrib.Voting.Services;
using CloudBust.Blogs.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs.Drivers
{
    [OrchardFeature("CloudBust.Blogs.Stats")]
    public class ViewersPartDriver : ContentPartDriver<ViewersPart>
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IContentManager _contentManager;
        private readonly IVotingService _votingService;

        public ViewersPartDriver(IOrchardServices orchardServices, IContentManager contentManager, IVotingService votingService)
        {
            _orchardServices = orchardServices;
            _contentManager = contentManager;
            _votingService = votingService;
        }

        protected override DriverResult Display(ViewersPart part, string displayType, dynamic shapeHelper)
        {
            if (!part.ShowVoter)
                return null;

            return Combined(
                ContentShape(
                    "Parts_Viewers",
                        () => shapeHelper.Parts_Viewers(part))
                );
        }
    }
}