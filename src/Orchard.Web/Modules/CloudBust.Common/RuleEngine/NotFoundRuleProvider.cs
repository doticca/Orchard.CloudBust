using System;
using Orchard.Environment.Configuration;
using Orchard.Mvc;
using Orchard.Conditions.Services;
using System.Net;

namespace Orchard.Widgets.RuleEngine {

    public class NotFoundRuleProvider : IConditionProvider {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ShellSettings _shellSettings;

        public NotFoundRuleProvider(IHttpContextAccessor httpContextAccessor, ShellSettings shellSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _shellSettings = shellSettings;
        }

        public void Evaluate(ConditionEvaluationContext evaluationContext)
        {
            if (!String.Equals(evaluationContext.FunctionName, "notfound", StringComparison.OrdinalIgnoreCase))
                return;
            var context = _httpContextAccessor.Current();      
            evaluationContext.Result = context.Response.StatusCode == (int)HttpStatusCode.NotFound;
        }
    }
}