using Lombiq.Hosting.Azure.ApplicationInsights.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Lombiq.Hosting.Azure.ApplicationInsights.Drivers {
    public class AzureApplicationInsightsTelemetrySettingsPartDriver : ContentPartDriver<AzureApplicationInsightsTelemetrySettingsPart> {
        private const string TemplateName = "Parts/AzureApplicationInsightsTelemetry.Settings";
        //private readonly IDeferredAppDomainRestarter _appDomainRestarter;


        protected override DriverResult Editor(AzureApplicationInsightsTelemetrySettingsPart part, dynamic shapeHelper) {
            return ContentShape("Parts_AzureApplicationInsightsTelemetrySettings_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: part,
                    Prefix: Prefix)).OnGroup(Constants.SiteSettingsEditorGroup);
        }

        protected override DriverResult Editor(AzureApplicationInsightsTelemetrySettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            if (updater.TryUpdateModel(part, Prefix, null, null)) {
                var previousEnableDependencyTracking = part.ApplicationWideDependencyTrackingIsEnabled;

                updater.TryUpdateModel(part, Prefix, null, null);

                if (previousEnableDependencyTracking != part.ApplicationWideDependencyTrackingIsEnabled) {
                    //_appDomainRestarter.RestartAppDomainWhenRequestEnds();
                }
            }

            return Editor(part, shapeHelper);
        }
    }
}