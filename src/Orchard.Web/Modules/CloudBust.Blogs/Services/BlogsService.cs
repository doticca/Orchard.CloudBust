using System.Collections.Generic;
using System.Linq;
using CloudBust.Application.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Logging;
using Orchard.Security;
using CloudBust.Application.Services;
using Orchard.Blogs.Models;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Title.Models;

namespace CloudBust.Blogs.Services
{
    public class BlogsService : IBlogsService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IContentManager _contentManager;
        private readonly IProfileService _profileService;
        private readonly IApplicationsService _applicationService;
        private readonly ISettingsService _settingsService;

        public BlogsService(
                                IContentManager contentManager
                                , IOrchardServices orchardServices
                                , IApplicationsService applicationService
                                , IProfileService profileService
                                , ISettingsService settingsService
            )
        {
            _orchardServices = orchardServices;
            _contentManager = contentManager;
            _applicationService = applicationService;
            _profileService = profileService;
            _settingsService = settingsService;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public IEnumerable<BlogPart> GetBlogsForUserInApplication(ApplicationRecord app, IUser user)
        {
            List<BlogPart> blogs = new List<BlogPart>();
            if (!_profileService.IsUserInApplication(user, app)) return blogs;
            if (app.blogPerUser && app.blogSecurity)
            {
                var bloglist = _orchardServices.ContentManager
                                       .Query<BlogPart>(VersionOptions.Latest, "Blog")
                                       .List()
                                       .Where(c => c.As<ICommonPart>().Owner == user);

                return bloglist;
            }
            else
            {
                var bloglist = _orchardServices.ContentManager
                       .Query<BlogPart>(VersionOptions.Latest, "Blog")
                       .List();

                return bloglist;
            }
        }
        public BlogPart CreateBlogForUserInApplication(ApplicationRecord app, IUser user)
        {
            var blog = _contentManager.New("Blog");
            blog.As<ICommonPart>().Owner = user;
            blog.As<TitlePart>().Title = user.UserName;
            blog.As<BlogPart>().Description = "Default Blog for " + user.UserName;

            _contentManager.Create(blog);
            return blog.As<BlogPart>();
        }
        public IEnumerable<BlogPart> GetBlogsForUserInApplication(IUser user)
        {
            List<BlogPart> blogs = new List<BlogPart>();
            var app = _settingsService.GetWebApplication();
            if (app == null)
                return blogs;

            
            blogs = GetBlogsForUserInApplication(app, user).ToList();
            if(blogs==null||blogs.Count()==0)
            {
                var blog = CreateBlogForUserInApplication(app, user);
                if(blogs==null)
                    blogs = new List<BlogPart>();
                blogs.Add(blog);
            }
            return blogs;
        }
    }
}