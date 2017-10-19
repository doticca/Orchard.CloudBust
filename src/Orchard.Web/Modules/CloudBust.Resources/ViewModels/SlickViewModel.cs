using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Models;
using System.Collections.Generic;

namespace CloudBust.Resources.ViewModels
{
    [OrchardFeature("CloudBust.Resources.Slick")]
    public class SlickViewModel
    {
        public string FolderPath { get; set; }
        public IEnumerable<MediaFile>  MediaFiles { get; set; }
    }
}