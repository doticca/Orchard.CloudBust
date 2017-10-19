using CloudBust.Resources.Services;
using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.GoogleMaps")]
    public class GoogleMaps : IResourceManifestProvider {
        private readonly IGoogleMapsService _googleMapsService;
        public GoogleMaps(IGoogleMapsService googleMapsService)
        {
            _googleMapsService = googleMapsService;
        }
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            string callback = _googleMapsService.GetCallBack();
            string language = _googleMapsService.GetLanguage();
            //bool sensor = _googleMapsService.GetSensor(); deprecated
            bool async = _googleMapsService.GetAsync();
            bool defer = _googleMapsService.GetDefer();
            string callbackparam = string.IsNullOrWhiteSpace(callback) ? "" : string.Format("&callback={0}", callback);
            string languageparam = string.IsNullOrWhiteSpace(language) ? "" : string.Format("&language={0}", language);
            //string sensorparam = string.Format("&sensor={0}", sensor ? "true" : "false"); deprecated

            string gurl = string.Format("https://maps.googleapis.com/maps/api/js?key={0}{1}{2}", _googleMapsService.GetApiKey(), callbackparam, languageparam);
            
            ResourceDefinition rd = manifest.DefineScript("GoogleMaps")
                                    .SetUrl(gurl, gurl)
                                    .SetVersion("3.22");
            if (async)
                rd.SetAttribute("async", "async");
            if (defer)
                rd.SetAttribute("defer", "defer");

        }
    }
}
