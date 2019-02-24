using System.Collections.Generic;
using CloudBust.Subscribers.Models;

namespace CloudBust.Subscribers.ViewModels
{
    public class SubscribersIndexViewModel
    {
        public IEnumerable<SubscriberRecord> Subscribers { get; set; }
    }
}