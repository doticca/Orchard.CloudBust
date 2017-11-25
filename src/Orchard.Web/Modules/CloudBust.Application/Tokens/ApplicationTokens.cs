using System;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Tokens;

namespace CloudBust.Application.Tokens {

    public class ApplicationTokens : ITokenProvider {
        private readonly IContentManager _contentManager;

        public ApplicationTokens(
            IContentManager contentManager
            )
        { 
            _contentManager = contentManager;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeContext context) {
            context.For("CloudBust", T("CloudBust"), T("CloudBust Tokens for Application URLS"))
                .Token("LogIn", T("Login"), T("The URL of the login page"))
                .Token("Register", T("Register"), T("The URL of the register page"))
                .Token("Home", T("Home"), T("The Home page"))
                .Token("AbsoluteHome", T("AbsoluteHome"), T("The absolute (public) url of the Home page"))
                ;
        }

        public void Evaluate(EvaluateContext context)
        {
            context.For<ICloudBustTokens>("CloudBust", () => new CloudBustTokens())
              .Token("Login", cloudbustTokens => cloudbustTokens.GetLogin())
              .Token("Register", cloudbustTokens => cloudbustTokens.GetRegister())
              .Token("Home", cloudbustTokens => cloudbustTokens.GetHome())
              .Token("AbsoluteHome", cloudbustTokens => cloudbustTokens.GetAbsoluteHome());
        }
    }
}
public class CloudBustTokens : ICloudBustTokens
{
    public string GetLogin()
    {
        var _Url = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);

        return _Url.Action("LogOn", "Account", new { Area = "CloudBust.Application" });
    }
    public string GetRegister()
    {
        var _Url = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);

        return _Url.Action("Register", "Account", new { Area = "CloudBust.Application" });
    }
    public string GetHome()
    {
        var _Url = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);

        return _Url.Content("~/");
    }
    public string GetAbsoluteHome()
    {
        var _Url = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
        var path = _Url.Content("~/");
        var url = new Uri(System.Web.HttpContext.Current.Request.Url, path).ToString();

        return url;
    }
}
    public interface ICloudBustTokens
{
    string GetLogin();
    string GetRegister();
    string GetHome();
    string GetAbsoluteHome();
}