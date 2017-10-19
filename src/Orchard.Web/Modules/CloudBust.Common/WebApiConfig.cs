using Autofac;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace CloudBust.Common
{
    public class WebApiConfig : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();

            if (jsonFormatter != null)
            {
                jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
        }
    }
}