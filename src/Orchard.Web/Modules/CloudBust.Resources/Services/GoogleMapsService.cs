using Orchard;
using Orchard.Caching;
using CloudBust.Resources.Models;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Services
{    
    public interface IGoogleMapsService : IDependency
    {
        string GetApiKey();
        string GetCallBack();
        string GetLanguage();
        bool GetSensor();
        bool GetAsync();
        bool GetDefer();
        bool GetAutoEnable();
        bool GetAutoEnableAdmin();
    }
    [OrchardFeature("CloudBust.Resources.GoogleMaps")]
    public class GoogleMapsService : IGoogleMapsService
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        private const string ScriptsFolder = "scripts";

        public GoogleMapsService(IWorkContextAccessor wca, ICacheManager cacheManager, ISignals signals)
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
                    var googlemapsSettings =
                        (GoogleMapsSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(GoogleMapsSettingsPart));
                    return googlemapsSettings.AutoEnable;
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
                    var googlemapsSettings =
                        (GoogleMapsSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(GoogleMapsSettingsPart));
                    return googlemapsSettings.AutoEnableAdmin;
                });
        }
        public bool GetSensor()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.Sensor",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var googlemapsSettings =
                        (GoogleMapsSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(GoogleMapsSettingsPart));
                    return googlemapsSettings.Sensor;
                });
        }
        public bool GetAsync()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.Async",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var googlemapsSettings =
                        (GoogleMapsSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(GoogleMapsSettingsPart));
                    return googlemapsSettings.Async;
                });
        }
        public bool GetDefer()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.Defer",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var googlemapsSettings =
                        (GoogleMapsSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(GoogleMapsSettingsPart));
                    return googlemapsSettings.Defer;
                });
        }

        public string GetApiKey()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.ApiKey",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var googlemapsSettings =
                        (GoogleMapsSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(GoogleMapsSettingsPart));
                    return googlemapsSettings.ApiKey;
                });
        }
        public string GetCallBack()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.CallBack",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var googlemapsSettings =
                        (GoogleMapsSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(GoogleMapsSettingsPart));
                    return googlemapsSettings.CallBack;
                });
        }
        public string GetLanguage()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.Language",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Resources.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var googlemapsSettings =
                        (GoogleMapsSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(GoogleMapsSettingsPart));
                    return googlemapsSettings.Language;
                });
        }

    }
}