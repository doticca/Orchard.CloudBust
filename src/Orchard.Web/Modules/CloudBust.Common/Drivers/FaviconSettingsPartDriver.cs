using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using CloudBust.Common.Models;
using CloudBust.Common.Services;
using CloudBust.Common.ViewModels;

namespace CloudBust.Common.Drivers {
    [OrchardFeature("CloudBust.Common.FavIcon")]
    public class FaviconSettingsPartDriver : ContentPartDriver<FaviconSettingsPart> {
        private readonly ISignals _signals;
        private readonly IFaviconService _faviconService;

        public FaviconSettingsPartDriver(ISignals signals, IFaviconService faviconService) {
            _signals = signals;
            _faviconService = faviconService;
        }

        protected override string Prefix { get { return "FaviconSettings"; } }

        protected override DriverResult Editor(FaviconSettingsPart part, dynamic shapeHelper) {
            var faviconSuggestions = _faviconService.GetFaviconSuggestions();
            var appleTouchIconUrlSuggestions = _faviconService.GetAppleIconSuggestions();
            var androidManifestSuggestions = _faviconService.GetAndroidManifestSuggestions();
            var msApplicationConfigSuggestions = _faviconService.GetMSApplicationConfigSuggestions();
            var pngimageSuggestions = _faviconService.GetPngImageSuggestions();
            var safariSuggestions = _faviconService.GetSafariSuggestions();

            return ContentShape("Parts_Favicon_FaviconSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/Favicon.FaviconSettings",
                                   Model: new FaviconSettingsViewModel {
                                       FaviconUrl = part.FaviconUrl,
                                       FaviconUrlSuggestions = faviconSuggestions,
                                       AppleTouchIconUrlSuggestions = appleTouchIconUrlSuggestions,
                                       AppleTouchIconUrl = part.AppleTouchIconUrl,
                                       AndroidManifestUrl = part.AndroidManifestUrl,
                                       AndroidManifestSuggestions = androidManifestSuggestions,
                                       SafariPinnedUrl = part.SafariPinnedUrl,
                                       SafariPinnedMask = part.SafariPinnedMask,
                                       SafariSuggestions = safariSuggestions,
                                       PngImageUrl = part.PngImageUrl,
                                       PngImageSuggestions = pngimageSuggestions,
                                       MSApplicationConfigUrl = part.MSApplicationConfigUrl,
                                       MSApplicationConfigSuggestions = msApplicationConfigSuggestions,
                                       ThemeColor = part.ThemeColor
                                   },
                                   Prefix: Prefix)).OnGroup("Favicon");
        }

        protected override DriverResult Editor(FaviconSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            _signals.Trigger("CloudBust.Common.Favicon.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(FaviconSettingsPart part, ExportContentContext context) {
            context.Element(part.PartDefinition.Name).SetAttributeValue("FaviconUrl", part.Record.FaviconUrl);
            context.Element(part.PartDefinition.Name).SetAttributeValue("AppleTouchIconUrl", part.Record.AppleTouchIconUrl);
            context.Element(part.PartDefinition.Name).SetAttributeValue("PngImageUrl", part.Record.PngImageUrl);
            context.Element(part.PartDefinition.Name).SetAttributeValue("AndroidManifestUrl", part.Record.AndroidManifestUrl);
            context.Element(part.PartDefinition.Name).SetAttributeValue("SafariPinnedUrl", part.Record.SafariPinnedUrl);
            context.Element(part.PartDefinition.Name).SetAttributeValue("SafariPinnedMask", part.Record.SafariPinnedMask);
            context.Element(part.PartDefinition.Name).SetAttributeValue("MSApplicationConfigUrl", part.Record.MSApplicationConfigUrl);
            context.Element(part.PartDefinition.Name).SetAttributeValue("ThemeColor", part.Record.ThemeColor);
        }

        protected override void Importing(FaviconSettingsPart part, ImportContentContext context) {
            part.Record.FaviconUrl = context.Attribute(part.PartDefinition.Name, "FaviconUrl");
            part.Record.AppleTouchIconUrl = context.Attribute(part.PartDefinition.Name, "AppleTouchIconUrl");
            part.Record.PngImageUrl = context.Attribute(part.PartDefinition.Name, "PngImageUrl");
            part.Record.AndroidManifestUrl = context.Attribute(part.PartDefinition.Name, "AndroidManifestUrl");
            part.Record.SafariPinnedUrl = context.Attribute(part.PartDefinition.Name, "SafariPinnedUrl");
            part.Record.SafariPinnedMask = context.Attribute(part.PartDefinition.Name, "SafariPinnedMask");
            part.Record.MSApplicationConfigUrl = context.Attribute(part.PartDefinition.Name, "MSApplicationConfigUrl");
            part.Record.ThemeColor = context.Attribute(part.PartDefinition.Name, "ThemeColor");
        }
    }
}