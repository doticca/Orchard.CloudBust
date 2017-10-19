using System;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using CloudBust.Resources.Models;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Handlers
{
    [OrchardFeature("CloudBust.Resources.Ace")]
    public class CssPartHandler : ContentHandler
    {

        public CssPartHandler(IRepository<CssPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
