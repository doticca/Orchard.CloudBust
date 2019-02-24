using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CloudBust.Common.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Notify;
using Orchard.UI.Resources;

namespace CloudBust.Common.Drivers {
    [OrchardFeature("CloudBust.Common.SEO")]
    public class MetaTagsDriver : ContentPartDriver<MetaTagsPart> {
        private readonly IContentManager _contentManager;
        private readonly INotifier _notifier;
        private readonly IWorkContextAccessor _wca;

        public MetaTagsDriver(INotifier notifier, IWorkContextAccessor workContextAccessor,
            IContentManager contentManager) {
            _notifier = notifier;
            _wca = workContextAccessor;
            _contentManager = contentManager;
        }

        public Localizer T { get; set; }

        protected override string Prefix => "MetaTags";

        protected override DriverResult Display(MetaTagsPart part, string displayType, dynamic shapeHelper) {
            if (displayType != "Detail")
                return null;

            var context = _wca.GetContext();
            var resourceManager = context.Resolve<IResourceManager>();
            var metadata = _contentManager.GetItemMetadata(part);
            MetaTagsPart blogMetaTags = null;

            var siteMetaTags = (MetaTagsPart) context.CurrentSite.ContentItem.Parts.SingleOrDefault(x => x.GetType() == typeof(MetaTagsPart));

            if (metadata.DisplayRouteValues["blogId"] != null) {
                var blogId = metadata.DisplayRouteValues["blogId"].ToString();

                if (int.TryParse(blogId, NumberStyles.Number, CultureInfo.InvariantCulture, out var tmpId)) {
                    var blog = _contentManager.Get(tmpId);

                    blogMetaTags = blog.Parts.FirstOrDefault(x => x.GetType() == typeof(MetaTagsPart)) as MetaTagsPart;
                }
            }

            var descriptionTag = GetMetaDescription(siteMetaTags, blogMetaTags, part);
            var keywordsTag = GetMetaKeywords(siteMetaTags, blogMetaTags, part);

            if (!string.IsNullOrWhiteSpace(descriptionTag))
                resourceManager.SetMeta(new MetaEntry {
                    Name = "description",
                    Content = descriptionTag
                });

            if (!string.IsNullOrWhiteSpace(keywordsTag))
                resourceManager.SetMeta(new MetaEntry {
                    Name = "keywords",
                    Content = keywordsTag
                });

            var tag1Records = GetTag1Records(siteMetaTags, blogMetaTags, part);
            var tag2Records = GetTag2Records(siteMetaTags, blogMetaTags, part);

            var matchedKeys = tag1Records.Keys.Intersect(tag2Records.Keys);

            foreach (var matchedKey in matchedKeys) tag2Records.Remove(matchedKey);

            foreach (var record in tag1Records.Union(tag2Records))
                resourceManager.SetMeta(new MetaEntry {
                    Name = record.Key,
                    Content = record.Value
                });

            return null;
        }

        //GET
        protected override DriverResult Editor(MetaTagsPart part, dynamic shapeHelper) {
            if (part.Id == 0 && part.ContentItem.ContentType == "Site") {
                part.KeywordsInherited = true;
                part.DescriptionInherited = true;
            }

            var result = ContentShape("Parts_MetaTags_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/MetaTags.Edit",
                    Model: part,
                    Prefix: Prefix));

            if (part.ContentItem.ContentType == "Site") result = result.OnGroup("MetaTags");

            return result;
        }

        //POST
        protected override DriverResult Editor(MetaTagsPart part, IUpdateModel updater, dynamic shapeHelper) {
            if (updater.TryUpdateModel(part, Prefix, null, null))
                _notifier.Information(
                    T("Meta tags updated successfully"));
            else
                _notifier.Error(
                    T("Error during Meta tagss update!"));

            return Editor(part, shapeHelper);
        }

        protected override void Exporting(MetaTagsPart part, ExportContentContext context) {
            context.Element(part.PartDefinition.Name).SetAttributeValue("Keywords", part.Record.Keywords);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Description", part.Record.Description);
            context.Element(part.PartDefinition.Name).SetAttributeValue("MetaTag1Name", part.Record.MetaTag1Name);
            context.Element(part.PartDefinition.Name).SetAttributeValue("MetaTag1Value", part.Record.MetaTag1Value);
            context.Element(part.PartDefinition.Name).SetAttributeValue("MetaTag2Name", part.Record.MetaTag2Name);
            context.Element(part.PartDefinition.Name).SetAttributeValue("MetaTag2Value", part.Record.MetaTag2Value);
            context.Element(part.PartDefinition.Name).SetAttributeValue("MetaTag1Inherited", part.Record.MetaTag1Inherited);
            context.Element(part.PartDefinition.Name).SetAttributeValue("MetaTag2Inherited", part.Record.MetaTag2Inherited);
        }

