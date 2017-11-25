using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.OData
{
    [DataContract]
    public class uGameScore
    {
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public double Agility { get; set; }
        [DataMember]
        public int AgilityScore { get; set; }
        [DataMember]
        public string AgilityText { get; set; }
        [DataMember]
        public double Accuracy { get; set; }
        [DataMember]
        public int AccuracyScore { get; set; }
        [DataMember]
        public string AccuracyText { get; set; }
        [DataMember]
        public double Stability { get; set; }
        [DataMember]
        public int StabilityScore { get; set; }
        [DataMember]
        public string StabilityText { get; set; }
        [DataMember]
        public double Smoothness { get; set; }
        [DataMember]
        public int SmoothnessScore { get; set; }
        [DataMember]
        public string SmoothnessText { get; set; }
        [DataMember]
        public int Attention { get; set; }
        [DataMember]
        public int Spatial { get; set; }
        [DataMember]
        public int Executive { get; set; }
        [DataMember]
        public int Score { get; set; }
    }
}