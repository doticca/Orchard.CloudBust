using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.CookieCuttr")]
    public class CookieCuttrSettingsPart : ContentPart<CookieCuttrSettingsPartRecord>
    {
        public string cookieDiscreetLinkText { get { return Record.cookieDiscreetLinkText; } set { Record.cookieDiscreetLinkText = value; } }
        public string cookieDiscreetPosition { get { return Record.cookieDiscreetPosition; } set { Record.cookieDiscreetPosition = value; } }
        public string cookieDomain { get { return Record.cookieDomain; } set { Record.cookieDomain = value; } }
        public bool cookieDiscreetLink { get { return Record.cookieDiscreetLink; } set { Record.cookieDiscreetLink = value; } }
        public bool cookieDiscreetReset { get { return Record.cookieDiscreetReset; } set { Record.cookieDiscreetReset = value; } }
        public string cookiePolicyPageMessage { get { return Record.cookiePolicyPageMessage; } set { Record.cookiePolicyPageMessage = value; } }
        public bool cookiePolicyPage { get { return Record.cookiePolicyPage; } set { Record.cookiePolicyPage = value; } }
        public string cookieErrorMessage { get { return Record.cookieErrorMessage; } set { Record.cookieErrorMessage = value; } }
        public string cookieDisable { get { return Record.cookieDisable; } set { Record.cookieDisable = value; } }
        public string cookieAcceptButtonText { get { return Record.cookieAcceptButtonText; } set { Record.cookieAcceptButtonText = value; } }
        public string cookieDeclineButtonText { get { return Record.cookieDeclineButtonText; } set { Record.cookieDeclineButtonText = value; } }
        public string cookieResetButtonText { get { return Record.cookieResetButtonText; } set { Record.cookieResetButtonText = value; } }
        public string cookieWhatAreLinkText { get { return Record.cookieWhatAreLinkText; } set { Record.cookieWhatAreLinkText = value; } }
        public bool cookieAnalytics { get { return Record.cookieAnalytics; } set { Record.cookieAnalytics = value; } }
        public string cookieAnalyticsMessage { get { return Record.cookieAnalyticsMessage; } set { Record.cookieAnalyticsMessage = value; } }
        public string cookiePolicyLink { get { return Record.cookiePolicyLink; } set { Record.cookiePolicyLink = value; } }
        public bool cookieNotificationLocationBottom { get { return Record.cookieNotificationLocationBottom; } set { Record.cookieNotificationLocationBottom = value; } }
        public bool showCookieDeclineButton { get { return Record.showCookieDeclineButton; } set { Record.showCookieDeclineButton = value; } }
        public bool showCookieAcceptButton { get { return Record.showCookieAcceptButton; } set { Record.showCookieAcceptButton = value; } }
        public bool showCookieResetButton { get { return Record.showCookieResetButton; } set { Record.showCookieResetButton = value; } }
        public bool cookieOverlayEnabled { get { return Record.cookieOverlayEnabled; } set { Record.cookieOverlayEnabled = value; } }
        public string cookieMessage { get { return Record.cookieMessage; } set { Record.cookieMessage = value; } }
        public string cookieWhatAreTheyLink { get { return Record.cookieWhatAreTheyLink; } set { Record.cookieWhatAreTheyLink = value; } }
        public bool cookieCutter { get { return Record.cookieCutter; } set { Record.cookieCutter = value; } }
    }
}