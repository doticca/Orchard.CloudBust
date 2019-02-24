using System.Net;
using System.Web.Mvc;
using CloudBust.Common.Services;
using Orchard.Caching;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Controllers {
    [OrchardFeature("CloudBust.Common.SEO")]
    public class FaviconController : Controller {
        private const string ContentType = "image/x-icon";
        private readonly ICacheManager _cacheManager;

        private readonly IFaviconService _faviconService;
        private readonly ISignals _signals;

        public FaviconController(IFaviconService faviconService, ICacheManager cacheManager, ISignals signals) {
            _faviconService = faviconService;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public FileResult Index() {
            var fileurl = _cacheManager.Get("CloudBust.Common.Favicon.Url",
                ctx => {
                    ctx.Monitor(_signals.When("CloudBust.Common.Favicon.Changed"));
                    var faviconUrl = _faviconService.GetFaviconUrl();
                    return faviconUrl;
                });

            if (string.IsNullOrWhiteSpace(fileurl)) {
                return null;
            }

            if (fileurl.StartsWith("/")) {
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + 
                    Request.ApplicationPath.TrimEnd('/') + "/";
                fileurl = baseUrl + fileurl;
            }

            using (WebClient wc = new WebClient())
            {                   
                var byteArr = wc.DownloadData(fileurl);
                return File(byteArr, ContentType);
            }
        }
    }
}