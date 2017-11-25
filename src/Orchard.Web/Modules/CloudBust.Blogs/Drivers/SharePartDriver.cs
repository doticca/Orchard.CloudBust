using Contrib.Voting.Services;
using CloudBust.Blogs.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs.Drivers
{
    public class SharePartDriver : ContentPartDriver<SharePart>
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IContentManager _contentManager;

        public SharePartDriver(IOrchardServices orchardServices, IContentManager contentManager)
        {
            _orchardServices = orchardServices;
            _contentManager = contentManager;
        }

        protected override DriverResult Display(SharePart part, string displayType, dynamic shapeHelper)
        {
            if (!part.ShowFacebook && !part.ShowMail && !part.ShowTwitter)
                return null;

            return Combined(
                ContentShape(
                    "Parts_Share",
                        () => shapeHelper.Parts_Share(part))
                );
        }
    }
}