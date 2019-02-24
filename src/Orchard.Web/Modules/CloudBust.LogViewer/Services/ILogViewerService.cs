using System.Collections.Generic;
using System.IO;
using Orchard;

namespace CloudBust.LogViewer.Services {
    public interface ILogViewerService : IDependency {
        List<string> GetLogFiles();

        string ReadLogFile(string name);

        Stream GetFileStream(string name);

        bool DeleteLogFile(string name);
    }
}