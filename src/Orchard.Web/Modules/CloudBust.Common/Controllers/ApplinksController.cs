using System.Text;
using System.Web.Mvc;
using CloudBust.Common.Services;
using Orchard.Caching;
using Orchard.Environment.Extensions;
using System.IO;
using System;
using System.Web;
using Orchard.Security;

namespace CloudBust.Common.Controllers {

    [OrchardFeature("CloudBust.Common.SEO")]
    public class ApplinksController : Controller {
		private const string ContentType = "application/json";
        private const string FileName = "apple-app-site-association";

        private readonly IApplinksService _applinksService;
		private readonly ICacheManager _cacheManager;
		private readonly ISignals _signals;

		public ApplinksController(IApplinksService applinksService, ICacheManager cacheManager, ISignals signals) {
			_applinksService = applinksService;
			_cacheManager = cacheManager;
			_signals = signals;
		}

        [AlwaysAccessible]
        public ActionResult Index() {
			var content = _cacheManager.Get("ApplinksFile.Settings",
							  ctx => {
								  ctx.Monitor(_signals.When("ApplinksFile.SettingsChanged"));
								  var applinksFile = _applinksService.Get();
								  return applinksFile.FileContent;
							  });
			if (string.IsNullOrWhiteSpace(content)) {
				content = _applinksService.Get().FileContent;
			}

            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.AppendHeader("content-disposition", "inline; filename=" + FileName);


            MemoryStream memoryStream = new MemoryStream();
            TextWriter tw = new StreamWriter(memoryStream);

            tw.Write(content);
            tw.Flush();
            tw.Close();

            return File(memoryStream.ToArray(), ContentType);
        }
	}
}