        protected override void Importing(MetaTagsPart part, ImportContentContext context) {
            part.Record.Keywords = context.Attribute(part.PartDefinition.Name, "Keywords");
            part.Record.Description = context.Attribute(part.PartDefinition.Name, "Description");
            part.Record.MetaTag1Name = context.Attribute(part.PartDefinition.Name, "MetaTag1Name");
            part.Record.MetaTag1Value = context.Attribute(part.PartDefinition.Name, "MetaTag1Value");
            //part.Record.MetaTag1Inherited = context.Attribute(part.PartDefinition.Name, "MetaTag1Inherited");
            part.Record.MetaTag2Name = context.Attribute(part.PartDefinition.Name, "MetaTag2Name");
            part.Record.MetaTag2Value = context.Attribute(part.PartDefinition.Name, "MetaTag2Value");
            //part.Record.MetaTag2Inherited = context.Attribute(part.PartDefinition.Name, "MetaTag2Inherited");
        }


        private static string GetMetaKeywords(MetaTagsPart siteMetaTags, MetaTagsPart blogMetaTags, MetaTagsPart pageMetaTags) {
            var result = pageMetaTags?.Keywords;

            if (string.IsNullOrEmpty(result))
                if (blogMetaTags != null)
                    result = blogMetaTags.KeywordsInherited ? blogMetaTags.Keywords : null;

            if (!string.IsNullOrEmpty(result)) return result;
            if (siteMetaTags != null)
                result = siteMetaTags.KeywordsInherited ? siteMetaTags.Keywords : null;

            return result;
        }

        private static string GetMetaDescription(MetaTagsPart siteMetaTags, MetaTagsPart blogMetaTags, MetaTagsPart pageMetaTags) {
            var result = pageMetaTags?.Description;

            if (string.IsNullOrEmpty(result))
                if (blogMetaTags != null)
                    result = blogMetaTags.DescriptionInherited ? blogMetaTags.Description : null;

            if (!string.IsNullOrEmpty(result)) return result;

            if (siteMetaTags != null)
                result = siteMetaTags.DescriptionInherited ? siteMetaTags.Description : null;

            return result;
        }

        private static Dictionary<string, string> GetTag1Records(MetaTagsPart siteMetaTags, MetaTagsPart blogMetaTags, MetaTagsPart pageMetaTags) {
            var result = new Dictionary<string, string>();

            if (pageMetaTags != null && !string.IsNullOrEmpty(pageMetaTags.MetaTag1Name)) result.Add(pageMetaTags.MetaTag1Name, pageMetaTags.MetaTag1Value);

            if (blogMetaTags != null && !string.IsNullOrEmpty(blogMetaTags.MetaTag1Name)) {
                if (result.ContainsKey(blogMetaTags.MetaTag1Name)) {
                    if (string.IsNullOrEmpty(result[blogMetaTags.MetaTag1Name]) && blogMetaTags.MetaTag1Inherited) result[blogMetaTags.MetaTag1Name] = blogMetaTags.MetaTag1Value;
                }
                else {
                    result.Add(blogMetaTags.MetaTag1Name, blogMetaTags.MetaTag1Value);
                }
            }

            if (siteMetaTags == null || string.IsNullOrEmpty(siteMetaTags.MetaTag1Name)) return result;

            if (result.ContainsKey(siteMetaTags.MetaTag1Name)) {
                if (string.IsNullOrEmpty(result[siteMetaTags.MetaTag1Name]) && siteMetaTags.MetaTag1Inherited) result[siteMetaTags.MetaTag1Name] = siteMetaTags.MetaTag1Value;
            }
            else {
                result.Add(siteMetaTags.MetaTag1Name, siteMetaTags.MetaTag1Value);
            }

            return result;
        }

        private static Dictionary<string, string> GetTag2Records(MetaTagsPart siteMetaTags, MetaTagsPart blogMetaTags, MetaTagsPart pageMetaTags) {
            var result = new Dictionary<string, string>();

            if (pageMetaTags != null && !string.IsNullOrEmpty(pageMetaTags.MetaTag2Name)) result.Add(pageMetaTags.MetaTag2Name, pageMetaTags.MetaTag2Value);

            if (blogMetaTags != null && !string.IsNullOrEmpty(blogMetaTags.MetaTag2Name)) {
                if (result.ContainsKey(blogMetaTags.MetaTag2Name)) {
                    if (string.IsNullOrEmpty(result[blogMetaTags.MetaTag2Name]) && blogMetaTags.MetaTag2Inherited) result[blogMetaTags.MetaTag2Name] = blogMetaTags.MetaTag2Value;
                }
                else {
                    result.Add(blogMetaTags.MetaTag2Name, blogMetaTags.MetaTag2Value);
                }
            }

            if (siteMetaTags == null || string.IsNullOrEmpty(siteMetaTags.MetaTag2Name)) return result;

            if (result.ContainsKey(siteMetaTags.MetaTag2Name)) {
                if (string.IsNullOrEmpty(result[siteMetaTags.MetaTag2Name]) && siteMetaTags.MetaTag2Inherited) result[siteMetaTags.MetaTag2Name] = siteMetaTags.MetaTag2Value;
            }
            else {
                result.Add(siteMetaTags.MetaTag2Name, siteMetaTags.MetaTag2Value);
            }

            return result;
        }
    }
}