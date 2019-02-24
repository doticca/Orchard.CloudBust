using System.Collections.Generic;

namespace CloudBust.LogViewer.ViewModels
{
    public class LogFilesVm
    {
        public List<string> Files { get; set; }
        public string FileContent { get; set; }
        public string FileName { get; set; }
    }
}