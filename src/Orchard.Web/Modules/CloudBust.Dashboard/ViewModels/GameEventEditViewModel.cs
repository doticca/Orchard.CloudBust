using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudBust.Application.Models;
using Orchard.Security.Permissions;
using Orchard.Security;

namespace CloudBust.Dashboard.ViewModels
{
    public class GameEventEditViewModel  {
        public IUser User { get; set; }
        public string GameName { get; set; }
        public string GameKey { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public GameEventType GameEventType { get; set; }
    }
}
