using System.Text.RegularExpressions;
using Orchard;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Services {

    [OrchardFeature("CloudBust.Common.ThemePicker")]
    public class RequestDirectoryThemeSelectionRule : IThemeSelectionRule {
        private readonly IWorkContextAccessor _workContextAccessor;

        public RequestDirectoryThemeSelectionRule(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }

        public bool Matches(string name, string criterion) {
            var agent = _workContextAccessor.GetContext().HttpContext.Request.Url.AbsolutePath;
            if (agent == null) return false;

            return new Regex(criterion, RegexOptions.IgnoreCase)
                .IsMatch(agent);
        }
    }
}