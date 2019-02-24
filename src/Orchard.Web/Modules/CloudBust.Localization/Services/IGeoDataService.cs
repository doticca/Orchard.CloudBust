using CloudBust.Localization.Models;
using Orchard;

namespace CloudBust.Localization.Services
{
    public interface IGeoDataService : IDependency
    {
        GeoData GetGeoLocation(string remote);
        GeoData GetGeoLocation();
    }
}