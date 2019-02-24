using System.Linq;
using CloudBust.Blogs.Models;
using CloudBust.Blogs.Services;
using Orchard.Caching;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Taxonomies.Models;

namespace CloudBust.Blogs.Handlers {
    [OrchardFeature("CloudBust.Blogs.TaxonomiesCloud")]
    public class TaxonomyCloudPartHandler : ContentHandler {
        private readonly ISignals _signals;

        public TaxonomyCloudPartHandler(ISignals signals) {
            _signals = signals;

            OnPublished<TermPart>((context, part) => InvalidateTaxonomyCloudCache());
            OnRemoved<TermPart>((context, part) => InvalidateTaxonomyCloudCache());
            OnUnpublished<TermPart>((context, part) => InvalidateTaxonomyCloudCache());
        }

        public void InvalidateTaxonomyCloudCache() {
            _signals.Trigger(TaxonomyCloudService.TaxonomyCloudTermsChanged);
        }
    }
}