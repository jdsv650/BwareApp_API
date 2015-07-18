using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Spatial;

namespace Bware.Data.Model
{
    public class Bridge
    {
        public int BridgeId { get; set; }
        public String BIN { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DbGeography BridgeLocation { get; set; }
        public String FeatureCarried { get; set; }
        public String FeatureCrossed { get; set; }
        public String LocationDescription { get; set; }
        public String State { get; set; }
        public String County { get; set; }
        public String Township { get; set; }
        public String Zip { get; set; }
        public String Country { get; set; }
        public double? Height { get; set; }
        public double? WeightStraight { get; set; }
        public double? WeightCombination { get; set; }
        public double? WeightDouble { get; set; }
        public bool isRposted { get; set; }
        public String OtherPosting { get; set; }
        public int? StateId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public String UserCreated { get; set; }
        public String UserModified { get; set; }
        public int NumberOfVotes { get; set; }
        public bool isLocked { get; set; }
        

    }
}
