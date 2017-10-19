using System.Text.RegularExpressions;
using Orchard;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Services {

    [OrchardFeature("CloudBust.Common.ThemePicker")]
    public class UserAgentThemeSelectionRule : IThemeSelectionRule {
        private readonly IWorkContextAccessor _workContextAccessor;

        public UserAgentThemeSelectionRule(IWorkContextAccessor workContextAccessor) {
            _workContextAccessor = workContextAccessor;
        }

        public bool Matches(string name, string criterion) {
            var agent = _workContextAccessor.GetContext().HttpContext.Request.UserAgent;
            if (agent == null) return false;
            return new Regex(criterion, RegexOptions.IgnoreCase)
                .IsMatch(agent);
        }
    }
}