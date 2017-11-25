using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.Models
{
    public static class GameEventTypeExtensions
    {
        public static string ToEventString(this GameEventType evtype)
        {
            switch(evtype)
            {
                case GameEventType.directive:
                    return "Directive";
                case GameEventType.distraction:
                    return "Distraction";
                case GameEventType.hook:
                    return "Hook";
                case GameEventType.instruction:
                    return "Instruction";
                case GameEventType.motion:
                    return "Motion";
                case GameEventType.nosense:
                    return "No Sense";
                case GameEventType.prompt:
                    return "Prompt";
                case GameEventType.recall:
                    return "Recall";
                case GameEventType.reflex:
                    return "Reflex";
                case GameEventType.time:
                    return "Time";
                default:
                    return "No Sense";
            }
        }
    }
    public enum GameEventType{
        motion=1,
        time=2,
        reflex=3,
        instruction=4,
        directive=5,
        prompt=6,
        hook=7,
        recall=8,
        distraction=9,
        nosense=0
    }

    public class GameEventRecord 
    {
        public virtual int Id { get; set; }
        public virtual ApplicationGameRecord ApplicationGameRecord { get; set; }
        public virtual string Name { get; set; }
        public virtual string NormalizedEventName { get; set; }
        public virtual string Description { get; set; }
        public virtual int BusinessProcess { get; set; }
        public virtual int GameEventType { get; set; }
    }
}