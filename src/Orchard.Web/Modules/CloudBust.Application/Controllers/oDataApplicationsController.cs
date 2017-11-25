using CloudBust.Application.Models;
using CloudBust.Application.OData;
using CloudBust.Application.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.Records;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Orchard.Caching;
using CloudBust.Common.Extensions;
using CloudBust.Common.OData;
using Orchard.Security;
using System;

namespace CloudBust.Application.Controllers
{
    public class oDataApplicationsController : ApiController
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IApplicationsService _applicationsService;
        private readonly IParametersService _parametersService;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public oDataApplicationsController(
                               IOrchardServices orchardServices
                               ,IApplicationsService applicationsService
                               ,IParametersService parametersService
                               ,ICacheManager cacheManager
                               ,ISignals signals 
            )
        {
            _orchardServices = orchardServices;
            _applicationsService = applicationsService;
            _parametersService = parametersService;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        [ExtendedQueryable]
        [Orchard.Core.XmlRpc.Controllers.LiveWriterController.NoCache]
        public IQueryable<OData.Category.Category> GetCategories()
        {
            var key = CBSignals.SignalApplications;
            var cats = _cacheManager.Get(key, ctx =>
                {
                    ctx.Monitor(_signals.When(key));
                    return _applicationsService.GetCategories()
                        .Select(c => new OData.Category.Category(c, Request));
                }
            );

            List<OData.Category.Category> categories = new List<OData.Category.Category>();
            categories.Add(new OData.Category.Category(Request));
            foreach (OData.Category.Category c in cats)
            {
                categories.Add(c);
            }

            return categories.AsQueryable();
        }

        [ExtendedQueryable]
        [Orchard.Core.XmlRpc.Controllers.LiveWriterController.NoCache]
        public IQueryable<CloudBust.Application.OData.Application.Application> GetApplications()
        {
            var key = CBSignals.SignalApplications;
            var mods = _cacheManager.Get(key, ctx =>
            {
                ctx.Monitor(_signals.When(key));
                return _applicationsService.GetApplications()
                    .Select(c => new CloudBust.Application.OData.Application.Application(c, Request));
            }
            );

            return mods.AsQueryable();
        }

        [HttpGet]
        public HttpResponseMessage GetCategory(int id)
        {
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.OK, new OData.Category.Category(Request)); 

            var key = CBSignals.SignalApplications;
            var cat = _cacheManager.Get(key, ctx =>
            {
                ctx.Monitor(_signals.When(key));
                var category = _applicationsService.GetCategory(id);
                if (category != null)
                    return new OData.Category.Category(category, Request);
                else
                    return null;
            }
            );

            if(cat==null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
            else
                return Request.CreateResponse(HttpStatusCode.OK, cat);
        }


        [HttpGet]
        public HttpResponseMessage GetApplication(int id = 0)
        {
            if(id<=0)
            {
                if (_orchardServices.WorkContext.CurrentUser == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));

                try
                {
                    string appid = _orchardServices.WorkContext.HttpContext.Session["doticca_aid"].ToString();

                    if (string.IsNullOrWhiteSpace(appid))
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));

                    int aid;
                    if (!Int32.TryParse(appid, out aid))
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));

                    id = aid;
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
                }
            }
            var key = CBSignals.SignalApplications;

            var mod = _cacheManager.Get(key, ctx =>
                {
                    ctx.Monitor(_signals.When(key));
                    var application = _applicationsService.GetApplication(id);
                    if (application != null)
                        return new OData.Application.Application(_applicationsService.GetApplication(id), Request);
                    else
                        return null;
                }
            );

            if (mod == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
            else
                return Request.CreateResponse(HttpStatusCode.OK, mod);
        }

        [HttpGet]
        public HttpResponseMessage GetCategoriesCount()
        {
            var cats =_applicationsService.GetCategories().Count();

            return Request.CreateResponse(HttpStatusCode.OK, new Totals(cats, "Category"));
        }

        [HttpGet]
        public HttpResponseMessage GeApplicationsCount()
        {
            var mods = _applicationsService.GetApplications().Count();

            return Request.CreateResponse(HttpStatusCode.OK, new Totals(mods, "Application"));
        }    
    }
}