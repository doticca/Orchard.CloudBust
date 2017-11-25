using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;
using System.Collections.Generic;

namespace CloudBust.Application.Models
{
    public class ApplicationRecord
    {
        public ApplicationRecord()
        {
            Categories = new List<ApplicationApplicationCategoriesRecord>();
            Games = new List<ApplicationApplicationGamesRecord>();
            StartParameters = new List<ApplicationStartParameterRecord>();
            EndParameters = new List<ApplicationEndParameterRecord>();
            ParametersCategories = new List<ApplicationParameterCategoryRecord>();
            DataTables = new List<ApplicationApplicationDataTablesRecord>();
        }
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string NormalizedName { get; set; }
        public virtual string AppKey { get; set; }
        public virtual string AppSecret { get; set; }
        public virtual string fbAppKey { get; set; }
        public virtual string fbAppSecret { get; set; }
        public virtual string owner { get; set; }
        public virtual DateTime? CreatedUtc { get; set; }
        public virtual DateTime? ModifiedUtc { get; set; }
        public virtual DateTime? LastLoginUtc { get; set; }
        public virtual bool internalEmail { get; set; }
        public virtual string senderEmail { get; set; }
        public virtual string mailServer { get; set; }
        public virtual int mailPort { get; set; }
        public virtual string mailUsername { get; set; }
        public virtual string mailPassword{get;set;}
        public virtual string BundleIdentifier { get; set; }
        public virtual string BundleIdentifierOSX { get; set; }
        public virtual string BundleIdentifierTvOS { get; set; }
        public virtual string BundleIdentifierWatch { get; set; }
        public virtual int ServerBuild { get; set; }
        public virtual int MinimumClientBuild { get; set; }
        public virtual string UpdateUrl { get; set; }
        public virtual string UpdateUrlOSX { get; set; }
        public virtual string UpdateUrlTvOS { get; set; }
        public virtual string UpdateUrlWatch { get; set; }
        public virtual string UpdateUrlDeveloper { get; set; }
        public virtual bool blogPerUser { get; set; }
        public virtual bool blogSecurity { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<ApplicationApplicationCategoriesRecord> Categories { get; set; }
        [CascadeAllDeleteOrphan]
        public virtual IList<ApplicationApplicationGamesRecord> Games { get; set; }
        [CascadeAllDeleteOrphan]
        public virtual IList<ApplicationStartParameterRecord> StartParameters { get; set; }
        [CascadeAllDeleteOrphan]
        public virtual IList<ApplicationEndParameterRecord> EndParameters { get; set; }
        [CascadeAllDeleteOrphan]
        public virtual IList<ApplicationParameterCategoryRecord> ParametersCategories { get; set; }
        [CascadeAllDeleteOrphan]
        public virtual IList<ApplicationApplicationDataTablesRecord> DataTables { get; set; }
    }
}