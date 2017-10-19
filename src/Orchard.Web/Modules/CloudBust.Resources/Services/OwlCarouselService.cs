using Orchard;
using Orchard.Caching;
using CloudBust.Resources.Models;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Services
{
    public interface IOwlCarouselService : IDependency
    {
        bool GetAutoEnable();
        bool GetAutoEnableAdmin();
    }
    [OrchardFeature("CloudBust.Resources.OwlCarousel")]
    public class OwlCarouselService : IOwlCarouselService
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        private const string ScriptsFolder = "scripts";

        public OwlCarouselService(IWorkContextAccessor wca, ICacheManager cacheManager, ISignals signals)
        {
            _wca = wca;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public bool GetAutoEnable()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.AutoEnable",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var owlcarouselSettings =
                        (OwlCarouselSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(OwlCarouselSettingsPart));
                    return owlcarouselSettings.AutoEnable;
                });
        }

        public bool GetAutoEnableAdmin()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.AutoEnableAdmin",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var owlcarouselSettings =
                        (OwlCarouselSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(OwlCarouselSettingsPart));
                    return owlcarouselSettings.AutoEnableAdmin;
                });
        }

    }
}