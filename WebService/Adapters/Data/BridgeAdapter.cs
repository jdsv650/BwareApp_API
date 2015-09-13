using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bware.Data.Model;
using Bware.Data;
using System.Web.Http;
using System.Data.Entity.Spatial;
using System.Data.Entity.Core.Objects;

namespace WebService.Adapters.Data
{
    public class BridgeAdapter : Adapters.Interface.IBridgeAdapter
    {
        const int numHoursBetweenCreate = 1;  // how often should a user be able to create a new bridge? once a week? roles? trust?

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

        public Models.ApiResult getByInfo(string country, string state, string county, string town)
        {
            var bridges = new List<Bridge>();
            var db = new BwareContext();
            var result = new Models.ApiResult();
            result.data = null;

            if (country == null || country == "")
            {
                result.isSuccess = false;
                result.message = "Country field must contain a value";
                return result;
            }

            if (state == null || state == "")
            {
                result.isSuccess = false;
                result.message = "State field must contain a value";
                return result;
            }

            if (county == null || county == "")
            {
                result.isSuccess = false;
                result.message = "County field must contain a value";
                return result;
            }

            if (town == null || town == "")
            {
                bridges = db.Bridges.Where(b => b.Country == country && b.State == state && b.County == county).ToList();
            }
            else // town info passed in so match that too
            {
                bridges = db.Bridges.Where(b => b.Country == country && b.State == state && b.County == county && b.Township.Contains(town)).ToList();
            }

            if (bridges == null)
            {
                result.isSuccess = false;
                result.message = "No bridges found for given area";
                return result;
            }

            result.isSuccess = true;
            result.message = "Success";
            result.multipleData = bridges;

            return result;
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

        public Models.ApiResult saveBridge(Bridge bridge)
        {
            var db = new BwareContext();
            var result = new Models.ApiResult();
            result.isSuccess = false;
            result.data = null;
            result.multipleData = null;

            if (bridge == null)
            {
                result.isSuccess = false;
                result.message = "Bridge data is null";
                return result;
            }

            if (bridge.UserCreated == null)
            {
                result.isSuccess = false;
                result.message = "User created can not be null";
                return result;
            }

            try
            {
                // 5 minutes for testing!!!!!!
                var theBridges = new List<Bridge>();
                theBridges = null;
                var isAlreadyCreated = false;

                theBridges = db.Bridges.Where(b => b.UserCreated == bridge.UserCreated).ToList();
                
                foreach (var b in theBridges)
                {
                    if (b.DateCreated.AddMinutes(5) > DateTime.Now)
                    {
                        isAlreadyCreated = true;
                        break;
                    }
                }

                var now = DateTime.UtcNow;
             
                //isAlreadyCreated = db.Bridges.AsEnumerable().Where(b => b.UserCreated == bridge.UserCreated && b.DateCreated.AddMinutes(5).CompareTo(now) < 0 ).ToList().Any();  //EntityFunctions.DiffMinutes(now, b.DateCreated) > 5)

              //  theBridges = db.Bridges.Where(b => b.UserCreated == bridge.UserCreated && EntityFunctions.DiffMinutes(now, b.DateCreated) < 5).ToList();  

                if (isAlreadyCreated)
                {
                    result.isSuccess = false;
                    result.message = "User must wait specified time between creating bridges";
                    return result;
                }
            }
            catch
            {
                result.isSuccess = false;
                result.message = "Error determining when user last created bridge. Please try again";
                return result;
            }
                
                
                /**
            if (1 == 2) // last time user created was less than numHoursBetween Create
            {
                result.isSuccess = false;
                result.message = "User can only create a bridge every " + numHoursBetweenCreate + "hours";
                return result;
            }
   * 
   * */

            bridge.NumberOfVotes = 0;    // new bridge
            bridge.isLocked = false;

            if (bridge.Latitude.GetType() != typeof(Double) || bridge.Longitude.GetType() != typeof(Double) )
            {
                result.isSuccess = false;
                result.message = "Latitude and Longitude must be included and of correct data type";
                return result;
            }

            // could just make it now 
            if (bridge.DateCreated == null)
            {
                result.isSuccess = false;
                result.message = "Date Created must be included";
                return result;
            }

            // could just make it now
            if (bridge.DateModified == null)
            {
                result.isSuccess = false;
                result.message = "Date Modified must be included";
                return result;
            }

            // check lat/lon in range
            if (bridge.Latitude > 90 || bridge.Latitude < -90 || bridge.Longitude > 180 || bridge.Longitude < -180)
            {
                result.isSuccess = false;
                result.message = "Latitude must be between -90 and 90; Longitude must be between -180 and 180";
                return result;
            }

            try
            {
                // set DbGeography
                var sourcePoint = string.Format("POINT({0} {1})", bridge.Longitude.ToString().Replace(',', '.'), bridge.Latitude.ToString().Replace(',', '.'));
                var bridgeLocation = DbGeography.PointFromText(sourcePoint, 4326);
                bridge.BridgeLocation = bridgeLocation;

                // Store date created/modified as UTC
                bridge.DateCreated = bridge.DateCreated.ToUniversalTime();
                bridge.DateModified = bridge.DateModified.ToUniversalTime();

                db.Bridges.Add(bridge);
                var numReturned = db.SaveChanges();
                if (numReturned == 1)
                {
                    result.isSuccess = true;
                    result.message = "Bridge saved successfully";
                    return result;
                }
                else
                {
                    result.isSuccess = false;
                    result.message = "Error saving bridge record to database";
                    return result;
                }
            }
            catch
            {
                result.isSuccess = false;
                result.message = "Error saving bridge record to database";
                return result;
            }
        }

        private Bridge getBridgeById(int id)
        {
            var bridge = new Bridge();
            var db = new BwareContext();
            bridge = db.Bridges.Where(b => b.BridgeId == id).FirstOrDefault();
            return bridge;
        }

        public Models.ApiResult increaseVote(int bridgeId, string userName)
        {
            var theBridge = new Bridge();
            var db = new BwareContext();
            var result = new Models.ApiResult();
            result.data = null;

            if (userName == "" || userName == null)
            {
                result.isSuccess = false;
                result.message = "Username is null or empty string";
                return result;
            }

            theBridge = db.Bridges.Where(b => b.BridgeId == bridgeId).FirstOrDefault();
            if (theBridge == null)
            { 
                result.isSuccess = false;
                result.message = "Bridge not found";
                return result;
            }

            // Creator can't upvote a bridge
            if (theBridge.UserCreated == userName) 
             { 
                result.isSuccess = false;
                result.message = "Error: User Created Bridge";
                return result;
            }
        
            // User can only upvote once
            if ((theBridge.User1Verified != null && theBridge.User1Verified == userName) ||
                (theBridge.User2Verified != null && theBridge.User2Verified == userName) ||
                (theBridge.User3Verified != null && theBridge.User3Verified == userName))
             { 
                result.isSuccess = false;
                result.message = "Error: User can only upvote once";
                return result;
             }
        
            // Can only upvote 3 times
            if (theBridge.NumberOfVotes >= 3)
             { 
                theBridge.NumberOfVotes = 3;
                result.isSuccess = false;
                result.message = "Error: Upvote slots full";
                return result;
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

            theBridge.NumberOfVotes = theBridge.NumberOfVotes + 1;

            try
            {
                if (db.SaveChanges() == 1)
                {
                    result.isSuccess = true;
                    result.message = "Upvote Succcessfully Recorded";
                }
                else
                {
                    result.isSuccess = false;
                    result.message = "Error Saving Upvote";
                }
                return result;
            }
            catch
            {
                result.isSuccess = false;
                result.message = "Error Saving Upvote";
                return result;
            }
        }


        public Models.ApiResult decreaseVote(int bridgeId, string userName)
        {
            var theBridge = new Bridge();
            var db = new BwareContext();
            var result = new Models.ApiResult();
            result.data = null;

            if (userName == "" || userName == null)
            {
                result.isSuccess = false;
                result.message = "Username is null or empty string";
                return result;
            }

            theBridge = db.Bridges.Where(b => b.BridgeId == bridgeId).FirstOrDefault();
            if (theBridge == null)
            {
                result.isSuccess = false;
                result.message = "Bridge not found";
                return result;
            }

            // Creator can downvote a bridge if he made a mistake
            /***
            if (theBridge.UserCreated == userName)
            {
                result.isSuccess = false;
                result.message = "Error: User Created Bridge";
                return result;
            }
             ***/

            // User can only down vote once
            if ((theBridge.User1Verified != null && theBridge.User1Verified == userName) ||
                (theBridge.User2Verified != null && theBridge.User2Verified == userName) ||
                (theBridge.User3Verified != null && theBridge.User3Verified == userName))
            {
                result.isSuccess = false;
                result.message = "Error: User can only down vote a bridge once";
                return result;
            }

            // Can only down vote max of 3 times
            if (theBridge.NumberOfVotes >= 3)
            {
                theBridge.NumberOfVotes = 3;
                result.isSuccess = false;
                result.message = "Error: Down vote slots full";
                return result;
            }

            // if down vote slot free grab it
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

            theBridge.NumberOfVotes = theBridge.NumberOfVotes + 1;

            try
            {
                if (db.SaveChanges() == 1)
                {
                    result.isSuccess = true;
                    result.message = "Down Vote Succcessfully Recorded";
                }
                else
                {
                    result.isSuccess = false;
                    result.message = "Error Saving Down Vote";
                }
                return result;
            }
            catch
            {
                result.isSuccess = false;
                result.message = "Error Saving Down Vote";
                return result;
            }

        }
    }

}