using Orchard;
using Orchard.Caching;
using CloudBust.Application.Models;
using Orchard.Settings;
using Orchard.ContentManagement;
using System.Linq;

namespace CloudBust.Application.Services
{
    public interface ISettingsService : IDependency
    {
        string GetWebApplicationKey();
        ApplicationRecord GetWebApplication();
        bool IsWebApplication();
    }

    public class SettingsService : ISettingsService
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private readonly IApplicationsService _applicationsService;
        //private readonly IContentManager _contentManager;
        public SettingsService(
            IWorkContextAccessor wca, 
            ICacheManager cacheManager, 
            ISignals signals,
            IApplicationsService applicationsService
            //IContentManager contentManager
            )
        {
            _wca = wca;
            _cacheManager = cacheManager;
            _signals = signals;
            _applicationsService = applicationsService;
            //_contentManager = contentManager;
        }
        public string GetWebApplicationKey()
        {
            return _cacheManager.Get(
                "CloudBust.Application.ApplicationKey",
                ctx =>
                {
                    ctx.Monitor(_signals.When(CBSignals.SignalWebApp));
                    WorkContext workContext = _wca.GetContext();
                    var cloudbustSettings =
                        (ApplicationSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(ApplicationSettingsPart));
                    return cloudbustSettings.ApplicationKey;
                });
        }
        public ApplicationRecord GetWebApplication()
        {
            try
            {
                string appkey = GetWebApplicationKey();
                if (string.IsNullOrWhiteSpace(appkey)) return null;
                return _applicationsService.GetApplicationByKey(appkey);
            }
            catch
            {
                return null;
            }
        }
        public bool IsWebApplication()
        {
            return GetWebApplication() == null ? false : true;
        }        
    }
}