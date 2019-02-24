using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using CloudBust.LogViewer.CustomUtils;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.UI.Notify;

namespace CloudBust.LogViewer.Services
{
    public class LogViewerService : ILogViewerService
    {
        #region Attributes
        private readonly INotifier _notifier;

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }
        #endregion

        #region Constructor
        public LogViewerService(INotifier notifier)
        {
            _notifier = notifier;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }
        #endregion

        #region Getting data
        public List<string> GetLogFiles()
        {
            string folder = HttpContext.Current.Server.MapPath(LogConstants.LogsFolder);
            return Directory.EnumerateFiles(folder, "*.*").Select(Path.GetFileName).ToList();
        }

        public string ReadLogFile(string name)
        {
            if (String.IsNullOrEmpty(name))
                return null;

            string folder = HttpContext.Current.Server.MapPath(LogConstants.LogsFolder);
            string filePath = Path.Combine(folder, name);

            if (!File.Exists(filePath))
                return null;

            try {
                using (var sr = new StreamReader(filePath, Encoding.GetEncoding(1250))) {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception e){
                _notifier.Error(T("Error while reading file '{0}'.", name));
                Logger.Error(e.Message);
                return null;
            }
        }

        public Stream GetFileStream(string name)
        {
            if (String.IsNullOrEmpty(name))
                return null;

            string folder = System.Web.HttpContext.Current.Server.MapPath(LogConstants.LogsFolder);
            string filePath = Path.Combine(folder, name);

            if (!File.Exists(filePath))
                return null;

            /* Nacteni souboru */
            try
            {
                return new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception e)
            {
                _notifier.Error(T("Error while loading file '{0}'.", name));
                Logger.Error(e.Message);
                return null;
            }
        }
        #endregion

        #region Updating data
        public bool DeleteLogFile(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            string folder = System.Web.HttpContext.Current.Server.MapPath(LogConstants.LogsFolder);
            string filePath = Path.Combine(folder, name);

            if (!File.Exists(filePath))
                return false;

            try
            {
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                _notifier.Error(T("Unable to delete file '{0}'.", name));
            }

            return true;
        }
        #endregion
    }
}
