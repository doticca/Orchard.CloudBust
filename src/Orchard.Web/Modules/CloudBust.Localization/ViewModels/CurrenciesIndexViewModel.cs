using System.Collections.Generic;
using CloudBust.Localization.Models;

namespace CloudBust.Localization.ViewModels
{
    public class CurrenciesIndexViewModel
    {
        public IEnumerable<CurrencyRecord> Currencies { get; set; }

        public bool RunsOnDefaultTenant { get; set; }
    }
}