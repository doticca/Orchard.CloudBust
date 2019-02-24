using System;
using System.Collections.Generic;
using System.Linq;
using CloudBust.Blogs.Models;
using CloudBust.Blogs.Services;
using Orchard.Autoroute.Models;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace CloudBust.Blogs.Services {
    [OrchardFeature("CloudBust.Blogs.TaxonomiesCloud")]
    public class TaxonomyCloudService : ITaxonomyCloudService {
        private readonly ITaxonomyService _taxonomyService;
        private readonly IContentManager _contentManager;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        internal static readonly string TaxonomyCloudTermsChanged = "CloudBust.Blogs.TaxonomiesCloud.TermsChanged";
        internal static readonly string CacheKey = "CloudBust.Blogs.TaxonomiesCloud.Topics";

        public TaxonomyCloudService(
            IContentManager contentManager,
            ICacheManager cacheManager,
            ISignals signals, 
            ITaxonomyService taxonomyService) {
            _contentManager = contentManager;
            _cacheManager = cacheManager;
            _signals = signals;
            _taxonomyService = taxonomyService;
        }

        public IEnumerable<TermPart> GetTopicTerms() {           
            return _cacheManager.Get(CacheKey, true,
                ctx => {
                    ctx.Monitor(_signals.When(TaxonomyCloudTermsChanged));

                    var taxonomy = _taxonomyService.GetTaxonomyByName("Topic");
                    if (taxonomy == null) {
                        return null;
                    }

                    var topics = _taxonomyService.GetTerms(taxonomy.Id);
                    return topics;
                });
        }
    }
}