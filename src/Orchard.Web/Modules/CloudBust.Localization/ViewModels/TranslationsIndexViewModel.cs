using System.Collections.Generic;
using CloudBust.Localization.Models;

namespace CloudBust.Localization.ViewModels
{
    public class TranslationsIndexViewModel
    {
        public IEnumerable<TranslationRecord> Translations { get; set; }

        public bool RunsOnDefaultTenant { get; set; }
    }
}