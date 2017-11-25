using Orchard;
using Orchard.ContentManagement;
using System.Collections.Generic;

namespace CloudBust.Application.Services
{
    public interface IPartWatcher : IDependency
    {
        void Watch<T>(T part) where T : IContent;
        IEnumerable<T> Get<T>() where T : IContent;
    }
}