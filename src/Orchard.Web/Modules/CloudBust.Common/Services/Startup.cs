using Orchard.Environment;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using CloudBust.Common.Formatters;

namespace CloudBust.Common.Services
{
    public class Startup : IOrchardShellEvents
    {
        public Startup()
        {
            // this is called more than once
        }

        void IOrchardShellEvents.Activated()
        {
            var config = GlobalConfiguration.Configuration;
            config.Formatters.Insert(0, new JsonpFormatter());
            config.Formatters.JsonFormatter.AddQueryStringMapping("$format", "json", new MediaTypeHeaderValue("application/json"));
            config.Formatters.XmlFormatter.AddQueryStringMapping("$format", "xml", new MediaTypeHeaderValue("application/xml"));
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        void IOrchardShellEvents.Terminating()
        {

        }
    }
}