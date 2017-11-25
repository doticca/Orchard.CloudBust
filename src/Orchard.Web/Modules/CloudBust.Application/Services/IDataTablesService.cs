using CloudBust.Application.Models;
using Orchard;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Application.Services
{

    public interface IDataTablesService : IDependency
    {
        ApplicationDataTableRecord GetDataTable(int Id);
        IEnumerable<ApplicationDataTableRecord> GetDataTables();
        IEnumerable<ApplicationDataTableRecord> GetApplicationDataTables(ApplicationRecord applicationRecord);
        IEnumerable<ApplicationDataTableRecord> GetApplicationDataTables(int applicationId);
        ApplicationDataTableRecord GetDataTableByName(string datatableName);
        ApplicationDataTableRecord CreateDataTable(string datatableName, string datatableDescription);
        bool UpdateDataTable(int id, string datatableName, string datatableDescription);
        bool CreateDataTableForApplication(string applicationName, int datatableId);
        bool RemoveDataTableFromApplication(string applicationName, int datatableId);
        IEnumerable<ApplicationDataTableRecord> GetNonApplicationDataTables(IUser user, ApplicationRecord applicationRecord);
    }
}