using Orchard;
using System.Collections.Generic;
using CloudBust.Subscribers.Models;

namespace CloudBust.Subscribers.Services
{
    public interface ISubscriberService : IDependency
    {
        SubscriberRecord GetSubscriber(int id);
        SubscriberRecord GetSubscriber(string email);
        IEnumerable<SubscriberRecord> GetSubscribers();
        SubscriberRecord CreateSubscriber(string email);
        bool UpdateSubscriber(int id, string email);
        bool DeleteSubscriber(int id);
    }
}