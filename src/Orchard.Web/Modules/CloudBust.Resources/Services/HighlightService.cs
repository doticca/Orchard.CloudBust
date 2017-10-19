using Orchard;
using Orchard.Caching;
using CloudBust.Resources.Models;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Services
{
    public interface IHighlightService : IDependency
    {
        string GetStyle();
        bool GetAutoEnable();
        bool GetAutoEnableAdmin();
        bool GetFullBundle();
    }
    [OrchardFeature("CloudBust.Resources.Highlight")]
    public class HighlightService : IHighlightService
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        private const string ScriptsFolder = "scripts";

        public HighlightService(IWorkContextAccessor wca, ICacheManager cacheManager, ISignals signals)
        {
            _wca = wca;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public string GetStyle()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.Style",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var highlightSettings =
                        (HighlightSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(HighlightSettingsPart));
                    return highlightSettings.Style;
                });
        }
        public bool GetAutoEnable()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.AutoEnable",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var highlightSettings =
                        (HighlightSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(HighlightSettingsPart));
                    return highlightSettings.AutoEnable;
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
                    var highlightSettings =
                        (HighlightSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(HighlightSettingsPart));
                    return highlightSettings.AutoEnableAdmin;
                });
        }

        public bool GetFullBundle()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.FullBundle",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var highlightSettings =
                        (HighlightSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(HighlightSettingsPart));
                    return highlightSettings.FullBundle;
                });
        }

    }
}