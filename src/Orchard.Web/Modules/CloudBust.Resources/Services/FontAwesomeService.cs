using Orchard;
using Orchard.Caching;
using CloudBust.Resources.Models;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Services
{    
    public interface IFontAwesomeService : IDependency
    {
        bool GetAutoEnable();
        bool GetAutoEnableAdmin();
    }
    [OrchardFeature("CloudBust.Resources.FontAwesome")]
    public class FontAwesomeService : IFontAwesomeService
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        private const string ScriptsFolder = "scripts";

        public FontAwesomeService(IWorkContextAccessor wca, ICacheManager cacheManager, ISignals signals)
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
                    var fontawesomeSettings =
                        (FontAwesomeSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(FontAwesomeSettingsPart));
                    return fontawesomeSettings.AutoEnable;
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
                    var fontawesomeSettings =
                        (FontAwesomeSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(FontAwesomeSettingsPart));
                    return fontawesomeSettings.AutoEnableAdmin;
                });
        }
    }
}