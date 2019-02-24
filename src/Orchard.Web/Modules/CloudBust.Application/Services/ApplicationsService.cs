using System;
using System.Collections.Generic;
using System.Linq;
using CloudBust.Application.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Security;
using Orchard.Services;

namespace CloudBust.Application.Services
{
    public class ApplicationsService : IApplicationsService
    {
        private readonly IRepository<ApplicationRecord> _applicationsRepository;

        private readonly IRepository<ApplicationCategoryRecord> _categoriesRepository;
        private readonly IClock _clock;
        private readonly IContentManager _contentManager;
        private readonly IDataNotificationService _datanotificationService;
        private readonly IOrchardServices _orchardServices;
        private readonly IRepository<ParameterRecord> _parametersRepository;
        private readonly IRepository<UserRoleRecord> _userrolesRepository;

        public ApplicationsService(IContentManager contentManager, IOrchardServices orchardServices, IDataNotificationService datanotificationService, IRepository<ApplicationCategoryRecord> categoriesRepository, IRepository<ApplicationRecord> applicationsRepository, IRepository<ParameterRecord> parametersRepository, IRepository<UserRoleRecord> userrolesRepository, IClock clock)
        {
            _orchardServices = orchardServices;
            _contentManager = contentManager;
            _datanotificationService = datanotificationService;
            _categoriesRepository = categoriesRepository;
            _applicationsRepository = applicationsRepository;
            _parametersRepository = parametersRepository;
            _userrolesRepository = userrolesRepository;
            _clock = clock;
        }

        // APPLICATION CATEGORIES
        #region Application Categories

