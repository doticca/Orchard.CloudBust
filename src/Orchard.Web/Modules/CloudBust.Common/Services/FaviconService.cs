using System;
using System.Collections.Generic;
using System.Linq;
using Orchard;
using Orchard.Caching;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Services;
using CloudBust.Common.Models;

namespace CloudBust.Common.Services {
    public interface IFaviconService : IDependency {
        string GetFaviconUrl();
        IEnumerable<string> GetFaviconSuggestions();
        string GetAppleTouchIconUrl();
        IEnumerable<string> GetAppleIconSuggestions();
        string GetPngImageUrl();
        IEnumerable<string> GetPngImageSuggestions();
        string GetAndroidManifestUrl();
        IEnumerable<string> GetAndroidManifestSuggestions();
        string GetSafariPinnedUrl();
        string GetSafariPinnedMask();
        IEnumerable<string> GetSafariSuggestions();
        string GetMSApplicationConfigUrl();
        IEnumerable<string> GetMSApplicationConfigSuggestions();
        string GetThemeColor();
    }

    public class FaviconService : IFaviconService {
        private readonly IWorkContextAccessor _wca;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private readonly IMediaLibraryService _mediaService;

        private const string FaviconMediaFolder = "favicon";

        public FaviconService(IWorkContextAccessor wca, ICacheManager cacheManager, ISignals signals, IMediaLibraryService mediaService)
        {
            _wca = wca;
            _cacheManager = cacheManager;
            _signals = signals;
            _mediaService = mediaService;
        }

