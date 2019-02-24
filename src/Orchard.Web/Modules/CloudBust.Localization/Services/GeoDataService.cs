using CloudBust.Localization.Models;
using Orchard;
using RestSharp;

namespace CloudBust.Localization.Services
{
    public class GeoDataService : IGeoDataService
    {
        private readonly IOrchardServices _orchardServices;

        public GeoDataService(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }

        public GeoData GetGeoLocation()
        {
            var remote = _orchardServices.WorkContext.HttpContext.Request.ServerVariables["REMOTE_ADDR"];

            return GetGeoLocation(remote);
        }

        public GeoData GetGeoLocation(string remote)
        {
            if (string.IsNullOrWhiteSpace(remote))
            {
                return null;
            }

            RestClient client = new RestClient("https://freegeoip.net");
            RestRequest request = new RestRequest("json/" + remote);

            return client.Execute<GeoData>(request).Data;
        }
    }
}