using CloudBust.Application.Models;
using Orchard.Security;
using System;
using System.Collections.Generic;

namespace CloudBust.Dashboard.ViewModels
{
    public class GameViewModel
    {
        public IUser User { get; set; }
        public string HostUrl { get; set; }
        public ApplicationGameRecord Game { get; set; }
        public IEnumerable<GameEventRecord> Events { get; set; }
        public int Page { get; set; }
        public bool afterPost { get; set; }
        public Uri Uri { get; set; }
        public dynamic Pager { get; set; }
    }
}