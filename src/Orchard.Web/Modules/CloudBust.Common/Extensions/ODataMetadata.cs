using System.Collections.Generic;

namespace CloudBust.Common.Extensions {
    public class ODataMetadata<T> where T : class {
        public ODataMetadata(IEnumerable<T> result, long? count) {
            Count = count;
            Results = result;
        }

        public IEnumerable<T> Results { get; }

        public long? Count { get; }
    }
}