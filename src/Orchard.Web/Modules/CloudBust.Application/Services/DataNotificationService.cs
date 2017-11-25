using CloudBust.Application.Models;
using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Services;

namespace CloudBust.Application.Services
{
    public class DataNotificationService : IDataNotificationService
    {
        private readonly IClock _clock;
        private readonly ISignals _signals;
        private readonly IOrchardServices _orchardServices;

        public DataNotificationService(
                                             IClock clock
                                            ,ISignals signals
                                            ,IOrchardServices orchardServices
                                      )
        {
            _clock = clock;
            _signals = signals;
            _orchardServices = orchardServices;
        }

        private void TriggerApplicationSignal()
        {
            _signals.Trigger(CBSignals.SignalApplications);
        }
        private void TriggerScoresSignal()
        {
            _signals.Trigger(CBSignals.SignalScores);
        }
        private void TriggerCategoriesSignal()
        {
            _signals.Trigger(CBSignals.SignalCategories);
        }
        private void TriggerServerUpdated()
        {
            _signals.Trigger(CBSignals.SignalServer);
        }
        public void GameUpdated(string gameName, ApplicationGameRecord gameRecord = null)
        {
            var utcNow = _clock.UtcNow;
            if(gameRecord!=null)
                gameRecord.ModifiedUtc = utcNow;
            TriggerApplicationSignal();
        }
        public void ApplicationUpdated(ApplicationRecord app)
        {
            var utcNow = _clock.UtcNow;
            app.ModifiedUtc = utcNow;
            var settings = _orchardServices.WorkContext.CurrentSite.As<ApplicationSettingsPart>();
            if (settings != null) // is web app?
            {
                if (settings.ApplicationKey == app.AppKey)
                    settings.ApplicationName = app.Name;
            }
            TriggerApplicationSignal();
        }
        public void ScoreUpdated(string gameName)
        {
            TriggerScoresSignal();
        }
        public void CategoryUpdated()
        {
            TriggerCategoriesSignal();
        }
        public void ServerUpdated()
        {
            TriggerServerUpdated();
        }
    }
}