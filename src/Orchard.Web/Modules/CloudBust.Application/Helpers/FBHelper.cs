using CloudBust.Application.Models;
using CloudBust.Application.OData;
using Facebook;

namespace CloudBust.Application.Helpers
{
    public static class FBHelper
    {
        //private static string ApiKey = "1450111005274061";
        //private static string ApiSecret = "1536d5a8e3519ca310b50a5d16d333ea";

        private static string ApplicationToken = string.Empty;
        public static string GetApplicationToken(string ApiKey, string ApiSecret)
        {
            if (string.IsNullOrEmpty(ApplicationToken)){
                var fb = new FacebookClient();
                dynamic result = fb.Get("oauth/access_token", new {
                    client_id = ApiKey,
                    client_secret = ApiSecret,
                    grant_type = "client_credentials"
                });
                ApplicationToken = result.access_token;
            }
            return ApplicationToken;
        }

        public static DebugFB GetDebugInfo(string access_token, ApplicationRecord appRecord)
        {
            string apptoken = GetApplicationToken(appRecord.fbAppKey, appRecord.fbAppSecret);
            var fb = new FacebookClient();
            dynamic result = fb.Get("debug_token", new
            {
                access_token = apptoken,
                input_token = access_token
            });

            DebugFB debugFB = new DebugFB(result);

            return debugFB;
        }
    }
}