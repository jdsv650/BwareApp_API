using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebService.Models
{
    [DataContract]
    public class BridgeCountResult
    {
        [DataMember]
        public String State { get; set; }

        [DataMember]
        public int NumberOfBridges { get; set; }
    }
}