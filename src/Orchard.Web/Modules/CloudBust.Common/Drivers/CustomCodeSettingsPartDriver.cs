using CloudBust.Common.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Drivers {

    [OrchardFeature("CloudBust.Common.CustomCode")]
    public class CustomCodeSettingsPartDriver : ContentPartDriver<CustomCodeSettingsPart> {
        private const string TemplateName = "Parts/CustomCodeSettings";

        public CustomCodeSettingsPartDriver() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix { get { return "CustomCodeSettings"; } }

        // GET
        protected override DriverResult Editor(CustomCodeSettingsPart part, dynamic shapeHelper) {
            return ContentShape("Parts_CustomCodeSettings_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: part,
                    Prefix: Prefix)
                ).OnGroup("CustomCode");
        }

        // POST
        protected override DriverResult Editor(CustomCodeSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}