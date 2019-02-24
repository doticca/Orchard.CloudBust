using CloudBust.Blogs.Models;
using CloudBust.Blogs.Services;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs.Drivers {
    [OrchardFeature("CloudBust.Blogs.TaxonomiesCloud")]
    public class TaxonomyCloudPartDriver : ContentPartDriver<TaxonomyCloudPart> {
        private readonly ITaxonomyCloudService _taxonomyCloudService;

        public TaxonomyCloudPartDriver(ITaxonomyCloudService taxonomyCloudService)
        {
            _taxonomyCloudService = taxonomyCloudService;
        }

        protected override DriverResult Display(TaxonomyCloudPart part, string displayType, dynamic shapeHelper) {
            var terms = _taxonomyCloudService.GetTopicTerms();

            return ContentShape("Parts_TaxonomyCloud",
                () => shapeHelper.Parts_TaxonomyCloud(Terms: terms));
        }  
    }
}