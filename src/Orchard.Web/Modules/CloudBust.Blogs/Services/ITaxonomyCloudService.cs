using System.Collections.Generic;
using Orchard;
using Orchard.Taxonomies.Models;

namespace CloudBust.Blogs.Services {
    public interface ITaxonomyCloudService : IDependency {
        IEnumerable<TermPart> GetTopicTerms();
    }
}