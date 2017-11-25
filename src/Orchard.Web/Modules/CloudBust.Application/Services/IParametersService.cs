using CloudBust.Application.Models;
using Orchard;
using System;
using System.Collections.Generic;

namespace CloudBust.Application.Services
{
    public interface IParametersService : IDependency
    {
        #region parameter categories
        ParameterCategoryRecord GetParameterCategory(int parameterCategoryId);
        IEnumerable<ParameterCategoryRecord> GetParameterCategoriesForApplication(ApplicationRecord applicationRecord);
        IEnumerable<ParameterCategoryRecord> GetParameterCategoriesForApplication(string applicationName);
        IEnumerable<ParameterCategoryRecord> GetParameterCategoriesForApplication(int applicationId);
        ParameterCategoryRecord GetParameterCategoryForApplication(ApplicationRecord applicationRecord, string parameterCategoryName);
        ParameterCategoryRecord GetParameterCategoryForApplication(int applicationId, string parameterCategoryName);
        ParameterCategoryRecord GetParameterCategoryForApplication(string applicationName, string parameterCategoryName);
        ParameterCategoryRecord CreateParameterCategoryForApplication(ApplicationRecord applicationRecord, string parameterCategoryName, string parameterCategoryDescription);
        ParameterCategoryRecord CreateParameterCategoryForApplication(string applicationName, string parameterCategoryName, string parameterCategoryDescription);
        ParameterCategoryRecord CreateParameterCategoryForApplication(int applicationId, string parameterCategoryName, string parameterCategoryDescription);
        bool DeleteParameterCategory(int parameterCategoryId);
        bool UpdateParameterCategory(int parameterCategoryId, string categoryName, string categoryDescription);
        #endregion

        #region parameters
        ParameterRecord GetParameter(int parameterId);
        IEnumerable<ParameterRecord> GetParametersForApplication(ApplicationRecord applicationRecord);
        IEnumerable<ParameterRecord> GetParametersForApplication(string applicationName);
        IEnumerable<ParameterRecord> GetParametersForApplication(int applicationId);
        ParameterRecord GetParameterForApplication(ApplicationRecord applicationRecord, string parameterName);
        ParameterRecord GetParameterForApplication(string applicationName, string parameterName);
        ParameterRecord GetParameterForApplication(int applicationId, string parameterName);
        ParameterRecord CreateParameterForApplication(ApplicationRecord applicationRecord, string parameterName, string parameterDescription);
        ParameterRecord CreateParameterForApplication(string applicationName, string parameterName, string parameterDescription);
        ParameterRecord CreateParameterForApplication(int applicationId, string parameterName, string parameterDescription);
        bool SetCategoryForParameter(int parameterId, int parameterCategoryId);
        bool DeleteParameter(ParameterRecord parameterRecord);
        bool DeleteParameter(int parameterId);
        bool DeleteParameterForApplication(ApplicationRecord applicationRecord, string parameterName);
        bool DeleteParameterForApplication(int applicationId, string parameterName);
        bool DeleteParameterForApplication(string applicationName, string parameterName);
        bool UpdateParameter(int parameterId, string parameterName, string parameterDescription, IEnumerable<ParameterCategoryRecord> parameterCategoryRecords);
        IEnumerable<ParameterCategoryRecord> GetCategoriesForParameter(int parameterId);
        ParameterRecord SetParameterType(int parameterId, string parameterType);
        void SetParameterValue(int parameterId, DateTime parameterValue);
        void SetParameterValue(int parameterId, bool parameterValue);
        void SetParameterValue(int parameterId, double parameterValue);
        void SetParameterValue(int parameterId, int parameterValue);
        void SetParameterValue(int parameterId, string parameterValue);
        dynamic GetParameterValue(ParameterRecord parameterRecord);
        dynamic GetParameterValue(int parameterId);
        dynamic GetParameterValueForApplication(string applicationName, string parameterName);
        dynamic GetParameterValueForApplication(int applicationId, string parameterName);
        #endregion
    }
}