using System.Linq;
using System.Web.Mvc;
using CloudBust.LogViewer.CustomUtils;
using CloudBust.LogViewer.Services;
using CloudBust.LogViewer.ViewModels;
using Orchard;
using Orchard.Localization;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace CloudBust.LogViewer.Controllers {
    [Themed]
    public class DashboardController : Controller {
        private readonly ILogViewerService _logViewerService;
        private readonly IOrchardServices _orchardServices;

        public DashboardController(
            IOrchardServices orchardServices,
            ILogViewerService logViewerService) {
            _orchardServices = orchardServices;
            _logViewerService = logViewerService;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult Index() {
            var logFiles = _logViewerService.GetLogFiles();

            var vm = new LogFilesVm {
                Files = logFiles,
                FileName = logFiles.FirstOrDefault(),
                FileContent = _logViewerService.ReadLogFile(logFiles.FirstOrDefault())
            };

            return View(vm);
        }

        public JsonResult Load(string file) {
            return Json(_logViewerService.ReadLogFile(file), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string file) {
            if (_logViewerService.DeleteLogFile(file))
                _orchardServices.Notifier.Information(T($"File '{file}' was deleted."));

            return RedirectToAction("Index");
        }

        public ActionResult Save(string file) {
            var stream = _logViewerService.GetFileStream(file);
            if (stream == null)
                return RedirectToAction("Index");

            return File(stream, LogConstants.LogFileContentType, file);
        }
    }
}