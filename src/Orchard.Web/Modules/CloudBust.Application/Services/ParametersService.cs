using CloudBust.Application.Models;
using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Logging;
using Orchard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Application.Services
{
    public class ParametersService: IParametersService
    {
        private readonly ISignals _signals;
        private readonly IOrchardServices _orchardServices;
        private readonly IApplicationsService _applicationsService;
        private readonly IContentManager _contentManager;
        private readonly IClock _clock;
        

        private readonly IRepository<ParameterCategoryRecord> _parametercategoriesRepository;
        //private readonly IRepository<ApplicationRecord> _applicationsRepository;
        private readonly IRepository<ParameterRecord> _parametersRepository;

        public ParametersService(
                                IContentManager contentManager
                                , IOrchardServices orchardServices
                                , IApplicationsService applicationService 
                                , IRepository<ParameterCategoryRecord> parametercategoriesRepository
                                , IRepository<ParameterRecord> parametersRepository
                                , ISignals signals
                                , IClock clock
            )
        {
            _orchardServices = orchardServices;
            _applicationsService = applicationService;
            _contentManager = contentManager;
            _parametercategoriesRepository = parametercategoriesRepository;
            _parametersRepository = parametersRepository;
            _signals = signals;
            Logger = NullLogger.Instance;
            _clock = clock;
        }

        public ILogger Logger { get; set; }

        private void TriggerSignal()
        {
            _signals.Trigger(CBSignals.SignalParameters);
        }
        // PARAMETER CATEGORIES
        #region Parameter Categories

        public ParameterCategoryRecord GetParameterCategory(int Id)
        {
            try
            {
                var category = _parametercategoriesRepository.Get(Id);
                return category;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<ParameterCategoryRecord> GetParameterCategoriesForApplication(ApplicationRecord applicationRecord)
        {
            if (applicationRecord == null)
                return new List<ParameterCategoryRecord>();

            try
            {                
                var categories = from category in _parametercategoriesRepository.Table where category.ApplicationRecord.Id == applicationRecord.Id select category;
                return categories.ToList();
            }
            catch
            {
                return new List<ParameterCategoryRecord>();
            }
        }
        public IEnumerable<ParameterCategoryRecord> GetParameterCategoriesForApplication(int applicationId)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationId);
            return GetParameterCategoriesForApplication(applicationRecord);
        }
        public IEnumerable<ParameterCategoryRecord> GetParameterCategoriesForApplication(string applicationName)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplicationByName(applicationName);
            return GetParameterCategoriesForApplication(applicationRecord);
        }
        public ParameterCategoryRecord GetParameterCategoryForApplication(ApplicationRecord applicationRecord, string parameterCategoryName)
        {
            if (applicationRecord == null)
                return null;

            return _parametercategoriesRepository.Get(x => x.Name == parameterCategoryName && x.ApplicationRecord.Id == applicationRecord.Id);
        }
        public ParameterCategoryRecord GetParameterCategoryForApplication(int applicationId, string parameterCategoryName)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationId);

            return GetParameterCategoryForApplication(applicationRecord, parameterCategoryName);
        }
        public ParameterCategoryRecord GetParameterCategoryForApplication(string applicationName, string parameterCategoryName)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationName);

            return GetParameterCategoryForApplication(applicationRecord, parameterCategoryName);
        }
        public ParameterCategoryRecord CreateParameterCategoryForApplication(ApplicationRecord applicationRecord, string parameterCategoryName, string parameterCategoryDescription)
        {
            if (applicationRecord == null)
                return null;

            ParameterCategoryRecord r = new ParameterCategoryRecord();
            r.Name = parameterCategoryName;
            r.Description = parameterCategoryDescription;
            r.NormalizedName = parameterCategoryName.ToLowerInvariant();
            r.ApplicationRecord = applicationRecord;

            _parametercategoriesRepository.Create(r);

            TriggerSignal();

            return GetParameterCategoryForApplication(applicationRecord, parameterCategoryName);
        }
        public ParameterCategoryRecord CreateParameterCategoryForApplication(int applicationId, string parameterCategoryName, string parameterCategoryDescription)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationId);

            return CreateParameterCategoryForApplication(applicationRecord, parameterCategoryName, parameterCategoryDescription);
        }
        public ParameterCategoryRecord CreateParameterCategoryForApplication(string applicationName, string parameterCategoryName, string parameterCategoryDescription)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationName);

            return CreateParameterCategoryForApplication(applicationRecord, parameterCategoryName, parameterCategoryDescription);
        }
        public bool DeleteParameterCategory(int parameterCategoryId)
        {
            ParameterCategoryRecord r = GetParameterCategory(parameterCategoryId);
            if (r == null) return false;
            _parametercategoriesRepository.Delete(r);
            TriggerSignal();
            return true;
        }
        public bool UpdateParameterCategory(int parameterCategoryId, string categoryName, string categoryDescription)
        {
            ParameterCategoryRecord applicationcategoryRecord = GetParameterCategory(parameterCategoryId);

            applicationcategoryRecord.Name = categoryName;
            applicationcategoryRecord.Description = categoryDescription;
            applicationcategoryRecord.NormalizedName = categoryName.ToLowerInvariant();

            TriggerSignal();
            return true;
        }

        #endregion

        // PARAMETERS
        #region Parameters

        public ParameterRecord GetParameter(int Id)
        {
            try
            {
                var parameter = _parametersRepository.Get(Id);
                return parameter;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<ParameterRecord> GetParametersForApplication(ApplicationRecord applicationRecord)
        {
            if (applicationRecord == null)
                return new List<ParameterRecord>();

            try
            {
                var parameters = from parameter in _parametersRepository.Table where parameter.ApplicationRecord.Id == applicationRecord.Id select parameter;
                return parameters.ToList();
            }
            catch
            {
                return new List<ParameterRecord>();
            }
        }
        public IEnumerable<ParameterRecord> GetParametersForApplication(int applicationId)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationId);
            return GetParametersForApplication(applicationRecord);
        }
        public IEnumerable<ParameterRecord> GetParametersForApplication(string applicationName)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationName);
            return GetParametersForApplication(applicationRecord);
        }
        public ParameterRecord GetParameterForApplication(ApplicationRecord applicationRecord, string parameterName)
        {
            if (applicationRecord == null)
                return null;

            return _parametersRepository.Get(x => x.Name == parameterName && x.ApplicationRecord.Id == applicationRecord.Id);
        }
        public ParameterRecord GetParameterForApplication(int applicationId, string parameterName)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationId);
            return GetParameterForApplication(applicationRecord, parameterName);
        }
        public ParameterRecord GetParameterForApplication(string applicationName, string parameterName)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationName);
            return GetParameterForApplication(applicationRecord, parameterName);
        }
        public ParameterRecord CreateParameterForApplication(ApplicationRecord applicationRecord, string parameterName, string parameterDescription)
        {
            if (applicationRecord == null)
                return null;
            _parametersRepository.Create(new ParameterRecord { Name = parameterName, Description = parameterDescription, NormalizedName = parameterName.ToLowerInvariant(), ApplicationRecord = applicationRecord });
            TriggerSignal();
            return GetParameterForApplication(applicationRecord, parameterName);
        }
        public ParameterRecord CreateParameterForApplication(int applicationId, string parameterName, string parameterDescription)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationId);

            return CreateParameterForApplication(applicationRecord, parameterName, parameterDescription);
        }
        public ParameterRecord CreateParameterForApplication(string applicationName, string parameterName, string parameterDescription)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationName);

            return CreateParameterForApplication(applicationRecord, parameterName, parameterDescription);
        }
        public bool SetCategoryForParameter(int parameterId, int parameterCategoryId)
        {
            ParameterCategoryRecord categoryRecord = GetParameterCategory(parameterCategoryId);
            if (categoryRecord == null) return false;

            ParameterRecord parameterRecord = GetParameter(parameterId);
            if (parameterRecord == null) return false;

            parameterRecord.Categories.Add(new ParameterParameterCategoriesRecord { Parameter = parameterRecord, ParameterCategory = categoryRecord });

            TriggerSignal();
            return true;
        }
        public ParameterRecord SetParameterType(int parameterId, string parameterType)
        {
            ParameterRecord parameterRecord = GetParameter(parameterId);
            if (parameterRecord == null) return null;

            parameterRecord.ParameterType = parameterType;
            TriggerSignal();

            return parameterRecord;
        }
        public void SetParameterValue(int parameterId, string parameterValue)
        {
            ParameterRecord parameterRecord = GetParameter(parameterId);
            if (parameterRecord == null) return;

            parameterRecord.ParameterValueString = parameterValue;
            TriggerSignal();
        }
        public void SetParameterValue(int parameterId, int parameterValue)
        {
            ParameterRecord parameterRecord = GetParameter(parameterId);
            if (parameterRecord == null) return;

            parameterRecord.ParameterValueInt = parameterValue;
            TriggerSignal();
        }
        public void SetParameterValue(int parameterId, double parameterValue)
        {
            ParameterRecord parameterRecord = GetParameter(parameterId);
            if (parameterRecord == null) return;

            parameterRecord.ParameterValueDouble = parameterValue;
            TriggerSignal();
        }
        public void SetParameterValue(int parameterId, bool parameterValue)
        {
            ParameterRecord parameterRecord = GetParameter(parameterId);
            if (parameterRecord == null) return;

            parameterRecord.ParameterValueBool = parameterValue;
            TriggerSignal();
        }
        public void SetParameterValue(int parameterId, DateTime parameterValue)
        {
            ParameterRecord parameterRecord = GetParameter(parameterId);
            if (parameterRecord == null) return;

            parameterRecord.ParameterValueDateTime = parameterValue;
            TriggerSignal();
        }
        public dynamic GetParameterValue(ParameterRecord parameterRecord)
        {
            if (parameterRecord == null) return null;

            CBType p = CBDataTypes.TypeFromString(parameterRecord.ParameterType.ToLower());

            switch (p)
            {
                case CBType.intSetting:
                    return parameterRecord.ParameterValueInt;
                case CBType.doubleSetting:
                    return parameterRecord.ParameterValueDouble;
                case CBType.boolSetting:
                    return parameterRecord.ParameterValueBool;
                case CBType.datetimeSetting:
                    return parameterRecord.ParameterValueDateTime;
                default:
                    return parameterRecord.ParameterValueString;
            }
        }
        public dynamic GetParameterValue(int parameterId)
        {
            ParameterRecord parameterRecord = GetParameter(parameterId);
            return GetParameterValue(parameterRecord);
        }
        public dynamic GetParameterValueForApplication(string applicationName, string parameterName)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationName);
            ParameterRecord parameterRecord = GetParameterForApplication(applicationRecord, parameterName);
            return GetParameterValue(parameterRecord);
        }
        public dynamic GetParameterValueForApplication(int applicationId, string parameterName)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationId);
            ParameterRecord parameterRecord = GetParameterForApplication(applicationRecord, parameterName);
            return GetParameterValue(parameterRecord);
        }
        public bool DeleteParameter(ParameterRecord parameterRecord)
        {
            if (parameterRecord == null) return false;

            _parametersRepository.Delete(parameterRecord);

            TriggerSignal();
            return true;
        }

        public bool DeleteParameter(int parameterId)
        {
            ParameterRecord parameterRecord = GetParameter(parameterId);
            return DeleteParameter(parameterRecord);
        }
        public bool DeleteParameterForApplication(ApplicationRecord applicationRecord, string parameterName)
        {
            if (applicationRecord == null) return false;
            ParameterRecord parameterRecord = GetParameterForApplication(applicationRecord, parameterName);
            return DeleteParameter(parameterRecord);
        }
        public bool DeleteParameterForApplication(int applicationId, string parameterName)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationId);
            ParameterRecord parameterRecord = GetParameterForApplication(applicationRecord, parameterName);
            return DeleteParameter(parameterRecord);
        }
        public bool DeleteParameterForApplication(string applicationName, string parameterName)
        {
            ApplicationRecord applicationRecord = _applicationsService.GetApplication(applicationName);
            ParameterRecord parameterRecord = GetParameterForApplication(applicationRecord, parameterName);
            return DeleteParameter(parameterRecord);
        }
        public bool UpdateParameter(int parameterId, string parameterName, string parameterDescription, IEnumerable<ParameterCategoryRecord> categoryRecords)
        {
            ParameterRecord parameterRecord = GetParameter(parameterId);

            parameterRecord.Name = parameterName;
            parameterRecord.Description = parameterDescription;
            parameterRecord.NormalizedName = parameterName.ToLowerInvariant();
            parameterRecord.Categories.Clear();

            foreach (ParameterCategoryRecord categoryRecord in categoryRecords)
            {
                parameterRecord.Categories.Add(new ParameterParameterCategoriesRecord { Parameter = parameterRecord, ParameterCategory = categoryRecord });
            }
            TriggerSignal();
            return true;
        }
        public IEnumerable<ParameterCategoryRecord> GetCategoriesForParameter(int id)
        {
            var categories = new List<ParameterCategoryRecord>();
            ParameterRecord parameterRecord = GetParameter(id);
            foreach (ParameterParameterCategoriesRecord category in parameterRecord.Categories)
            {
                categories.Add(GetParameterCategory(category.ParameterCategory.Id));
            }
            return categories;
        }

        #endregion
    }
}