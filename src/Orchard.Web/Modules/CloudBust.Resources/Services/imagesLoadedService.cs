using CloudBust.Resources.Models;
using Orchard;
using Orchard.Caching;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Services {
    public interface IImagesLoadedService : IDependency {
        bool GetAutoEnable();
        bool GetAutoEnableAdmin();
    }

    [OrchardFeature("CloudBust.Resources.ImagesLoaded")]
    public class ImagesLoadedService : IImagesLoadedService {
        private const string ScriptsFolder = "scripts";
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private readonly IWorkContextAccessor _wca;

        public ImagesLoadedService(IWorkContextAccessor wca, ICacheManager cacheManager, ISignals signals) {
            _wca = wca;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public bool GetAutoEnable() {
            return _cacheManager.Get(
                "CloudBust.Resources.AutoEnable",
                ctx => {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    var workContext = _wca.GetContext();
                    var imagesLoadedSettings =
                        (ImagesLoadedSettingsPart) workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(ImagesLoadedSettingsPart));
                    return imagesLoadedSettings.AutoEnable;
                });
        }

        public bool GetAutoEnableAdmin() {
            return _cacheManager.Get(
                "CloudBust.Resources.AutoEnableAdmin",
                ctx => {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    var workContext = _wca.GetContext();
                    var imagesLoadedSettings =
                        (ImagesLoadedSettingsPart) workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(ImagesLoadedSettingsPart));
                    return imagesLoadedSettings.AutoEnableAdmin;
                });
        }
    }
}