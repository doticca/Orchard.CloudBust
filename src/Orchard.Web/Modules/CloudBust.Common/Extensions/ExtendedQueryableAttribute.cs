using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.OData.Query;

namespace CloudBust.Common.Extensions
{
    public class ExtendedQueryableAttribute : System.Web.Http.OData.EnableQueryAttribute

    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            long? originalsize = null;
            var inlinecount = HttpUtility.ParseQueryString(actionExecutedContext.Request.RequestUri.Query).Get("$inlinecount");

            object responseObject;
            actionExecutedContext.Response.TryGetContentValue(out responseObject);
            var originalquery = responseObject as IQueryable<object>;

            if (originalquery != null && inlinecount == "allpages")
                originalsize = originalquery.Count();

            base.OnActionExecuted(actionExecutedContext);

            if (ResponseIsValid(actionExecutedContext.Response))
            {
                actionExecutedContext.Response.TryGetContentValue(out responseObject);

                if (responseObject is IQueryable)
                {
                    var robj = responseObject as IQueryable<object>;

                    if (originalsize != null)
                    {
                        actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, new ODataMetadata<object>(robj, originalsize));
                    }
                }
            }
        }

        private bool ResponseIsValid(HttpResponseMessage response)
        {
            if (response == null || response.StatusCode != HttpStatusCode.OK || !(response.Content is ObjectContent)) return false;
            return true;
        }

        public override void ValidateQuery(HttpRequestMessage request, ODataQueryOptions queryOptions)
        {
            // don't report error for unsupported query
        }
    }
}