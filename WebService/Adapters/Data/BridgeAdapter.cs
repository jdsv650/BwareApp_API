using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bware.Data.Model;
using Bware.Data;
using System.Web.Http;
using System.Data.Entity.Spatial;

namespace WebService.Adapters.Data
{
    public class BridgeAdapter : Adapters.Interface.IBridgeAdapter
    {
        public IEnumerable<Bridge> getAllBridges()
        {
            var bridges = new List<Bridge>();
            var db = new BwareContext();

            bridges = db.Bridges.ToList();
            return bridges;
        }


        public IEnumerable<Bridge> getBridgesWithinMiles(double lat, double lon, int miles)
        {
            var bridges = new List<Bridge>();
            var db = new BwareContext();

            if (miles <= 0)
            {
                return null;
            }

            var sourcePoint = string.Format("POINT({0} {1})", lon.ToString().Replace(',','.'), lat.ToString().Replace(',','.'));
            var currentLocation = DbGeography.PointFromText(sourcePoint, 4326);
            //1609.34 meters per miles - req meters
            bridges = db.Bridges.Where(b => b.BridgeLocation.Distance(currentLocation) < miles * 1609.34).ToList(); 

            return bridges;
        }

        public Bridge getBridgeByLocation(double lat, double lon)
        {
            var bridge = new Bridge();
            var db = new BwareContext();

           // if (lat > 90 || lat < -90 || lon > 180 || lon < -180)   {  }
            // may need to give a bit here instead of = 
            bridge = db.Bridges.Where(b => b.Latitude <= lat + 0.0001 && b.Latitude >= lat - 0.0001 && b.Longitude <= lon + 0.0001 && b.Longitude >= lon - 0.0001).FirstOrDefault();

            return bridge;
        }



        public bool removeBridgeByLocation(double lat, double lon)
        {
            var bridge = new Bridge();
            var db = new BwareContext();

            try
            {
                bridge = getBridgeByLocation(lat, lon);

                if (bridge != null)
                {
                    db.Bridges.Remove(bridge);
                    return 1 == db.SaveChanges();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool saveBridge(Bridge bridge)
        {
            var db = new BwareContext();

            if (bridge == null) return false;

            try
            {
                // check lat/lon in range
                if (bridge.Latitude > 90 || bridge.Latitude < -90 || bridge.Longitude > 180 || bridge.Longitude < -180)
                {
                    return false;
                }

                // set DbGeography
                var sourcePoint = string.Format("POINT({0} {1})", bridge.Longitude.ToString().Replace(',', '.'), bridge.Latitude.ToString().Replace(',', '.'));
                var bridgeLocation = DbGeography.PointFromText(sourcePoint, 4326);
                bridge.BridgeLocation = bridgeLocation;

                db.Bridges.Add(bridge);
                return 1 == db.SaveChanges();
            }
            catch
            {
                return false;
            }
        }

        private Bridge getBridgeById(int id)
        {
            var bridge = new Bridge();
            var db = new BwareContext();
            bridge = db.Bridges.Where(b => b.BridgeId == id).FirstOrDefault();
            return bridge;
        }

        public bool increaseVote(int id, string userName)
        {
            var theBridge = new Bridge();
            var db = new BwareContext();
            theBridge = db.Bridges.Where(b => b.BridgeId == id).FirstOrDefault();

            if (theBridge == null) return false;

            // Creator can't upvote a bridge
            if (theBridge.UserCreated == userName) return false;  
        
            // User can only upvote once
            if ((theBridge.User1Verified != null && theBridge.User1Verified == userName) ||
                (theBridge.User2Verified != null && theBridge.User2Verified == userName) ||
                (theBridge.User3Verified != null && theBridge.User3Verified == userName))
            {
                return false; 
            }

            // Can only upvote 3 times
            if (theBridge.NumberOfVotes >= 3)
            {
                theBridge.NumberOfVotes = 3;
                return false;
            }

            // if upvote slot free grab it
            if (theBridge.User1Verified == null || theBridge.User1Verified == "")
            {
                theBridge.User1Verified = userName;
            }
            else if (theBridge.User2Verified == null || theBridge.User2Verified == "")
            {
                theBridge.User2Verified = userName;
            }
            else if (theBridge.User3Verified == null || theBridge.User3Verified == "")
            {
                theBridge.User3Verified = userName;
            }

            theBridge.NumberOfVotes = theBridge.NumberOfVotes++;

            try
            {
                return db.SaveChanges() == 1;
            }
            catch
            {
                return false;
            }
        }

    }

}