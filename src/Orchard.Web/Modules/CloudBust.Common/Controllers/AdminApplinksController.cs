using System.Linq;
using System.Web.Mvc;
using Orchard;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using CloudBust.Common.Services;
using CloudBust.Common.ViewModels;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Controllers {
	[ValidateInput(false)]
    [Admin]

    [OrchardFeature("CloudBust.Common.SEO")]
    public class AdminApplinksController : Controller {
		private readonly IApplinksService _applinksService;

		public AdminApplinksController(IApplinksService applinksService, IOrchardServices orchardServices) {
            _applinksService = applinksService;
			Services = orchardServices;
			T = NullLocalizer.Instance;
		}

		public IOrchardServices Services { get; set; }
		public Localizer T { get; set; }

		public ActionResult Index() {
			if (!Authorized())
				return new HttpUnauthorizedResult();
			return View(new ApplinksFileViewModel() { Text = _applinksService.Get().FileContent });
		}

		[HttpPost]
		public ActionResult Index(ApplinksFileViewModel viewModel) {
			if (!Authorized())
				return new HttpUnauthorizedResult();
			var saveResult = _applinksService.Save(viewModel.Text);
			if (saveResult.Item1)
				Services.Notifier.Information(T("apple-app-site-association settings successfully saved"));
			else {
				Services.Notifier.Information(T("apple-app-site-association saved with warnings"));
				saveResult.Item2.ToList().ForEach(error =>
					Services.Notifier.Warning(T(error))
				);
			}
			return View(viewModel);
		}

		private bool Authorized() {
			return Services.Authorizer.Authorize(ApplinksPermissions.ConfigureApplinksTextFile, T("Cannot manage apple-app-site-association file"));
		}
	}
}