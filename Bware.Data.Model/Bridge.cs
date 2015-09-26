using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Runtime.Serialization;

namespace Bware.Data.Model
{
    [DataContract]
    public class Bridge
    {
        public DbGeography BridgeLocation { get; set; }

        [DataMember]
        public int BridgeId { get; set; }
        [DataMember]
        public String BIN { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public String FeatureCarried { get; set; }
        [DataMember]
        public String FeatureCrossed { get; set; }
        [DataMember]
        public String LocationDescription { get; set; }
        [DataMember]
        public String State { get; set; }
        [DataMember]
        public String County { get; set; }
        [DataMember]
        public String Township { get; set; }
        [DataMember]
        public String Zip { get; set; }
        [DataMember]
        public String Country { get; set; }
        [DataMember]
        public double? Height { get; set; }
        [DataMember]
        public double? WeightStraight { get; set; }
        [DataMember]
        public double? WeightCombination { get; set; }
        [DataMember]
        public double? WeightDouble { get; set; }
        [DataMember]
        public bool isRposted { get; set; }
        [DataMember]
        public String OtherPosting { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime DateModified { get; set; }
        [DataMember]
        public String UserCreated { get; set; }
        [DataMember]
        public String UserModified { get; set; }
        [DataMember]
        public int NumberOfVotes { get; set; }
        [DataMember]
        public String User1Verified { get; set; }
        [DataMember]
        public String User2Verified { get; set; }
        [DataMember]
        public String User3Verified { get; set; }
        [DataMember]
        public Boolean? User1Reason { get; set; }
        [DataMember]
        public Boolean? User2Reason { get; set; }
        [DataMember]
        public Boolean? User3Reason { get; set; }
        [DataMember]
        public bool isLocked { get; set; }
        [DataMember]
        public bool isActive { get; set; }

    }
}
