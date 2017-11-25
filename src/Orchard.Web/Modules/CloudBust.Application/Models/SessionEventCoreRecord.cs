using System;
using System.Runtime.Serialization;
using System.Web.Security;
using Orchard.ContentManagement.Records;

namespace CloudBust.Application.Models
{
    public static class StimulaeTypeExtensions
    {
        public static string ToStimulaeString(this StimulaeType evtype)
        {
            switch (evtype)
            {
                case StimulaeType.trigger:
                    return "Trigger";
                case StimulaeType.instruction:
                    return "Instruction";
                case StimulaeType.message:
                    return "Message";
                case StimulaeType.video:
                    return "Video";

                default:
                    return "No Stimuly";
            }
        }
    }

    public enum StimulaeType
    {
        trigger = 1,
        instruction = 2,
        message = 3,
        video = 4
    }

    public static class ObjectTypeExtensions
    {
        public static string ToStimulaeString(this ObjectType evtype)
        {
            switch (evtype)
            {
                case ObjectType.hotspot:
                    return "HotSpot";
                case ObjectType.instruction:
                    return "Instruction";
                case ObjectType.camerafeed:
                    return "Camera Feed";
                case ObjectType.shape:
                    return "Shape";
                case ObjectType.message:
                    return "Message";
                case ObjectType.sound:
                    return "Sound";

                default:
                    return "No Stimuly";
            }
        }
    }

    public enum ObjectType
    {
        hotspot = 1,
        instruction = 2,
        camerafeed = 3,
        shape = 4,
        message = 5,
        sound = 6
    }

    public class SessionEventCoreRecord 
    {
        public virtual int Id { get; set; }
        public virtual SessionRecord SessionRecord { get; set; }
        public virtual GameEventRecord GameEventRecord { get; set; }
        public virtual int Stimulae { get; set; }
        public virtual int Object { get; set; }
        public virtual DateTime? Timestamp { get; set; }

    }
}