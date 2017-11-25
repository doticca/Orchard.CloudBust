using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Security;
using Orchard.Security.Permissions;
using Orchard.Environment.Extensions;
using Orchard;
using CloudBust.Application.Services;

namespace CloudBust.Application.Security
{

    [OrchardFeature("CloudBust.Application.WebApp")]
    [OrchardSuppressDependency("Orchard.Blogs.Security.BlogAuthorizationEventHandler")]
    public class BlogAuthorizationEventHandler : IAuthorizationServiceEventHandler {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly ISettingsService _settingsService;
        private readonly IProfileService _profileService;
        public BlogAuthorizationEventHandler(
            IWorkContextAccessor workContextAccessor,
            IProfileService profileService,
            ISettingsService settingsService)
        {
            _workContextAccessor = workContextAccessor;
            _settingsService = settingsService;
            _profileService = profileService;
        }
        public void Checking(CheckAccessContext context) { }
        public void Complete(CheckAccessContext context)
        {

            var app = _settingsService.GetWebApplication();
            if (app == null)
                return;
            if (!app.blogPerUser || !app.blogSecurity)
                return;

            if (context.Content == null)
            {
                return;
            }
            else
            {
                if(context.Content.ContentItem.ContentType == "BlogPost" || context.Content.ContentItem.ContentType == "Blog")
                {

                    // we should add this at the end, to ensure admins can see all content

                    //if (context.User != null)
                    //{
                    //    var superuser = _workContextAccessor.GetContext().CurrentSite.SuperUser;
                    //    if (!string.IsNullOrEmpty(superuser) && string.Equals(context.User.UserName, superuser, StringComparison.Ordinal))
                    //    {
                    //        context.Granted = true;
                    //        return;
                    //    }
                    //}
                    if (context.User == null)
                    {
                        context.Granted = false;
                    }
                    else
                    {
                        // security checks to be moved in a service
                        if (_profileService.IsUserInApplication(context.User, app))
                        {
                            var commonPart = context.Content.As<ICommonPart>();
                            if (commonPart != null) {
                                // basics single user
                                if (commonPart.Owner == context.User)
                                {
                                    context.Granted = true;
                                    return;
                                }
                                //context.Granted = true;
                                //return;
                            }
                        }
                        context.Granted = false;
                    }


                }
            }
        }

        public void Adjust(CheckAccessContext context) {
            if (!context.Granted &&
                context.Content.Is<ICommonPart>()) {

                if (context.Permission.Name == Orchard.Core.Contents.Permissions.PublishContent.Name && context.Content.ContentItem.ContentType == "BlogPost") {
                    context.Adjusted = true;
                    context.Permission = Orchard.Blogs.Permissions.PublishBlogPost;
                }
                else if (OwnerVariationExists(context.Permission) &&
                    HasOwnership(context.User, context.Content)) {
                    context.Adjusted = true;
                    context.Permission = GetOwnerVariation(context.Permission);
                }
            }
        }

        private static bool HasOwnership(IUser user, IContent content) {
            if (user == null || content == null)
                return false;

            if (HasOwnershipOnContainer(user, content)) {
                return true;
            }

            var common = content.As<ICommonPart>();
            if (common == null || common.Owner == null)
                return false;

            return user.Id == common.Owner.Id;
        }

        private static bool HasOwnershipOnContainer(IUser user, IContent content) {
            if (user == null || content == null)
                return false;

            var common = content.As<ICommonPart>();
            if (common == null || common.Container == null)
                return false;

            common = common.Container.As<ICommonPart>();
            if (common == null || common.Container == null)
                return false;

            return user.Id == common.Owner.Id;
        }

        private static bool OwnerVariationExists(Permission permission) {
            return GetOwnerVariation(permission) != null;
        }

        private static Permission GetOwnerVariation(Permission permission) {
            if (permission.Name == Orchard.Blogs.Permissions.PublishBlogPost.Name)
                return Orchard.Blogs.Permissions.PublishOwnBlogPost;
            if (permission.Name == Orchard.Blogs.Permissions.EditBlogPost.Name)
                return Orchard.Blogs.Permissions.EditOwnBlogPost;
            if (permission.Name == Orchard.Blogs.Permissions.DeleteBlogPost.Name)
                return Orchard.Blogs.Permissions.DeleteOwnBlogPost;
            if (permission.Name == Orchard.Core.Contents.Permissions.ViewContent.Name)
                return Orchard.Core.Contents.Permissions.ViewOwnContent;
            if (permission.Name == Orchard.Blogs.Permissions.MetaListBlogs.Name)
                return Orchard.Blogs.Permissions.MetaListOwnBlogs;

            return null;
        }
    }
}