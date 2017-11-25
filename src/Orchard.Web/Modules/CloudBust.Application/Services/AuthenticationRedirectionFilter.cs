using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using Orchard.Mvc.Filters;
using Orchard.Environment.Extensions;

namespace CloudBust.Application.Services {

    /// <summary>
    /// This class is responsible for redirecting the user to the authentication page 
    /// of the current tenant.
    /// </summary>
    [OrchardFeature("CloudBust.Application.WebApp")]
    [OrchardSuppressDependency("Orchard.Users.Services.AuthenticationRedirectionFilter")]
    public class AuthenticationRedirectionFilter : FilterProvider, IAuthenticationFilter {

        public void OnAuthentication(AuthenticationContext filterContext) {
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext) {
            if (filterContext.Result is HttpUnauthorizedResult) {
                filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                {
                    { "controller", "Account" },
                    { "action", "AccessDenied" },
                    { "area", "CloudBust.Application"},
                    { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
                });
            }
        }
    }
}