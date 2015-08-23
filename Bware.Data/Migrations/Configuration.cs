namespace Bware.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Bware.Data.Model;
    using System.Data.Entity.Spatial;

    internal sealed class Configuration : DbMigrationsConfiguration<Bware.Data.BwareContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Bware.Data.BwareContext context)
        {
            /*****
             * PLEASE DON'T SEED ANY MORE DATA!!!
             * 
            var point1String = string.Format("POINT({0} {1})", (-78.806689).ToString(), 43.098961.ToString());
            var geo1 = DbGeography.FromText(point1String);

            var point2String = string.Format("POINT({0} {1})", (-78.729942).ToString(), 43.123628.ToString());
            var geo2 = DbGeography.FromText(point2String);

            var point3String = string.Format("POINT({0} {1})", (-78.649933).ToString(), 43.191122.ToString());
            var geo3 = DbGeography.FromText(point3String);


            context.Bridges.AddOrUpdate(b => b.Latitude,
                new Bridge() { BIN = "2213560", WeightStraight = 10, isRposted = false, FeatureCarried = "Aiken Rd", FeatureCrossed = "Bull Creek", LocationDescription = "1.9 MI SW of Pendleton Ctr", Township = "Pendleton", County = "Niagara", State = "NY", Zip = "14094", Country = "US", Latitude = 43.098961, Longitude = -78.806689, BridgeLocation = geo1, isLocked = true, NumberOfVotes = 0,  DateCreated = DateTime.UtcNow, DateModified = DateTime.UtcNow },
                new Bridge() { BIN = "3329070", WeightStraight = 15, isRposted = false, FeatureCarried = "East Canal Rd", FeatureCrossed = "Unamed Stream", LocationDescription = "2.4 MI E of Pendleton Center", Township = "Pendleton", County = "Niagara", State = "NY", Zip = "14094", Country = "US", Latitude = 43.123628, Longitude = -78.729942, BridgeLocation = geo2 ,isLocked = true, NumberOfVotes = 0, DateCreated = DateTime.UtcNow, DateModified = DateTime.UtcNow },
                new Bridge() { BIN = "4454110", WeightStraight = 20, isRposted = false, FeatureCarried = "Day Rd", FeatureCrossed = "Erie Canal", LocationDescription = "1 MI E of Lockport", Township = "Lockport", County = "Niagara", State = "NY", Zip = "14094", Country="US", Latitude = 43.191122, Longitude = -78.649933, BridgeLocation = geo3 ,isLocked = false, NumberOfVotes = 0,  DateCreated = DateTime.UtcNow, DateModified = DateTime.UtcNow });

      
            context.SaveChanges();
             *****/
        }
    }
}
