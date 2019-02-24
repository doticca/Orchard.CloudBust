using System.Collections.Generic;
using CloudBust.Localization.Models;

namespace CloudBust.Localization.ViewModels
{
    public class CountriesIndexViewModel
    {
        public IEnumerable<CountryInfo> Countries { get; set; }

        public bool RunsOnDefaultTenant { get; set; }
    }
}