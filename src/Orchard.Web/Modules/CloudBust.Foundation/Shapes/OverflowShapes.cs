using Orchard.ContentManagement;
using Orchard.DisplayManagement.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Widgets.Models;

namespace CloudBust.Foundation.Shapes
{
    public class OverflowShapes : IShapeTableProvider
    {
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Parts_MenuWidget").OnDisplaying(displaying =>
            {
                ContentItem contentItem = displaying.Shape.ContentItem;
                var widgetPart = contentItem.As<WidgetPart>();
                string zoneName = widgetPart.Zone.ToString();
                if(zoneName.ToLower() == "overflow"){
                    if (displaying.ShapeMetadata.DisplayType == "Detail")
                    {
                        var MenuShape = displaying.Shape.Menu;
                        MenuShape.Metadata.Alternates.Add("Menu___" + zoneName);
                        foreach (dynamic o in MenuShape.Items)
                        {
                            string type = o.Item.GetType().Name;
                            o.Metadata.Alternates.Add(type + "___" + zoneName);
                        }
                    }
                }
            });
        }
    }
}