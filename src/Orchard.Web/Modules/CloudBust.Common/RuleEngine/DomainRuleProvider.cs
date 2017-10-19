using System;
using Orchard.Environment.Configuration;
using Orchard.Mvc;
using Orchard.Conditions.Services;

namespace Orchard.Widgets.RuleEngine {

    public class DomainRuleProvider : IConditionProvider {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ShellSettings _shellSettings;

        public DomainRuleProvider(IHttpContextAccessor httpContextAccessor, ShellSettings shellSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _shellSettings = shellSettings;
        }

        public void Evaluate(ConditionEvaluationContext evaluationContext)
        {
            if (!String.Equals(evaluationContext.FunctionName, "domain", StringComparison.OrdinalIgnoreCase))
                return;
            var context = _httpContextAccessor.Current();
            var domain = Convert.ToString(evaluationContext.Arguments[0]);

            evaluationContext.Result = string.Equals(context.Request.Url.Host, domain, StringComparison.OrdinalIgnoreCase);
        }
    }
}