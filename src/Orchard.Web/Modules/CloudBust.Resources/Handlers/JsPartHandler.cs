using System;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using CloudBust.Resources.Models;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Handlers
{
    [OrchardFeature("CloudBust.Resources.Ace")]
    public class JsPartHandler : ContentHandler
    {
        public JsPartHandler(IRepository<JsPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
