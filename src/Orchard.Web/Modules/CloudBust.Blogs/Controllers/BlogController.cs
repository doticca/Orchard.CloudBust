using System.Linq;
using System.Web.Mvc;
using Orchard.Blogs.Extensions;
using Orchard.Blogs.Services;
using Orchard.Core.Feeds;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Themes;
using Orchard.UI.Navigation;
using Orchard.Settings;
using Orchard.ContentManagement;
using Orchard.Blogs.Models;
using Orchard;
using Orchard.ContentManagement.Aspects;
using Orchard.Security;
using CloudBust.Application.Services;
using System.Collections.Generic;

namespace CloudBust.Blogs.Controllers
{

    //[Themed]
    //public class BlogController : Controller {
    //    private readonly IOrchardServices _services;
    //    private readonly IBlogService _blogService;
    //    private readonly IBlogPostService _blogPostService;
    //    private readonly IFeedManager _feedManager;
    //    private readonly ISiteService _siteService;
    //    private readonly IProfileService _profileService;
    //    private readonly ISettingsService _settingsService;

    //    public BlogController(
    //        IOrchardServices services, 
    //        IProfileService profileService,
    //        IBlogService blogService,
    //        IBlogPostService blogPostService,
    //        IFeedManager feedManager, 
    //        IShapeFactory shapeFactory,
    //        ISettingsService settingsService,
    //        ISiteService siteService) {
    //        _services = services;
    //        _blogService = blogService;
    //        _blogPostService = blogPostService;
    //        _feedManager = feedManager;
    //        _siteService = siteService;
    //        _profileService = profileService;
    //        _settingsService = settingsService;
    //        Logger = NullLogger.Instance;
    //        Shape = shapeFactory;
    //        T = NullLocalizer.Instance;
    //    }

    //    dynamic Shape { get; set; }
    //    protected ILogger Logger { get; set; }
    //    public Localizer T { get; set; }

    //    public ActionResult List() {
    //        IUser user = _services.WorkContext.CurrentUser;

    //        if (user == null)
    //        {
    //            return new HttpUnauthorizedResult();
    //        }
    //        var app = _settingsService.GetWebApplication();
    //        if (user == null)
    //        {
    //            return new HttpUnauthorizedResult();
    //        }
    //        if (!_profileService.IsUserInApplication(user, app)) return new HttpUnauthorizedResult();
    //        IEnumerable<dynamic> blogs;
    //        if (app.blogPerUser && app.blogSecurity)
    //        {
    //            blogs = _blogService.Get()
    //            .Where(b => (b.As<ICommonPart>().Owner == user) && (_services.Authorizer.Authorize(Orchard.Core.Contents.Permissions.ViewContent, b)))
    //            .Select(b => _services.ContentManager.BuildDisplay(b, "Summary"));
    //        }
    //        else
    //        {
    //            blogs = _blogService.Get()
    //            .Where(b => _services.Authorizer.Authorize(Orchard.Core.Contents.Permissions.ViewContent, b))
    //            .Select(b => _services.ContentManager.BuildDisplay(b, "Summary"));
    //        }
    //        var list = Shape.List();
    //        list.AddRange(blogs);

    //        var viewModel = Shape.ViewModel()
    //            .ContentItems(list);

    //        return View(viewModel);
    //    }

    //    public ActionResult Item(int blogId, PagerParameters pagerParameters) {
    //        IUser user = _services.WorkContext.CurrentUser;

    //        if (user == null)
    //        {
    //            return new HttpUnauthorizedResult();
    //        }
    //        var app = _settingsService.GetWebApplication();
    //        if (user == null)
    //        {
    //            return new HttpUnauthorizedResult();
    //        }
    //        if (!_profileService.IsUserInApplication(user, app)) return new HttpUnauthorizedResult();
    //        Pager pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

    //        var blogPart = _blogService.Get(blogId, VersionOptions.Published).As<BlogPart>();
    //        if (blogPart == null)
    //            return HttpNotFound();
    //        if(blogPart.As<ICommonPart>().Owner!=user)
    //            return HttpNotFound();
    //        if (!_services.Authorizer.Authorize(Orchard.Core.Contents.Permissions.ViewContent, blogPart, T("Cannot view content"))) {
    //            return new HttpUnauthorizedResult();
    //        }

    //        pager.PageSize = blogPart.PostsPerPage;

    //        _feedManager.Register(blogPart, _services.ContentManager.GetItemMetadata(blogPart).DisplayText);
    //        var blogPosts = _blogPostService.Get(blogPart, pager.GetStartIndex(), pager.PageSize)
    //            .Select(b => _services.ContentManager.BuildDisplay(b, "Summary"));
    //        dynamic blog = _services.ContentManager.BuildDisplay(blogPart);

    //        var list = Shape.List();
    //        list.AddRange(blogPosts);
    //        blog.Content.Add(Shape.Parts_Blogs_BlogPost_List(ContentItems: list), "5");

    //        var totalItemCount = _blogPostService.PostCount(blogPart);
    //        blog.Content.Add(Shape.Pager(pager).TotalItemCount(totalItemCount), "Content:after");

    //        return new ShapeResult(this, blog);
    //    }
    //}
}
