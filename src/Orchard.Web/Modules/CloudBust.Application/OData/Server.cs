using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace CloudBust.Application.OData
{
    [DataContract]
    public class Server
    {
        [DataMember]
        public string Type { get; private set; }
        public DateTime ServerDateTime { get; private set; }
        [DataMember(Name = "ServerDateTime")]
        private string ServerDateTimeSerialized { get; set; }
        [DataMember]
        public int ServerVersioningData { get; private set; }
        [DataMember]
        public int ServerVersioningMinimumClientData { get; private set; }

        public const string OType = "Server";
        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            try
            {
                this.ServerDateTimeSerialized = this.ServerDateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.ServerDateTimeSerialized = default(DateTime).ToString("o");
            }
        }
        [OnDeserialized]
        void OnDeserializing(StreamingContext context)
        {
            if (this.ServerDateTimeSerialized == null)
            {
                this.ServerDateTime = default(DateTime);
            }
            else
            {
                this.ServerDateTime = DateTime.ParseExact(this.ServerDateTimeSerialized, "yyyy-MM-dd'T'HH:mm:ssZ", CultureInfo.InvariantCulture);
            }
        }
        public Server(DateTime serverDateTime, int serverVersioningData, int serverVersioningMinimumClientData)
        {
            this.Type = OType;
            this.ServerDateTime = serverDateTime;
            this.ServerVersioningData = serverVersioningData;
            this.ServerVersioningMinimumClientData = serverVersioningMinimumClientData;
        }
    }
}