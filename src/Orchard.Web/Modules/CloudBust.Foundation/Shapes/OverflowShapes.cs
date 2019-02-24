using Orchard.ContentManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Widgets.Models;

namespace CloudBust.Foundation.Shapes {
    public class OverflowShapes : IShapeTableProvider {
        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("Parts_MenuWidget").OnDisplaying(displaying => {
                ContentItem contentItem = displaying.Shape.ContentItem;
                var widgetPart = contentItem.As<WidgetPart>();
                var zoneName = widgetPart.Zone.ToString();
                
                if (zoneName.ToLower() != "overflow") return;
                if (displaying.ShapeMetadata.DisplayType != "Detail") return;

                var menuShape = displaying.Shape.Menu;
                menuShape.Metadata.Alternates.Add("Menu___" + zoneName);
                foreach (var o in menuShape.Items) {
                    string type = o.Item.GetType().Name;
                    o.Metadata.Alternates.Add(type + "___" + zoneName);
                }
            });
        }
    }
}