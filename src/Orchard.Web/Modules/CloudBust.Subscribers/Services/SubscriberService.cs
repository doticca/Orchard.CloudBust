using System.Collections.Generic;
using System.Linq;
using CloudBust.Subscribers.Models;
using Orchard.Data;

namespace CloudBust.Subscribers.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly IRepository<SubscriberRecord> _subscribersRepository;

        public SubscriberService(
            IRepository<SubscriberRecord> subscribersRepository
        )
        {
            _subscribersRepository = subscribersRepository;
        }

        public SubscriberRecord GetSubscriber(int id)
        {
            try
            {
                var currency = _subscribersRepository.Get(id);
                return currency;
            }
            catch
            {
                return null;
            }
        }

        public SubscriberRecord GetSubscriber(string email)
        {
            try
            {
                var subscriberRecords = from subscriberRecord in _subscribersRepository.Table
                    where subscriberRecord.Email == email
                    select subscriberRecord;
                return subscriberRecords.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<SubscriberRecord> GetSubscribers()
        {
            try
            {
                var subscriberRecords = from subscriberRecord in _subscribersRepository.Table select subscriberRecord;
                return subscriberRecords.ToList();
            }
            catch
            {
                return new List<SubscriberRecord>();
            }
        }

        public SubscriberRecord CreateSubscriber(string email)
        {
            _subscribersRepository.Create(new SubscriberRecord
            {
                Email = email
            });

            var subscriberRecord = GetSubscriber(email);

            return subscriberRecord;
        }       

        public bool UpdateSubscriber(int id, string email)
        {
            var currency = GetSubscriber(id);
            if (currency == null) {
                return false;
            }

            currency.Email = email;

            return true;
        }

        public bool DeleteSubscriber(int id)
        {
            var subscriberRecord = GetSubscriber(id);
            if (subscriberRecord == null) {
                return false;
            }

            _subscribersRepository.Delete(subscriberRecord);

            return true;
        }
    }
}