using CloudBust.Blogs.Models;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CloudBust.Blogs.OData
{
    [DataContract]
    public class Viewers
    {
        [DataMember]
        public double Count { get; private set; }
        [DataMember]
        public double Result { get; private set; }
        [DataMember]
        public double Vote { get; private set; }

        public Viewers(ViewersPart p)
        {
            Result = p.ResultValue;
            Vote = p.UserRating;
            Count = p.Count;
        }
    }
}