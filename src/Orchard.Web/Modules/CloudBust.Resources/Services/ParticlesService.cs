using Orchard;
using Orchard.Caching;
using CloudBust.Resources.Models;
using Orchard.Environment.Extensions;
using System.Collections.Generic;
using Orchard.MediaLibrary.Services;
using System.Linq;
using System;

namespace CloudBust.Resources.Services
{
    public interface IParticlesService : IDependency
    {
        string GetJsonUrl();
        bool GetAutoEnable();
        bool GetAutoEnableAdmin();
        IEnumerable<string> GetJsonUrlSuggestions();
    }
    [OrchardFeature("CloudBust.Resources.Particles")]
    public class ParticlesService : IParticlesService
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private readonly IMediaLibraryService _mediaService;

        private const string ParticlesMediaFolder = "particles";
        private const string signalstring = "CloudBust.Resources.Particles.JsonUrl";


        public ParticlesService(IWorkContextAccessor wca, ICacheManager cacheManager, ISignals signals, IMediaLibraryService mediaService)
        {
            _wca = wca;
            _cacheManager = cacheManager;
            _signals = signals;
            _mediaService = mediaService;
        }

        public string GetJsonUrl()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.Particles.JsonUrl",
                ctx =>
                {
                    ctx.Monitor(_signals.When(signalstring));
                    WorkContext workContext = _wca.GetContext();
                    var particlesSettings =
                        (ParticlesSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(ParticlesSettingsPart));
                    return particlesSettings.JsonUrl;
                });
        }
        public bool GetAutoEnable()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.Particles.AutoEnable",
                ctx =>
                {
                    ctx.Monitor(_signals.When(signalstring));
                    WorkContext workContext = _wca.GetContext();
                    var particlesSettings =
                        (ParticlesSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(ParticlesSettingsPart));
                    return particlesSettings.AutoEnable;
                });
        }
        public bool GetAutoEnableAdmin()
        {
            return _cacheManager.Get(
                "CloudBust.Resources.Particles.AutoEnableAdmin",
                ctx =>
                {
                    ctx.Monitor(_signals.When(signalstring));
                    WorkContext workContext = _wca.GetContext();
                    var particlesSettings =
                        (ParticlesSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(ParticlesSettingsPart));
                    return particlesSettings.AutoEnableAdmin;
                });
        }    
        public IEnumerable<string> GetJsonUrlSuggestions()
        {
            List<string> jsonUrlSuggestions = null;
            var rootMediaFolders = _mediaService
                .GetMediaFolders(null)
                .Where(f => f.Name.Equals(ParticlesMediaFolder, StringComparison.OrdinalIgnoreCase));
            if (rootMediaFolders.Any())
            {
                jsonUrlSuggestions = new List<string>(
                    _mediaService.GetMediaFiles(ParticlesMediaFolder)
                        .Where(f => f.Type == ".json")
                        .Select(f => _mediaService.GetMediaPublicUrl(ParticlesMediaFolder, f.Name)));
            }
            return jsonUrlSuggestions;
        }

    }
}