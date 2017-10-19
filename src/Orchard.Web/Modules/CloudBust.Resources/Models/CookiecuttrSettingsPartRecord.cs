using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.CookieCuttr")]
    public class CookieCuttrSettingsPartRecord : ContentPartRecord
    {
        public virtual string cookieDiscreetLinkText { get; set; }
        public virtual string cookieDiscreetPosition { get; set; }
        public virtual string cookieDomain { get; set; }
        public virtual bool cookieDiscreetLink { get; set; }
        public virtual bool cookieDiscreetReset { get; set; }
        public virtual string cookiePolicyPageMessage { get; set; }
        public virtual bool cookiePolicyPage { get; set; }
        public virtual string cookieErrorMessage { get; set; }
        public virtual string cookieDisable { get; set; }
        public virtual string cookieAcceptButtonText { get; set; }
        public virtual string cookieDeclineButtonText { get; set; }
        public virtual string cookieResetButtonText { get; set; }
        public virtual string cookieWhatAreLinkText { get; set; }
        public virtual bool cookieAnalytics { get; set; }
        public virtual string cookieAnalyticsMessage { get; set; }
        public virtual string cookiePolicyLink { get; set; }
        public virtual bool cookieNotificationLocationBottom { get; set; }
        public virtual bool showCookieDeclineButton { get; set; }
        public virtual bool showCookieAcceptButton { get; set; }
        public virtual bool showCookieResetButton { get; set; }
        public virtual bool cookieOverlayEnabled { get; set; }
        public virtual string cookieMessage { get; set; }
        public virtual string cookieWhatAreTheyLink { get; set; }
        public virtual bool cookieCutter { get; set; }
    }
}