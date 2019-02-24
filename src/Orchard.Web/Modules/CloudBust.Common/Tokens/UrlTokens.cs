using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Tokens;
using System;

namespace CloudBust.Common.Tokens
{

    public class UrlTokens : ITokenProvider {
        private readonly IContentManager _contentManager;

        public UrlTokens(
            IContentManager contentManager
            )
        { 
            _contentManager = contentManager;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeContext context) {
            context.For("CommonUrl", T("Common Url"), T("Tokens for common URLS"))
                .Token("Home", T("Home"), T("The Home page"))
                .Token("AbsoluteHome", T("AbsoluteHome"), T("The absolute (public) url of the Home page"))
                ;
        }

        public void Evaluate(EvaluateContext context)
        {
            context.For<ICommonUrlTokens>("CommonUrl", () => new CommonUrlTokens())
              .Token("Home", cloudbustTokens => cloudbustTokens.GetHome())
              .Token("AbsoluteHome", cloudbustTokens => cloudbustTokens.GetAbsoluteHome())
              ;
        }
    }
}
public class CommonUrlTokens : ICommonUrlTokens
{
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
public interface ICommonUrlTokens
{
    string GetHome();
    string GetAbsoluteHome();
}