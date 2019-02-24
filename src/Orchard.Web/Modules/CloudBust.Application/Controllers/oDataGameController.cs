using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloudBust.Application.Models;
using CloudBust.Application.OData;
using CloudBust.Application.Services;
using CloudBust.Common.Extensions;
using CloudBust.Common.OData;
using Orchard;
using Orchard.Caching;
using Orchard.Core.XmlRpc.Controllers;

namespace CloudBust.Application.Controllers
{
    public class oDataGameController : ApiController
    {
        private readonly IApplicationsService _applicationsService;
        private readonly ICacheManager _cacheManager;
        private readonly IGamesService _gamesService;
        private readonly IOrchardServices _orchardServices;
        private readonly ISignals _signals;

        public oDataGameController(IOrchardServices orchardServices, IApplicationsService applicationsService, IGamesService gamesService, ICacheManager cacheManager, ISignals signals)
        {
            _orchardServices = orchardServices;
            _applicationsService = applicationsService;
            _gamesService = gamesService;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        private string HostUrl()
        {
            return _orchardServices.WorkContext.CurrentSite.BaseUrl;
        }

        private int GetAppId()
        {
            if (_orchardServices.WorkContext.CurrentUser == null)
                return 0;

            try
            {
                var appid = _orchardServices.WorkContext.HttpContext.Session["doticca_aid"].ToString();

                if (string.IsNullOrWhiteSpace(appid))
                    return 0;

                return !int.TryParse(appid, out var aid) ? 0 : aid;
            }
            catch
            {
                return 0;
            }
        }

        [HttpGet]
        public HttpResponseMessage GetGame(string key = null)
        {
            var cachekey = CBSignals.SignalApplications;
            if (!string.IsNullOrWhiteSpace(key))
            {
                var cat = _cacheManager.Get(cachekey, ctx =>
                                                      {
                                                          ctx.Monitor(_signals.When(cachekey));
                                                          return new ApplicationGame(_gamesService.GetGameByKey(key), HostUrl());
                                                      });
                return Request.CreateResponse(HttpStatusCode.OK, cat);
            }

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [ExtendedQueryable]
        [LiveWriterController.NoCache]
        public IQueryable<ApplicationGame> GetGames()
        {
            var id = GetAppId();
            if (id <= 0) return null;

            var key = CBSignals.SignalApplications;
            var mods = _cacheManager.Get(key, ctx =>
                                              {
                                                  ctx.Monitor(_signals.When(key));
                                                  return _gamesService.GetApplicationGames(id).Select(c => new ApplicationGame(c, HostUrl()));
                                              });

            return mods.AsQueryable();
        }

        [ActionName("Score")]
        [HttpPut]
        public HttpResponseMessage PutScore(string key, uGameScore gameScore)
        {
            if (gameScore == null) return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            var user = _orchardServices.WorkContext.CurrentUser;
            if (user == null) return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
            var id = GetAppId();
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));

            var app = _applicationsService.GetApplication(id);
            if (app == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            var game = _gamesService.GetGameByKey(key);
            if (game == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            var r = new GameScoreRecord();

            r.Agility = gameScore.Agility;
            r.AgilityScore = gameScore.AgilityScore;
            r.AgilityText = gameScore.AgilityText;
            r.Accuracy = gameScore.Accuracy;
            r.AccuracyScore = gameScore.AccuracyScore;
            r.AccuracyText = gameScore.AccuracyText;
            r.ApplicationName = app.Name;
            r.Attention = gameScore.Attention;
            r.EndDate = gameScore.EndDate;
            r.Executive = gameScore.Executive;
            r.GameName = game.Name;
            r.Score = gameScore.Score;
            r.Smoothness = gameScore.Smoothness;
            r.SmoothnessScore = gameScore.SmoothnessScore;
            r.SmoothnessText = gameScore.SmoothnessText;
            r.Spatial = gameScore.Spatial;
            r.Stability = gameScore.Stability;
            r.StabilityScore = gameScore.StabilityScore;
            r.StabilityText = gameScore.StabilityText;
            r.StartDate = gameScore.StartDate;
            r.UserName = user.UserName;

            _gamesService.StoreScore(r);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}