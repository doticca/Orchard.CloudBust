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
    public class AdminRobotsController : Controller {
		private readonly IRobotsService _robotsService;

		public AdminRobotsController(IRobotsService robotsService, IOrchardServices orchardServices) {
			_robotsService = robotsService;
			Services = orchardServices;
			T = NullLocalizer.Instance;
		}

		public IOrchardServices Services { get; set; }
		public Localizer T { get; set; }

		public ActionResult Index() {
			if (!Authorized())
				return new HttpUnauthorizedResult();
			return View(new RobotsFileViewModel() { Text = _robotsService.Get().FileContent });
		}

		[HttpPost]
		public ActionResult Index(RobotsFileViewModel viewModel) {
			if (!Authorized())
				return new HttpUnauthorizedResult();
			var saveResult = _robotsService.Save(viewModel.Text);
			if (saveResult.Item1)
				Services.Notifier.Information(T("Robots.txt settings successfully saved"));
			else {
				Services.Notifier.Information(T("Robots.txt saved with warnings"));
				saveResult.Item2.ToList().ForEach(error =>
					Services.Notifier.Warning(T(error))
				);
			}
			return View(viewModel);
		}

		private bool Authorized() {
			return Services.Authorizer.Authorize(RobotsPermissions.ConfigureRobotsTextFile, T("Cannot manage robots.txt file"));
		}
	}
}