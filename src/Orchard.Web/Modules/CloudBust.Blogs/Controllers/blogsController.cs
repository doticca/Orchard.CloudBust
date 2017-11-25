using CloudBust.Blogs.OData;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Security;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Orchard.Blogs.Services;
using Orchard.Blogs.Models;
using System.Web.Routing;
using CloudBust.Blogs.Models;
using CloudBust.Common.Extensions;
using CloudBust.Application.Models;
using CloudBust.Application.Services;
using CloudBust.Blogs.Services;

namespace CloudBust.Blogs.Controllers
{
    public class blogsController: ApiController
    {
        private readonly IBlogService _blogService;
        private readonly IBlogPostService _blogPostService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IOrchardServices _orchardServices;
        private readonly IMembershipService _membershipService;
        private readonly RouteCollection _routeCollection;
        private readonly IApplicationsService _applicationsService;
        private readonly IBlogsService _blogsService;
        private readonly ILoginsService _loginsService;
        private readonly ISettingsService _settingsService;

        public Localizer T { get; set; }
        public blogsController(
            IBlogService blogService,
            IBlogPostService blogPostService,
            IAuthorizationService authorizationService,
            IOrchardServices orchardServices,
            RouteCollection routeCollection,
            IMembershipService membershipService,
            IApplicationsService applicationsService,
            IBlogsService blogsService,
            ILoginsService loginsService,
            ISettingsService settingsService
            )
        {
            _blogService = blogService;
            _blogPostService = blogPostService;
            _authorizationService = authorizationService;
            _membershipService = membershipService;
            _orchardServices = orchardServices;
            _routeCollection = routeCollection;
            _applicationsService = applicationsService;
            _blogsService = blogsService;
            _loginsService = loginsService;
            _settingsService = settingsService;

            T = NullLocalizer.Instance;
        }
        [ActionName("Info")]
        public HttpResponseMessage GetInfo(string blogname = null)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }

            foreach (BlogPart blog in _blogService.Get())
            {
                // User needs to at least have permission to edit its own blog posts to access the service
                if (blog.Name == blogname)
                {

                    BlogPart blogPart = blog;
                    var urlHelper = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext, _routeCollection);
                    CloudBust.Blogs.OData.Blog bloginfo = new CloudBust.Blogs.OData.Blog(blogPart, Request, urlHelper);
                    return Request.CreateResponse(HttpStatusCode.OK, bloginfo);
                }
            }


            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        [ActionName("Blogs")]
        [ExtendedQueryable]
        public IQueryable<Blog> GetBlogs()
        {
            IUser user = CheckUser();
            if (user == null)
            {
                return null;
            }
            var app = _settingsService.GetWebApplication();

            var urlHelper = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext, _routeCollection);
            var b = _blogsService.GetBlogsForUserInApplication(app, user);
            return b.Select(c => new Blog(c, Request, urlHelper))
                                   .AsQueryable();


            //return _orchardServices.ContentManager
            //                       .Query<BlogPart>(VersionOptions.Published, "Blog")
            //                       .List()
            //                       .Select(c => new Blog(c, Request, urlHelper))
            //                       .AsQueryable();
        }

        [ActionName("Posts")]
        [ExtendedQueryable]
        public IQueryable<BlogPost> GetPosts(string blogname = null)
        {
            if (string.IsNullOrWhiteSpace(blogname))
                return null;

            IUser user = _orchardServices.WorkContext.CurrentUser;
            if (user == null)
            {
                return null;
            }
            BlogPart blogPart = null;
            foreach (BlogPart blog in _blogService.Get())
            {
                if (blog.Name == blogname)
                {
                    blogPart = blog;
                }
            }
            if (blogPart == null)
                return null;



            var urlHelper = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext, _routeCollection);
            return _blogPostService.Get(blogPart)
                                             .Select(m => new BlogPost(m, Request, urlHelper))
                                             .AsQueryable();
        }

        [ActionName("Posts")]
        [HttpGet]
        public HttpResponseMessage GetViews(int id)
        {
            int requestobject = 0;  // this flag is for direct calls to do an error handling
            if (Request.RequestUri.AbsolutePath.ToLower().Contains("/v1/blogpost"))
                requestobject = 1;
            if (requestobject != 1)
                return Request.CreateResponse(HttpStatusCode.Conflict, "this is a blogpost property only.");
            var Part = Query(id, VersionOptions.Latest);
            if (Part == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            switch (Part.ContentType)
            {
                case "BlogPost":
                    Viewers viewers = new Viewers(Part.As<ViewersPart>());
                    return Request.CreateResponse(HttpStatusCode.OK, viewers);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        #region utilities

        private IUser CheckUser()
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;
            if (user != null)
            {
                int aid = _loginsService.GetSessionAppId(user);
                ApplicationRecord app = _applicationsService.GetApplication(aid);
                if (app != null && app.AppKey == _settingsService.GetWebApplicationKey())
                {
                    return user;
                }
            }
            return null;
        }
        private ContentItem Query(int id, VersionOptions options)
        {
            return _orchardServices.ContentManager.Get(id, options);
        }
        #endregion
    }
}