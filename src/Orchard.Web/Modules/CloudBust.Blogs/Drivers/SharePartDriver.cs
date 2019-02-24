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