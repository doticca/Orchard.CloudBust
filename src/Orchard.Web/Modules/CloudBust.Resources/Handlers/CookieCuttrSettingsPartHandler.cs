using CloudBust.Resources.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Resources.Handlers
{
    [OrchardFeature("CloudBust.Resources.CookieCuttr")]
    public class CookieCuttrSettingsPartHandler : ContentHandler
    {
        public CookieCuttrSettingsPartHandler(IRepository<CookieCuttrSettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(new ActivatingFilter<CookieCuttrSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));

            OnInitializing<CookieCuttrSettingsPart>((context, part) =>
            {
                    part.cookieNotificationLocationBottom= false;           
                    part.cookieAnalytics= true;                             
                    part.cookieAnalyticsMessage = CookieCuttrMigrations.cookieanalyticsmsg;
                    part.cookiePolicyLink= string.Empty;              
                    part.showCookieDeclineButton= false;              
                    part.showCookieAcceptButton= true;               
                    part.showCookieResetButton= false;                
                    part.cookieOverlayEnabled= false;                 
                    part.cookieMessage = CookieCuttrMigrations.cookiemsg;
                    part.cookieWhatAreTheyLink= CookieCuttrMigrations.whatarecookieslink;          
                    part.cookieCutter= false;
                    part.cookieErrorMessage = CookieCuttrMigrations.errormsg;             
                    part.cookieDisable= string.Empty;                  
                    part.cookieAcceptButtonText= CookieCuttrMigrations.acceptmsg;
                    part.cookieDeclineButtonText = CookieCuttrMigrations.declinemsg;
                    part.cookieResetButtonText = CookieCuttrMigrations.resetmsg;
                    part.cookieWhatAreLinkText = CookieCuttrMigrations.whataremsg;          
                    part.cookiePolicyPage= false;                      
                    part.cookiePolicyPageMessage= string.Empty;        
                    part.cookieDiscreetLink= false;                    
                    part.cookieDiscreetLinkText= string.Empty;         
                    part.cookieDiscreetPosition= "topleft";            
                    part.cookieDomain= string.Empty;
                    part.cookieDiscreetReset = false;
            });
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Cookies")));
        }
    }
}