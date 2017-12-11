using Orchard;
using Orchard.Caching;
using CloudBust.Foundation.Models;

namespace CloudBust.Foundation.Services
{
    public interface IFoundationService : IDependency
    {
        bool GetAutoEnableAdmin();
        bool GetDoNotEnableFrontEnd();
        bool GetUseSelect();
        bool GetUseDatePicker();
        bool GetUseIcons();
        bool GetUsePlaceholder();
        bool GetUseNicescroll();
        int GetGridStyle();
        string GetGridStyleText();
    }

    public class FoundationService : IFoundationService
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        private const string ScriptsFolder = "scripts";
        private const string StylesFolder = "styles";

        public FoundationService(
            IWorkContextAccessor wca, 
            ICacheManager cacheManager, 
            ISignals signals
            )
        {
            _wca = wca;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public bool GetAutoEnableAdmin()
        {
            return _cacheManager.Get(
                "CloudBust.Foundation.AutoEnableAdmin",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Foundation.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var foundationSettings = (FoundationSettingsPart)workContext.CurrentSite.ContentItem.Get(typeof(FoundationSettingsPart));
                    return foundationSettings.AutoEnableAdmin;
                });
        }
        public bool GetDoNotEnableFrontEnd()
        {
            return _cacheManager.Get(
                "CloudBust.Foundation.DoNotEnableFrontEnd",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Foundation.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var foundationSettings = (FoundationSettingsPart)workContext.CurrentSite.ContentItem.Get(typeof(FoundationSettingsPart));
                    return foundationSettings.DoNotEnableFrontEnd;
                });
        }
        public bool GetUseDatePicker()
        {
            return _cacheManager.Get(
                "CloudBust.Foundation.UseDatePicker",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Foundation.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var foundationSettings = (FoundationSettingsPart)workContext.CurrentSite.ContentItem.Get(typeof(FoundationSettingsPart));
                    return foundationSettings.UseDatePicker;
                });
        }
        public bool GetUseSelect()
        {
            return _cacheManager.Get(
                "CloudBust.Foundation.UseSelect",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Foundation.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var foundationSettings = (FoundationSettingsPart)workContext.CurrentSite.ContentItem.Get(typeof(FoundationSettingsPart));
                    return foundationSettings.UseSelect;
                });
        }
        public bool GetUseNicescroll()
        {
            return _cacheManager.Get(
                "CloudBust.Foundation.UseNicescroll",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Foundation.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var foundationSettings = (FoundationSettingsPart)workContext.CurrentSite.ContentItem.Get(typeof(FoundationSettingsPart));
                    return foundationSettings.UseNicescroll;
                });
        }
        public bool GetUsePlaceholder()
        {
            return _cacheManager.Get(
                "CloudBust.Foundation.UsePlaceholder",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Foundation.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var foundationSettings = (FoundationSettingsPart)workContext.CurrentSite.ContentItem.Get(typeof(FoundationSettingsPart));
                    return foundationSettings.UsePlaceholder;
                });
        }
        public bool GetUseIcons()
        {
            return _cacheManager.Get(
                "CloudBust.Foundation.UseIcons",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Foundation.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var foundationSettings = (FoundationSettingsPart)workContext.CurrentSite.ContentItem.Get(typeof(FoundationSettingsPart));
                    return foundationSettings.UseIcons;
                });
        }
        public string GetGridStyleText()
        {
            return _cacheManager.Get(
                "CloudBust.Foundation.GridStyleText",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Foundation.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var foundationSettings = (FoundationSettingsPart)workContext.CurrentSite.ContentItem.Get(typeof(FoundationSettingsPart));
                    return foundationSettings.GridStyleText;
                });
        }
        public int GetGridStyle()
        {
            return _cacheManager.Get(
                "CloudBust.Foundation.GridStyle",
                ctx =>
                {
                    ctx.Monitor(_signals.When("CloudBust.Foundation.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var foundationSettings = (FoundationSettingsPart)workContext.CurrentSite.ContentItem.Get(typeof(FoundationSettingsPart));
                    return foundationSettings.GridStyle;
                });
        }
    }
}