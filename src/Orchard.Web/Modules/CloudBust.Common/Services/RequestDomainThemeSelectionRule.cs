using System.Text.RegularExpressions;
using Orchard;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Services {

    [OrchardFeature("CloudBust.Common.ThemePicker")]
    public class RequestDomainThemeSelectionRule : IThemeSelectionRule {
        private readonly IWorkContextAccessor _workContextAccessor;

        public RequestDomainThemeSelectionRule(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }

        public bool Matches(string name, string criterion) {
            var agent = _workContextAccessor.GetContext().HttpContext.Request.Url.Host;
            if (agent == null) return false;

            return new Regex(criterion, RegexOptions.IgnoreCase)
                .IsMatch(agent);
        }
    }
}