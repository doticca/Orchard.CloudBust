using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs.Settings
{
    public class ShareTypePartSettings
    {
        private bool? _showFacebook;

        public bool ShowFacebook
        {
            get
            {
                if (_showFacebook == null)
                    _showFacebook = true;
                return (bool)_showFacebook;
            }
            set { _showFacebook = value; }
        }

        private bool? _showTwitter;

        public bool ShowTwitter
        {
            get
            {
                if (_showTwitter == null)
                    _showTwitter = false;
                return (bool)_showTwitter;
            }
            set { _showTwitter = value; }
        }

        private bool? _showMail;

        public bool ShowMail
        {
            get
            {
                if (_showMail == null)
                    _showMail = true;
                return (bool)_showMail;
            }
            set { _showMail = value; }
        }
    }

    public class ShareContainerSettingsHooks : ContentDefinitionEditorEventsBase
    {
        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "SharePart")
                yield break;

            var model = definition.Settings.GetModel<ShareTypePartSettings>();

            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.Name != "SharePart")
                yield break;

            var model = new ShareTypePartSettings();
            updateModel.TryUpdateModel(model, "ShareTypePartSettings", null, null);
            builder.WithSetting("ShareTypePartSettings.ShowFacebook", model.ShowFacebook.ToString());
            builder.WithSetting("ShareTypePartSettings.ShowTwitter", model.ShowTwitter.ToString());
            builder.WithSetting("ShareTypePartSettings.ShowMail", model.ShowMail.ToString());

            yield return DefinitionTemplate(model);
        }
    }
}