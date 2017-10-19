using CloudBust.Resources.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Services;
using Orchard.Themes;
using Orchard.UI.Admin;
using System.Web.Mvc;

namespace CloudBust.Resources.Controllers
{
    [Admin, Themed(false)]
    [OrchardFeature("CloudBust.Resources.Slick")]
    public class SlickController : Controller
    {
        private readonly IMediaLibraryService _mediaLibraryService;
        private readonly IContentManager _contentManager;

        public SlickController(
            IMediaLibraryService mediaManagerService,
            IContentManager contentManager,
            IOrchardServices orchardServices)
        {
            _mediaLibraryService = mediaManagerService;
            _contentManager = contentManager;
            Services = orchardServices;
        }

        public IOrchardServices Services { get; set; }

        public ActionResult Index(string folderPath)
        {

            var viewModel = new SlickViewModel
            {
                FolderPath = folderPath,
                MediaFiles = _mediaLibraryService.GetMediaFiles(folderPath)
            };

            return View(viewModel);
        }
    }
}