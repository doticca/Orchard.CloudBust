using System.Collections.Generic;
using System.Linq;
using Orchard.ContentManagement;

namespace CloudBust.Application.Services
{
    public class PartWatcher : IPartWatcher
    {
        readonly HashSet<IContent> _parts = new HashSet<IContent>();

        public void Watch<T>(T part) where T : IContent
        {
            _parts.Add(part);
        }

        public IEnumerable<T> Get<T>() where T : IContent
        {
            return _parts.Where(p => p.Is<T>()).Select(p => p.As<T>());
        }

    }
}