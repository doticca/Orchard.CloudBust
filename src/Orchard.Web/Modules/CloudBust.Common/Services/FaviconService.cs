﻿using System;
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
                        .Select(f => _mediaService.GetMediaPublicUrl(FaviconMediaFolder, f.Name)));
            }
            return faviconSuggestions;
        }

    }
}