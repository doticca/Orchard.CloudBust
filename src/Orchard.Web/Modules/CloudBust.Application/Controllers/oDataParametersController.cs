using CloudBust.Application.Models;
using CloudBust.Application.Services;
using Orchard;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Orchard.Caching;
using CloudBust.Common.Extensions;
using CloudBust.Common.OData;

namespace CloudBust.Application.Controllers
{
    public class oDataParametersController : ApiController
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IApplicationsService _applicationsService;
        private readonly IParametersService _parametersService;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public oDataParametersController(
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
        public IQueryable<OData.ParameterCategory.ParameterCategory> GetParameterCategoriesForApplication(int applicationId)
        {
            var key = CBSignals.SignalParameters;
            var cats = _cacheManager.Get(key, ctx =>
            {
                ctx.Monitor(_signals.When(key));
                return _parametersService.GetParameterCategoriesForApplication(applicationId)
                    .Select(c => new OData.ParameterCategory.ParameterCategory(c, Request));
            }
            );

            List<OData.ParameterCategory.ParameterCategory> categories = new List<OData.ParameterCategory.ParameterCategory>();
            categories.Add(new OData.ParameterCategory.ParameterCategory(applicationId, Request));
            foreach (OData.ParameterCategory.ParameterCategory c in cats)
            {
                categories.Add(c);
            }

            return categories.AsQueryable();
        }

        [HttpGet]
        public HttpResponseMessage GetParameterCategoriesForApplicationCount(int applicationId)
        {
            var cats = _parametersService.GetParameterCategoriesForApplication(applicationId).Count();

            return Request.CreateResponse(HttpStatusCode.OK, new Totals(cats, "Parameter Category"));
        }

        [HttpGet]
        public HttpResponseMessage GetParameterCategory(int parameterCategoryId)
        {            
            if (parameterCategoryId == 0)
                return Request.CreateResponse(HttpStatusCode.OK, new OData.ParameterCategory.ParameterCategory(0, Request));

            var key = CBSignals.SignalParameters;
            var cat = _cacheManager.Get(key, ctx =>
            {
            ctx.Monitor(_signals.When(key));
                var parametercategory = _parametersService.GetParameterCategory(parameterCategoryId);
                if (parametercategory != null)
                    return new OData.ParameterCategory.ParameterCategory(parametercategory, Request);
                else
                    return null;
            }
            );

            if (cat == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
            else
                return Request.CreateResponse(HttpStatusCode.OK, cat);
        }

        [HttpGet]
        public HttpResponseMessage GetParameterCategoryByNameForApplication(int applicationId, string parameterCategoryName)
        {
            var application = _applicationsService.GetApplication(applicationId);
            if(application==null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            if(parameterCategoryName=="All Parameters")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new OData.ParameterCategory.ParameterCategory(applicationId, Request));
            }

            var key = CBSignals.SignalParameters;
            var cat = _cacheManager.Get(key, ctx =>
            {
                ctx.Monitor(_signals.When(key));
                var parametercategory = _parametersService.GetParameterCategoryForApplication(applicationId, parameterCategoryName);
                if (parametercategory != null)
                    return new OData.ParameterCategory.ParameterCategory(parametercategory, Request);
                else
                    return null;
            }
            );

            if (cat == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
            else
                return Request.CreateResponse(HttpStatusCode.OK, cat);
        }

        [HttpGet]
        public HttpResponseMessage GetParameterCategoryFieldsForApplication(int applicationId, string parameterCategoryName)
        {
            var key = CBSignals.SignalApplications;

            var mod = _cacheManager.Get(key, ctx =>
            {
                ctx.Monitor(_signals.When(key));
                return _applicationsService.GetApplication(applicationId);
            }
            );



            if (mod == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            ParameterCategoryRecord cat = null;
            if (parameterCategoryName == "All Parameters")
            {
                cat = new ParameterCategoryRecord
                {
                    Id = 0,
                    ApplicationRecord = mod,
                    Description = "Browse all Parameters",
                    Name = "All Parameters"
                };
            }
            else
            {
                cat = _parametersService.GetParameterCategoryForApplication(applicationId, parameterCategoryName);
                if (cat == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Parameter Category not found", 404));
                }
            }

            string r = Request.RequestUri.AbsolutePath.ToLower();

            if (r.EndsWith("name"))// && r.Count(f => f == '/') == 4)
            {
                OData.ParameterCategory.fieldName category = new OData.ParameterCategory.fieldName(cat, Request);
                return Request.CreateResponse(HttpStatusCode.OK, category);
            }
            if (r.EndsWith("description"))// && r.Count(f => f == '/') == 4)
            {
                OData.ParameterCategory.fieldDescription category = new OData.ParameterCategory.fieldDescription(cat, Request);
                return Request.CreateResponse(HttpStatusCode.OK, category);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

    }
}