        public ApplicationCategoryRecord GetCategory(int Id)
        {
            try
            {
                var category = _categoriesRepository.Get(Id);
                return category;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<ApplicationCategoryRecord> GetCategories()
        {
            try
            {
                var categories = from category in _categoriesRepository.Table select category;
                return categories.ToList();
            }
            catch
            {
                return new List<ApplicationCategoryRecord>();
            }
        }

        public ApplicationCategoryRecord GetCategoryByName(string categoryName)
        {
            return _categoriesRepository.Get(x => x.Name == categoryName);
        }

        public ApplicationCategoryRecord CreateCategory(string categoryName, string categoryDescription)
        {
            var r = new ApplicationCategoryRecord();
            r.Name = categoryName;
            r.Description = categoryDescription;
            r.NormalizedName = categoryName.ToLowerInvariant();

            _categoriesRepository.Create(r);

            _datanotificationService.CategoryUpdated();
            return GetCategoryByName(r.Name);
        }

        public bool DeleteCategory(int Id)
        {
            var r = GetCategory(Id);
            if (r == null) return false;
            _categoriesRepository.Delete(r);

            _datanotificationService.CategoryUpdated();
            return true;
        }

        public bool UpdateCategory(int id, string categoryName, string categoryDescription)
        {
            var applicationcategoryRecord = GetCategory(id);

            applicationcategoryRecord.Name = categoryName;
            applicationcategoryRecord.Description = categoryDescription;
            applicationcategoryRecord.NormalizedName = categoryName.ToLowerInvariant();

            _datanotificationService.CategoryUpdated();
            return true;
        }

        #endregion

        // APPLICATION USER ROLES
        #region Application User Roles

        public UserRoleRecord GetUserRole(int Id)
        {
            try
            {
                var userrole = _userrolesRepository.Get(Id);
                return userrole;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<UserRoleRecord> GetUserRoles(ApplicationRecord applicationRecord)
        {
            if (applicationRecord == null)
                return new List<UserRoleRecord>();

            try
            {
                var userroles = from userrole in _userrolesRepository.Table where userrole.ApplicationRecord.Id == applicationRecord.Id select userrole;
                return userroles.ToList();
            }
            catch
            {
                return new List<UserRoleRecord>();
            }
        }

        public IEnumerable<UserRoleRecord> GetUserRoles(string applicationName)
        {
            var applicationRecord = GetApplicationByName(applicationName);
            return GetUserRoles(applicationRecord);
        }

        public IEnumerable<UserRoleRecord> GetUserRoles(int applicationId)
        {
            var applicationRecord = GetApplication(applicationId);
            return GetUserRoles(applicationRecord);
        }

        public UserRoleRecord GetUserRoleByName(string applicationName, string userroleName)
        {
            return _userrolesRepository.Get(x => x.Name == userroleName && x.ApplicationRecord.Name == applicationName);
        }

        public UserRoleRecord GetUserRoleByName(int applicationId, string userroleName)
        {
            return _userrolesRepository.Get(x => x.Name == userroleName && x.ApplicationRecord.Id == applicationId);
        }

        public UserRoleRecord GetUserRoleByName(ApplicationRecord applicationRecord, string userroleName)
        {
            return _userrolesRepository.Get(x => x.Name.ToLowerInvariant() == userroleName.ToLowerInvariant() && x.ApplicationRecord.Id == applicationRecord.Id);
        }

        public UserRoleRecord CreateUserRole(ApplicationRecord applicationRecord, string userroleName, string userroleDescription)
        {
            var r = new UserRoleRecord();
            r.Name = userroleName;
            r.ApplicationRecord = applicationRecord;
            r.Description = userroleDescription;
            r.NormalizedRoleName = userroleName.ToLowerInvariant();
            r.IsDefaultRole = false;

            _userrolesRepository.Create(r);

            _datanotificationService.ApplicationUpdated(r.ApplicationRecord);
            return GetUserRoleByName(applicationRecord, r.Name);
        }

        public UserRoleRecord CreateUserRole(string applicationName, string userroleName, string userroleDescription)
        {
            var applicationRecord = GetApplicationByName(applicationName);
            return CreateUserRole(applicationRecord, userroleName, userroleDescription);
        }

        public UserRoleRecord CreateUserRole(int applicationId, string userroleName, string userroleDescription)
        {
            var applicationRecord = GetApplication(applicationId);
            return CreateUserRole(applicationRecord, userroleName, userroleDescription);
        }

        public void SetDefaultRole(ApplicationRecord applicationRecord, string userroleName)
        {
            //UserRoleRecord userrole = GetUserRoleByName(applicationRecord, userroleName);
            foreach (var userrole in GetUserRoles(applicationRecord)) userrole.IsDefaultRole = userrole.Name != userroleName ? false : true;
            _datanotificationService.ApplicationUpdated(applicationRecord);
        }

        public void SetDefaultRole(ApplicationRecord applicationRecord, int Id)
        {
            //UserRoleRecord userrole = GetUserRoleByName(applicationRecord, userroleName);
            foreach (var userrole in GetUserRoles(applicationRecord)) userrole.IsDefaultRole = userrole.Id != Id ? false : true;
            _datanotificationService.ApplicationUpdated(applicationRecord);
        }

        public UserRoleRecord GetDefaultRole(ApplicationRecord applicationRecord)
        {
            var defaultrole = (from userrole in _userrolesRepository.Table where userrole.ApplicationRecord.Id == applicationRecord.Id && userrole.IsDefaultRole select userrole).FirstOrDefault();
            if (defaultrole == null)
            {
                var userrole = GetUserRoleByName(applicationRecord, "User");
                if (userrole == null) userrole = CreateUserRole(applicationRecord, "User", "Default Users");
                SetDefaultRole(applicationRecord, "User");
                defaultrole = userrole;
            }

            return defaultrole;
        }

        public bool DeleteUserRole(int Id)
        {
            var r = GetUserRole(Id);
            if (r == null) return false;
            if (r.IsDefaultRole) return false;
            _userrolesRepository.Delete(r);
            return true;
        }

        public bool DeleteUserRole(ApplicationRecord applicationRecord, string userroleName)
        {
            var r = GetUserRoleByName(applicationRecord, userroleName);
            if (r == null) return false;
            _userrolesRepository.Delete(r);
            _datanotificationService.ApplicationUpdated(applicationRecord);
            return true;
        }

        public bool DeleteUserRole(string applicationName, string userroleName)
        {
            var applicationRecord = GetApplicationByName(applicationName);
            if (applicationRecord == null)
                return false;

            var r = GetUserRoleByName(applicationName, userroleName);
            if (r == null) return false;
            _userrolesRepository.Delete(r);
            _datanotificationService.ApplicationUpdated(applicationRecord);
            return true;
        }

        public bool DeleteUserRole(int applicationId, string userroleName)
        {
            var applicationRecord = GetApplication(applicationId);
            if (applicationRecord == null)
                return false;
            var r = GetUserRoleByName(applicationId, userroleName);
            if (r == null) return false;
            _userrolesRepository.Delete(r);
            _datanotificationService.ApplicationUpdated(applicationRecord);
            return true;
        }

        private bool UpdateUserRole(UserRoleRecord userroleRecord, string userroleName, string userroleDescription)
        {
            if (userroleRecord == null)
                return false;

            userroleRecord.Name = userroleName;
            userroleRecord.Description = userroleDescription;
            userroleRecord.NormalizedRoleName = userroleName.ToLowerInvariant();

            return true;
        }

        public bool UpdateUserRole(int Id, string userroleName, string userroleDescription)
        {
            var userroleRecord = GetUserRole(Id);

            return UpdateUserRole(userroleRecord, userroleName, userroleDescription);
        }

        #endregion

        // APPLICATIONS
        #region Applications

        public string GetApplicationProtocol(int Id)
        {
            try
            {
                var group = _applicationsRepository.Get(Id);
                var appkey = group.AppKey;
                var protocol = "dot" + appkey + "://";
                return protocol;
            }
            catch
            {
                return string.Empty;
            }
        }

        public ApplicationRecord GetApplication(int Id)
        {
            try
            {
                var group = _applicationsRepository.Get(Id);
                return group;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<ApplicationRecord> GetApplications()
        {
            try
            {
                var modules = from module in _applicationsRepository.Table select module;
                return modules.ToList();
            }
            catch
            {
                return new List<ApplicationRecord>();
            }
        }

        public IEnumerable<ApplicationRecord> GetUserApplications(IUser user)
        {
            try
            {
                var modules = from module in _applicationsRepository.Table where module.owner == user.UserName select module;
                return modules.ToList();
            }
            catch
            {
                return new List<ApplicationRecord>();
            }
        }

        public ApplicationRecord GetApplicationByName(string name)
        {
            return _applicationsRepository.Get(x => x.Name == name);
        }

        public ApplicationRecord GetApplication(string applicationName)
        {
            return _applicationsRepository.Get(x => x.Name == applicationName);
        }

        public ApplicationRecord GetApplicationByKey(string key)
        {
            return _applicationsRepository.Get(x => x.AppKey == key);
        }

        public ApplicationRecord CreateApplication(string applicationName, string applicationDescription, string owner)
        {
            var utcNow = _clock.UtcNow;
            _applicationsRepository.Create(new ApplicationRecord
                                           {
                                               Name = applicationName,
                                               Description = applicationDescription,
                                               owner = owner,
                                               CreatedUtc = utcNow,
                                               ModifiedUtc = utcNow,
                                               LastLoginUtc = utcNow
                                           });

            var app = GetApplicationByName(applicationName);
            if (app != null)
                CreateUserRole(app, "User", "Default Users");

            _datanotificationService.ApplicationUpdated(app);
            return app;
        }

        public bool CreateCategoryForApplication(string applicationName, int categoryId)
        {
            var categoryRecord = GetCategory(categoryId);
            if (categoryRecord == null) return false;

            var moduleRecord = GetApplicationByName(applicationName);
            if (moduleRecord == null) return false;

            moduleRecord.Categories.Add(new ApplicationApplicationCategoriesRecord {Application = moduleRecord, ApplicationCategory = categoryRecord});

            _datanotificationService.ApplicationUpdated(moduleRecord);
            return true;
        }
        //public bool CreateStartParameterForApplication(string applicationName, int parameterId)
        //{
        //    ParameterRecord parameterRecord = GetParameter(parameterId);
        //    if (parameterRecord == null) return false;

        //    ApplicationRecord moduleRecord = GetApplicationByName(applicationName);
        //    if (moduleRecord == null) return false;

        //    moduleRecord.StartParameters.Add(new ApplicationStartParameterRecord { Application = moduleRecord, Parameter = parameterRecord });

        //    ApplicationUpdated(moduleRecord);
        //    return true;
        //}
        //public bool DeleteStartParameterFromApplication(string applicationName, int parameterId)
        //{
        //    ApplicationRecord moduleRecord = GetApplicationByName(applicationName);
        //    if (moduleRecord == null) return false;

        //    foreach(ApplicationStartParameterRecord startparameterRecord in moduleRecord.StartParameters)
        //    {
        //        if (startparameterRecord.Parameter.Id == parameterId)
        //        {
        //            moduleRecord.StartParameters.Remove(startparameterRecord);
        //            break;
        //        }
        //    }

        //    ApplicationUpdated(moduleRecord);
        //    return true;
        //}
        //public bool CreateEndParameterForApplication(string applicationName, int parameterId)
        //{
        //    ParameterRecord parameterRecord = GetParameter(parameterId);
        //    if (parameterRecord == null) return false;

        //    ApplicationRecord moduleRecord = GetApplicationByName(applicationName);
        //    if (moduleRecord == null) return false;

        //    moduleRecord.EndParameters.Add(new ApplicationEndParameterRecord { Application = moduleRecord, Parameter = parameterRecord });

        //    ApplicationUpdated(moduleRecord);
        //    return true;
        //}
        //public bool DeleteEndParameterFromApplication(string applicationName, int parameterId)
        //{
        //    ApplicationRecord moduleRecord = GetApplicationByName(applicationName);
        //    if (moduleRecord == null) return false;

        //    foreach (ApplicationEndParameterRecord endparameterRecord in moduleRecord.EndParameters)
        //    {
        //        if (endparameterRecord.Parameter.Id == parameterId)
        //        {
        //            moduleRecord.EndParameters.Remove(endparameterRecord);
        //            break;
        //        }
        //    }

        //    ApplicationUpdated(moduleRecord);
        //    return true;

        //}
        private bool DeleteApplication(int Id)
        {
            var r = GetApplication(Id);
            if (r == null) return false;

            _applicationsRepository.Delete(r);
            return true;
        }

        public bool UpdateApplication(int id, string applicationName, string applicationDescription, string owner, IEnumerable<ApplicationCategoryRecord> categoryRecords)
        {
            var applicationRecord = GetApplication(id);
            var utcNow = _clock.UtcNow;
            applicationRecord.Name = applicationName;
            applicationRecord.Description = applicationDescription;
            applicationRecord.owner = owner;
            applicationRecord.Categories.Clear();
            if (applicationRecord.CreatedUtc == null)
            {
                applicationRecord.CreatedUtc = utcNow;
                applicationRecord.LastLoginUtc = utcNow;
            }

            applicationRecord.ModifiedUtc = utcNow;

            foreach (var categoryRecord in categoryRecords) applicationRecord.Categories.Add(new ApplicationApplicationCategoriesRecord {Application = applicationRecord, ApplicationCategory = categoryRecord});

            _datanotificationService.ApplicationUpdated(applicationRecord);
            return true;
        }

        public bool UpdateApplication(int id, string applicationName, string applicationDescription)
        {
            var applicationRecord = GetApplication(id);
            var utcNow = _clock.UtcNow;
            applicationRecord.Name = applicationName;
            applicationRecord.Description = applicationDescription;
            applicationRecord.Categories.Clear();
            if (applicationRecord.CreatedUtc == null)
            {
                applicationRecord.CreatedUtc = utcNow;
                applicationRecord.LastLoginUtc = utcNow;
            }

            _datanotificationService.ApplicationUpdated(applicationRecord);
            return true;
        }

        public bool UpdateApplicationFacebook(int id, string fbAppKey, string fbAppSecret)
        {
            var applicationRecord = GetApplication(id);

            applicationRecord.fbAppKey = fbAppKey;
            applicationRecord.fbAppSecret = fbAppSecret;

            _datanotificationService.ApplicationUpdated(applicationRecord);
            return true;
        }

        public bool UpdateApplicationGameCenter(int id, string bundleIdentifier, string bundleIdentifierOSX, string bundleIdentifierTvOS, string bundleIdentifierWatch)
        {
            var applicationRecord = GetApplication(id);

            applicationRecord.BundleIdentifier = bundleIdentifier;
            applicationRecord.BundleIdentifierOSX = bundleIdentifierOSX;
            applicationRecord.BundleIdentifierTvOS = bundleIdentifierTvOS;
            applicationRecord.BundleIdentifierWatch = bundleIdentifierWatch;

            _datanotificationService.ApplicationUpdated(applicationRecord);
            return true;
        }

        public bool UpdateApplicationAppStore(int id, int serverBuild, int minimumClientBuild, string updateUrl, string updateUrlOSX, string updateUrlTvOS, string updateUrlWatch, string updateUrlDeveloper)
        {
            var applicationRecord = GetApplication(id);
            var utcNow = _clock.UtcNow;

            applicationRecord.ServerBuild = serverBuild;
            applicationRecord.MinimumClientBuild = minimumClientBuild;

            applicationRecord.UpdateUrl = updateUrl;
            applicationRecord.UpdateUrlOSX = updateUrlOSX;
            applicationRecord.UpdateUrlTvOS = updateUrlTvOS;
            applicationRecord.UpdateUrlWatch = updateUrlWatch;
            applicationRecord.UpdateUrlDeveloper = updateUrlDeveloper;

            _datanotificationService.ApplicationUpdated(applicationRecord);
            _datanotificationService.ServerUpdated();
            return true;
        }

        public bool UpdateApplicationSmtp(int id, bool internalEmail, string senderEmail, string mailServer, int mailPort, string mailUsername, string mailPassword)
        {
            var applicationRecord = GetApplication(id);
            var utcNow = _clock.UtcNow;
            applicationRecord.internalEmail = internalEmail;
            applicationRecord.senderEmail = senderEmail;
            applicationRecord.mailServer = mailServer;
            applicationRecord.mailPort = mailPort;
            applicationRecord.mailUsername = mailUsername;
            applicationRecord.mailPassword = mailPassword;

            _datanotificationService.ApplicationUpdated(applicationRecord);
            return true;
        }

        public bool UpdateApplicationBlogs(int id, bool blogPerUser, bool blogSecurity)
        {
            var applicationRecord = GetApplication(id);
            var utcNow = _clock.UtcNow;
            applicationRecord.blogPerUser = blogPerUser;
            applicationRecord.blogSecurity = blogSecurity;

            _datanotificationService.ApplicationUpdated(applicationRecord);
            return true;
        }

        public IEnumerable<ApplicationCategoryRecord> GetCategoriesForApplication(int id)
        {
            var categories = new List<ApplicationCategoryRecord>();
            var applicationRecord = GetApplication(id);
            foreach (var category in applicationRecord.Categories) categories.Add(GetCategory(category.ApplicationCategory.Id));
            return categories;
        }

        //public IEnumerable<ParameterRecord> GetStartParametersForApplication(int id)
        //{
        //    var parameters = new List<ParameterRecord>();
        //    ApplicationRecord applicationRecord = GetApplication(id);
        //    foreach (ApplicationStartParameterRecord parameter in applicationRecord.StartParameters)
        //    {
        //        parameters.Add(GetParameter(parameter.Parameter.Id));
        //    }
        //    return parameters;
        //}
        //public IEnumerable<ParameterRecord> GetEndParametersForApplication(int id)
        //{
        //    var parameters = new List<ParameterRecord>();
        //    ApplicationRecord applicationRecord = GetApplication(id);
        //    foreach (ApplicationEndParameterRecord parameter in applicationRecord.EndParameters)
        //    {
        //        parameters.Add(GetParameter(parameter.Parameter.Id));
        //    }
        //    return parameters;
        //}
        public bool CreateKeysForApplication(int id)
        {
            var applicationRecord = GetApplication(id);
            applicationRecord.AppKey = CBDataTypes.GenerateIdentifier(16, true);
            applicationRecord.AppSecret = CBDataTypes.GenerateIdentifier(32);

            _datanotificationService.ApplicationUpdated(applicationRecord);
            return true;
        }

        #endregion
    }
}