using System.Collections.Generic;
using System.Linq;
using CloudBust.Application.Models;
using CloudBust.Application.Services;
using Orchard;
using Orchard.Blogs.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Title.Models;
using Orchard.Logging;
using Orchard.Security;

namespace CloudBust.Blogs.Services {
    public class BlogsService : IBlogsService {
        private readonly IContentManager _contentManager;
        private readonly IOrchardServices _orchardServices;
        private readonly IProfileService _profileService;
        private readonly ISettingsService _settingsService;

        public BlogsService(
            IContentManager contentManager
          , IOrchardServices orchardServices
          , IProfileService profileService
          , ISettingsService settingsService
        ) {
            _orchardServices = orchardServices;
            _contentManager = contentManager;
            _profileService = profileService;
            _settingsService = settingsService;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public IEnumerable<BlogPart> GetBlogsForUserInApplication(ApplicationRecord app, IUser user) {
            if (!_profileService.IsUserInApplication(user, app)) return new List<BlogPart>();

            if (app.blogPerUser && app.blogSecurity) {
                var bloglist = _orchardServices.ContentManager
                                               .Query<BlogPart>(VersionOptions.Latest, "Blog")
                                               .List()
                                               .Where(c => c.As<ICommonPart>().Owner == user);

                return bloglist;
            }
            else {
                var bloglist = _orchardServices.ContentManager
                                               .Query<BlogPart>(VersionOptions.Latest, "Blog")
                                               .List();

                return bloglist;
            }
        }

        public IEnumerable<BlogPart> GetBlogsForUserInApplication(IUser user) {
            var blogs = new List<BlogPart>();
            var app = _settingsService.GetWebApplication();
            if (app == null)
                return blogs;


            blogs = GetBlogsForUserInApplication(app, user).ToList();
            if (blogs.Any()) return blogs;

            var blog = CreateBlogForUserInApplication(app, user);
            blogs.Add(blog);

            return blogs;
        }

        public BlogPart CreateBlogForUserInApplication(ApplicationRecord app, IUser user) {
            var blog = _contentManager.New<BlogPart>("Blog");
            blog.As<ICommonPart>().Owner = user;
            blog.As<TitlePart>().Title = user.UserName;
            blog.As<BlogPart>().Description = "Default Blog for " + user.UserName;            

            _contentManager.Create(blog, VersionOptions.Draft);
            _contentManager.Publish(blog.ContentItem);

           return blog.As<BlogPart>();
        }
    }
}