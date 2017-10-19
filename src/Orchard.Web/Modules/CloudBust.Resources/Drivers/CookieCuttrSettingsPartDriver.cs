using CloudBust.Resources.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using Orchard.Environment.Extensions;
using System;

namespace CloudBust.Resources.Drivers
{
    [OrchardFeature("CloudBust.Resources.CookieCuttr")]
    public class CookieCuttrSettingsPartDriver : ContentPartDriver<CookieCuttrSettingsPart>
    {
        public CookieCuttrSettingsPartDriver()
        {
            T = NullLocalizer.Instance;
        }
        public Localizer T { get; set; }
        protected override string Prefix { get { return "CookieCuttrSettings"; } }
        protected override DriverResult Editor(CookieCuttrSettingsPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(CookieCuttrSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_CookieCuttr_CookieCuttrSettings", () =>
            {
                if (updater != null)
                {
                    updater.TryUpdateModel(part.Record, Prefix, null, null);
                }
                return shapeHelper.EditorTemplate(TemplateName: "Parts/CookieCuttr.CookieCuttrSettings", Model: part.Record, Prefix: Prefix);
            })
                .OnGroup("cookies");
        }

        protected override void Exporting(CookieCuttrSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("cookieDiscreetLinkText", part.cookieDiscreetLinkText);
            element.SetAttributeValue("cookieDiscreetPosition", part.cookieDiscreetPosition);
            element.SetAttributeValue("cookieDomain", part.cookieDomain);
            element.SetAttributeValue("cookieDiscreetLink", part.cookieDiscreetLink);
            element.SetAttributeValue("cookieDiscreetReset", part.cookieDiscreetReset);
            element.SetAttributeValue("cookiePolicyPageMessage", part.cookiePolicyPageMessage);
            element.SetAttributeValue("cookiePolicyPage", part.cookiePolicyPage);
            element.SetAttributeValue("cookieErrorMessage", part.cookieErrorMessage);
            element.SetAttributeValue("cookieDisable", part.cookieDisable);
            element.SetAttributeValue("cookieAcceptButtonText", part.cookieAcceptButtonText);
            element.SetAttributeValue("cookieDeclineButtonText", part.cookieDeclineButtonText);
            element.SetAttributeValue("cookieResetButtonText", part.cookieResetButtonText);
            element.SetAttributeValue("cookieWhatAreLinkText", part.cookieWhatAreLinkText);
            element.SetAttributeValue("cookieAnalytics", part.cookieAnalytics);
            element.SetAttributeValue("cookieAnalyticsMessage", part.cookieAnalyticsMessage);
            element.SetAttributeValue("cookiePolicyLink", part.cookiePolicyLink);
            element.SetAttributeValue("cookieNotificationLocationBottom", part.cookieNotificationLocationBottom);
            element.SetAttributeValue("showCookieDeclineButton", part.showCookieDeclineButton);
            element.SetAttributeValue("showCookieAcceptButton", part.showCookieAcceptButton);
            element.SetAttributeValue("showCookieResetButton", part.showCookieResetButton);
            element.SetAttributeValue("cookieOverlayEnabled", part.cookieOverlayEnabled);
            element.SetAttributeValue("cookieMessage", part.cookieMessage);
            element.SetAttributeValue("cookieWhatAreTheyLink", part.cookieWhatAreTheyLink);
            element.SetAttributeValue("cookieCutter", part.cookieCutter);
        }

        protected override void Importing(CookieCuttrSettingsPart part, ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            part.Record.cookieDiscreetLinkText = GetAttribute<string>(context, partName, "cookieDiscreetLinkText");
            part.Record.cookieDiscreetPosition = GetAttribute<string>(context, partName, "cookieDiscreetPosition");
            part.Record.cookieDomain = GetAttribute<string>(context, partName, "cookieDomain");
            part.Record.cookiePolicyPageMessage = GetAttribute<string>(context, partName, "cookiePolicyPageMessage");
            part.Record.cookieErrorMessage = GetAttribute<string>(context, partName, "cookieErrorMessage");
            part.Record.cookieDisable = GetAttribute<string>(context, partName, "cookieDisable");
            part.Record.cookieAcceptButtonText = GetAttribute<string>(context, partName, "cookieAcceptButtonText");
            part.Record.cookieDeclineButtonText = GetAttribute<string>(context, partName, "cookieDeclineButtonText");
            part.Record.cookieResetButtonText = GetAttribute<string>(context, partName, "cookieResetButtonText");
            part.Record.cookieWhatAreLinkText = GetAttribute<string>(context, partName, "cookieWhatAreLinkText");
            part.Record.cookieAnalyticsMessage = GetAttribute<string>(context, partName, "cookieAnalyticsMessage");
            part.Record.cookiePolicyLink = GetAttribute<string>(context, partName, "cookiePolicyLink");
            part.Record.cookieMessage = GetAttribute<string>(context, partName, "cookieMessage");
            part.Record.cookieWhatAreTheyLink = GetAttribute<string>(context, partName, "cookieWhatAreTheyLink");
            part.Record.cookieDiscreetLink = GetAttribute<bool>(context, partName, "cookieDiscreetLink");
            part.Record.cookieDiscreetReset = GetAttribute<bool>(context, partName, "cookieDiscreetReset");
            part.Record.cookiePolicyPage = GetAttribute<bool>(context, partName, "cookiePolicyPage");
            part.Record.cookieAnalytics = GetAttribute<bool>(context, partName, "cookieAnalytics");
            part.Record.cookieNotificationLocationBottom = GetAttribute<bool>(context, partName, "cookieNotificationLocationBottom");
            part.Record.showCookieDeclineButton = GetAttribute<bool>(context, partName, "showCookieDeclineButton");
            part.Record.showCookieAcceptButton = GetAttribute<bool>(context, partName, "showCookieAcceptButton");
            part.Record.showCookieResetButton = GetAttribute<bool>(context, partName, "showCookieResetButton");
            part.Record.cookieOverlayEnabled = GetAttribute<bool>(context, partName, "cookieOverlayEnabled");
            part.Record.cookieCutter = GetAttribute<bool>(context, partName, "cookieCutter");
        }

        private TV GetAttribute<TV>(ImportContentContext context, string partName, string elementName)
        {
            string value = context.Attribute(partName, elementName);
            if (value != null)
            {
                return (TV)Convert.ChangeType(value, typeof(TV));
            }
            return default(TV);
        }
    }
}