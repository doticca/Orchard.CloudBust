using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.OData;

namespace CloudBust.Common.Extensions
{
    public class ExtendedQueryableAttribute : EnableQueryAttribute

    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            long? originalsize = null;
            var inlinecount = HttpUtility.ParseQueryString(actionExecutedContext.Request.RequestUri.Query).Get("$inlinecount");

            actionExecutedContext.Response.TryGetContentValue(out object responseObject);

            if (responseObject is IQueryable<object> originalquery && inlinecount == "allpages")
                originalsize = originalquery.Count();

            base.OnActionExecuted(actionExecutedContext);

            if (!ResponseIsValid(actionExecutedContext.Response)) return;

            actionExecutedContext.Response.TryGetContentValue(out responseObject);

            if (!(responseObject is IQueryable)) return;

            var robj = responseObject as IQueryable<object>;

            if (originalsize != null)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, new ODataMetadata<object>(robj, originalsize));
            }
        }

        private static bool ResponseIsValid(HttpResponseMessage response) {
            return response != null && response.StatusCode == HttpStatusCode.OK && response.Content is ObjectContent;
        }

        public override void ValidateQuery(HttpRequestMessage request, System.Web.Http.OData.Query.ODataQueryOptions queryOptions)
        {
            // don't report error for unsupported query
        }
    }
}