        public string GetFaviconUrl() {
            return _cacheManager.Get(
                "CloudBust.Common.Favicon.Url",
                ctx => {
                    ctx.Monitor(_signals.When("CloudBust.Common.Favicon.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var faviconSettings =
                        (FaviconSettingsPart) workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof (FaviconSettingsPart));
                    return faviconSettings.FaviconUrl;
                });
        }
        public IEnumerable<string> GetFaviconSuggestions() {
            List<string> faviconSuggestions = null;
            var rootMediaFolders = _mediaService
                .GetMediaFolders(null)
                .Where(f => f.Name.Equals(FaviconMediaFolder, StringComparison.OrdinalIgnoreCase));
            if (rootMediaFolders.Any()) {
                faviconSuggestions = new List<string>(
                    _mediaService.GetMediaFiles(FaviconMediaFolder)
                        .Where(f => f.Type == ".ico")
                        .Select(f => _mediaService.GetMediaPublicUrl(FaviconMediaFolder, f.Name)));
            }
            return faviconSuggestions;
        }

        public string GetAppleTouchIconUrl()
        {
            return _cacheManager.Get(
                "CloudBust.Common.Favicon.AppleTouchIconUrl",
                ctx => {
                    ctx.Monitor(_signals.When("CloudBust.Common.Favicon.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var faviconSettings =
                        (FaviconSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(FaviconSettingsPart));
                    return faviconSettings.AppleTouchIconUrl;
                });
        }
        public IEnumerable<string> GetAppleIconSuggestions()
        {
            List<string> appleiconSuggestions = null;
            var rootMediaFolders = _mediaService
                .GetMediaFolders(null)
                .Where(f => f.Name.Equals(FaviconMediaFolder, StringComparison.OrdinalIgnoreCase));
            if (rootMediaFolders.Any())
            {
                appleiconSuggestions = new List<string>(
                    _mediaService.GetMediaFiles(FaviconMediaFolder)
                        .Where(f => f.Type == ".png" && f.Name.StartsWith("apple"))
                        .Select(f => _mediaService.GetMediaPublicUrl(FaviconMediaFolder, f.Name)));
            }
            return appleiconSuggestions;
        }

        public string GetPngImageUrl()
        {
            return _cacheManager.Get(
                "CloudBust.Common.Favicon.PngImageUrl",
                ctx => {
                    ctx.Monitor(_signals.When("CloudBust.Common.Favicon.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var faviconSettings =
                        (FaviconSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(FaviconSettingsPart));
                    return faviconSettings.PngImageUrl;
                });
        }
        public IEnumerable<string> GetPngImageSuggestions()
        {
            List<string> pngimageSuggestions = null;
            var rootMediaFolders = _mediaService
                .GetMediaFolders(null)
                .Where(f => f.Name.Equals(FaviconMediaFolder, StringComparison.OrdinalIgnoreCase));
            if (rootMediaFolders.Any())
            {
                pngimageSuggestions = new List<string>(
                    _mediaService.GetMediaFiles(FaviconMediaFolder)
                        .Where(f => f.Type == ".png")
                        .Select(f => _mediaService.GetMediaPublicUrl(FaviconMediaFolder, f.Name)));
            }
            return pngimageSuggestions;
        }
        public string GetAndroidManifestUrl()
        {
            return _cacheManager.Get(
                "CloudBust.Common.Favicon.AndroidManifestUrl",
                ctx => {
                    ctx.Monitor(_signals.When("CloudBust.Common.Favicon.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var faviconSettings =
                        (FaviconSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(FaviconSettingsPart));
                    return faviconSettings.AndroidManifestUrl;
                });
        }
        public IEnumerable<string> GetAndroidManifestSuggestions()
        {
            List<string> androidmanifestSuggestions = null;
            var rootMediaFolders = _mediaService
                .GetMediaFolders(null)
                .Where(f => f.Name.Equals(FaviconMediaFolder, StringComparison.OrdinalIgnoreCase));
            if (rootMediaFolders.Any())
            {
                androidmanifestSuggestions = new List<string>(
                    _mediaService.GetMediaFiles(FaviconMediaFolder)
                        .Where(f => f.Type == ".json" && f.Name.StartsWith("manifest"))
                        .Select(f => _mediaService.GetMediaPublicUrl(FaviconMediaFolder, f.Name)));
            }
            return androidmanifestSuggestions;
        }
        public string GetSafariPinnedUrl()
        {
            return _cacheManager.Get(
                "CloudBust.Common.Favicon.SafariPinnedUrl",
                ctx => {
                    ctx.Monitor(_signals.When("CloudBust.Common.Favicon.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var faviconSettings =
                        (FaviconSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(FaviconSettingsPart));
                    return faviconSettings.SafariPinnedUrl;
                });
        }
        public string GetSafariPinnedMask()
        {
            return _cacheManager.Get(
                "CloudBust.Common.Favicon.SafariPinnedMask",
                ctx => {
                    ctx.Monitor(_signals.When("CloudBust.Common.Favicon.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var faviconSettings =
                        (FaviconSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(FaviconSettingsPart));
                    return faviconSettings.SafariPinnedMask;
                });
        }
        public IEnumerable<string> GetSafariSuggestions()
        {
            List<string> safariSuggestions = null;
            var rootMediaFolders = _mediaService
                .GetMediaFolders(null)
                .Where(f => f.Name.Equals(FaviconMediaFolder, StringComparison.OrdinalIgnoreCase));
            if (rootMediaFolders.Any())
            {
                safariSuggestions = new List<string>(
                    _mediaService.GetMediaFiles(FaviconMediaFolder)
                        .Where(f => f.Type == ".svg")
                        .Select(f => _mediaService.GetMediaPublicUrl(FaviconMediaFolder, f.Name)));
            }
            return safariSuggestions;
        }
        public string GetMSApplicationConfigUrl()
        {
            return _cacheManager.Get(
                "CloudBust.Common.Favicon.MSApplicationConfigUrl",
                ctx => {
                    ctx.Monitor(_signals.When("CloudBust.Common.Favicon.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var faviconSettings =
                        (FaviconSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(FaviconSettingsPart));
                    return faviconSettings.MSApplicationConfigUrl;
                });
        }
        public IEnumerable<string> GetMSApplicationConfigSuggestions()
        {
            List<string> msapplicationconfigSuggestions = null;
            var rootMediaFolders = _mediaService
                .GetMediaFolders(null)
                .Where(f => f.Name.Equals(FaviconMediaFolder, StringComparison.OrdinalIgnoreCase));
            if (rootMediaFolders.Any())
            {
                msapplicationconfigSuggestions = new List<string>(
                    _mediaService.GetMediaFiles(FaviconMediaFolder)
                        .Where(f => f.Type == ".xml" && f.Name.StartsWith("browser"))
                        .Select(f => _mediaService.GetMediaPublicUrl(FaviconMediaFolder, f.Name)));
            }
            return msapplicationconfigSuggestions;
        }
        public string GetThemeColor()
        {
            return _cacheManager.Get(
                "CloudBust.Common.Favicon.ThemeColor",
                ctx => {
                    ctx.Monitor(_signals.When("CloudBust.Common.Favicon.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var faviconSettings =
                        (FaviconSettingsPart)workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof(FaviconSettingsPart));
                    return faviconSettings.ThemeColor;
                });
        }
    }
}