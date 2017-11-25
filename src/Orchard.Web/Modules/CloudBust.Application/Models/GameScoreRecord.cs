using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.Models
{
    public class GameScoreRecord 
    {
        public virtual int Id { get; set; }
        public virtual string ApplicationName { get; set; }
        public virtual string GameName { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual double Agility { get; set; }
        public virtual int AgilityScore { get; set; }
        public virtual string AgilityText { get; set; }
        public virtual double Accuracy { get; set; }
        public virtual int AccuracyScore { get; set; }
        public virtual string AccuracyText { get; set; }
        public virtual double Stability { get; set; }
        public virtual int StabilityScore { get; set; }
        public virtual string StabilityText { get; set; }
        public virtual double Smoothness { get; set; }
        public virtual int SmoothnessScore { get; set; }
        public virtual string SmoothnessText { get; set; }
        public virtual int Attention { get; set; }
        public virtual int Spatial { get; set; }
        public virtual int Executive { get; set; }
        public virtual int Score { get; set; }
    }
}