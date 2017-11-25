using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs.Settings
{
    [OrchardFeature("CloudBust.Blogs.Stats")]
    public class ViewersTypePartSettings
    {
        private bool? _showVoter;

        public bool ShowVoter
        {
            get
            {
                if (_showVoter == null)
                    _showVoter = true;
                return (bool)_showVoter;
            }
            set { _showVoter = value; }
        }

        private bool? _allowAnonymousRatings;

        public bool AllowAnonymousRatings
        {
            get
            {
                if (_allowAnonymousRatings == null)
                    _allowAnonymousRatings = false;
                return (bool)_allowAnonymousRatings;
            }
            set { _allowAnonymousRatings = value; }
        }
    }

    public class ViewersContainerSettingsHooks : ContentDefinitionEditorEventsBase
    {
        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "ViewersPart")
                yield break;

            var model = definition.Settings.GetModel<ViewersTypePartSettings>();

            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.Name != "ViewersPart")
                yield break;

            var model = new ViewersTypePartSettings();
            updateModel.TryUpdateModel(model, "ViewersTypePartSettings", null, null);
            builder.WithSetting("ViewersTypePartSettings.ShowVoter", model.ShowVoter.ToString());
            builder.WithSetting("ViewersTypePartSettings.AllowAnonymousRatings", model.AllowAnonymousRatings.ToString());

            yield return DefinitionTemplate(model);
        }
    }
}