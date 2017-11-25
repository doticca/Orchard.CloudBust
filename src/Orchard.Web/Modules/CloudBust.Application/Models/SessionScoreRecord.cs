using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.Models
{
    public class SessionScoreRecord 
    {
        public virtual int Id { get; set; }
        public virtual SessionRecord Session { get; set; }
        public virtual double ShortTermMemory { get; set; }
        public virtual double InvoluntaryAttention { get; set; }
        public virtual double MotorLearningStability { get; set; }
        public virtual double MotorLearningAgility { get; set; }
        public virtual double MotorLearningSmoothness { get; set; }
        public virtual double MotorReflexScore { get; set; }
        public virtual double MotorLearning { get; set; }
    }
}