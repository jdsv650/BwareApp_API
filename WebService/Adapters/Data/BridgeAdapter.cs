using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bware.Data.Model;
using Bware.Data;
using System.Web.Http;
using System.Data.Entity.Spatial;
using System.Data.Entity.Core.Objects;
using WebService.Models;

namespace WebService.Adapters.Data
{
    public class BridgeAdapter : Adapters.Interface.IBridgeAdapter
    {
        const int numHoursBetweenCreate = 1;  // how often should a user be able to create a new bridge? once a week? roles? trust?


        public IEnumerable<BridgeCountResult> getBridgeCountStates()
        {
            var db = new BwareContext();
            var bridgeCountResults = new List<Models.BridgeCountResult>();
           /***     
            SELECT Count(*) as BridgeCount, [State]
            FROM [dbo].[Bridges]
            GROUP BY [State]
           ***/

            bridgeCountResults = (from b in db.Bridges
                 orderby b.State
                 group b by b.State into stateGrp
                 select new BridgeCountResult() { NumberOfBridges = stateGrp.Count(), State = stateGrp.Key }).OrderByDescending(b => b.NumberOfBridges).Take(5).ToList();

            return bridgeCountResults;
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
            //1609.34 meters per miles - req meters  -- Add isActive check
            bridges = db.Bridges.Where(b => b.BridgeLocation.Distance(currentLocation) < miles * 1609.34 && b.isActive == true).ToList(); 

            return bridges;
        }

        public Bridge getBridgeByLocation(double lat, double lon)
        {
            var bridge = new Bridge();
            var db = new BwareContext();

           // if (lat > 90 || lat < -90 || lon > 180 || lon < -180)   {  }
            // may need to give a bit here instead of =   -- Add isActive check
            bridge = db.Bridges.Where(b => b.Latitude <= lat + 0.0001 && b.Latitude >= lat - 0.0001 && b.Longitude <= lon + 0.0001 && b.Longitude >= lon - 0.0001 && b.isActive == true).FirstOrDefault();

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
            {   // -- Add isActive check
                bridges = db.Bridges.Where(b => b.Country == country && b.State == state && b.County == county && b.isActive == true).ToList();
            }
            else // town info passed in so match that too
            {   // -- Add isActive check
                bridges = db.Bridges.Where(b => b.Country == country && b.State == state && b.County == county && b.Township.Contains(town) && b.isActive == true).ToList();
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

        private bool allowEdit()
        {
            return true;
        }

        // if at least 2/3 are marked as edit don't allow remove (only edit)
        private bool allowDelete(Bridge bridge)
        {
            if (bridge.User1Reason == true && bridge.User2Reason == true && bridge.User3Reason == true ||
                bridge.User1Reason == true && bridge.User2Reason == true && bridge.User3Reason == false ||
                bridge.User1Reason == true && bridge.User2Reason == false && bridge.User3Reason == true ||
                bridge.User1Reason == false && bridge.User2Reason == true && bridge.User3Reason == true)
            {
                return false;
            }

            return true;
        }

        public Models.ApiResult removeBridgeByLocation(double lat, double lon)
        {
            var bridge = new Bridge();
            var db = new BwareContext();
            var result = new Models.ApiResult();
            result.isSuccess = false;
            result.data = null;
            result.multipleData = null;

            try
            {
                bridge = getBridgeByLocation(lat, lon);
                db.Bridges.Attach(bridge);

                if (bridge == null)
                {  // couldn't find bridge
                    result.message = "Bridge not found";
                    return result;
                }
                else
                {
                    // Check to see if bridge can be deleted (marked isActive assigned to false) first!!!!!!
                    if (bridge.User1Reason == null || bridge.User2Reason == null || bridge.User3Reason == null)
                    {
                        result.message = "Bridge must have at least 3 down votes to be removed";
                        return result;
                    }
                    // false == 0 == mark for delete -- true == 1 == mark for edit
                    // if at least 2/3 are marked as edit don't allow remove (only edit)
                    if (allowDelete(bridge) == false)
                    {
                        result.message = "Bridge must have at least 2 down votes of correct type to be removed";
                        return result;
                    }

                    // Delete only one per week???????

                    // Mark isActive as false for soft delete
                    bridge.isActive = false;

                    var saveOk = 1 == db.SaveChanges();
                    if (saveOk == true)
                    {
                        result.isSuccess = true;
                        result.message = "Bridge marked as inactive";
                        return result;
                    }

                    result.message = "Error removing bridge";
                    return result;
                }
            }
            catch 
            {
                result.message = "Error removing bridge";
                return result;
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

            if (bridge.Latitude.GetType() != typeof(Double) || bridge.Longitude.GetType() != typeof(Double))
            {
                result.isSuccess = false;
                result.message = "Latitude and Longitude must be included and of correct data type";
                return result;
            }

            try
            {
                // 5 minutes for testing!!!!!!
                var theBridges = new List<Bridge>();
                theBridges = null;
                var isAlreadyCreated = false;

                var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
                var oneWeekAgo = DateTime.UtcNow.AddDays(-7);

                //************************** change to oneWeekAgo before app gets released *************************************************
                isAlreadyCreated = db.Bridges.Where(b => b.UserCreated == bridge.UserCreated && b.DateCreated > fiveMinutesAgo).ToList().Any(); 
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
                
            bridge.NumberOfVotes = 0;    // new bridge
            bridge.isLocked = false;

            try
            {
                // set DbGeography
                var sourcePoint = string.Format("POINT({0} {1})", bridge.Longitude.ToString().Replace(',', '.'), bridge.Latitude.ToString().Replace(',', '.'));
                var bridgeLocation = DbGeography.PointFromText(sourcePoint, 4326);
                bridge.BridgeLocation = bridgeLocation;

                // Store date created/modified as UTC
                bridge.DateCreated = bridge.DateCreated.ToUniversalTime();
                bridge.DateModified = bridge.DateModified.ToUniversalTime();

                // Mark new bridge as isActive
                bridge.isActive = true;

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



        public Models.ApiResult saveBridgeBatch(Bridge bridge)
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

            if (bridge.Latitude.GetType() != typeof(Double) || bridge.Longitude.GetType() != typeof(Double))
            {
                result.isSuccess = false;
                result.message = "Latitude and Longitude must be included and of correct data type";
                return result;
            }

           
            bridge.NumberOfVotes = 0;    // new bridge
            bridge.isLocked = false;

            try
            {
                // set DbGeography
                var sourcePoint = string.Format("POINT({0} {1})", bridge.Longitude.ToString().Replace(',', '.'), bridge.Latitude.ToString().Replace(',', '.'));
                var bridgeLocation = DbGeography.PointFromText(sourcePoint, 4326);
                bridge.BridgeLocation = bridgeLocation;

                // Store date created/modified as UTC
                bridge.DateCreated = bridge.DateCreated.ToUniversalTime();
                bridge.DateModified = bridge.DateModified.ToUniversalTime();

                // Mark new bridge as isActive
                bridge.isActive = true;

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


        public Models.ApiResult updateBridge(Bridge bridge)
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

            if (bridge.DateModified == null)
            {
                result.isSuccess = false;
                result.message = "Date Modified must be included";
                return result;
            }

            if (bridge.UserModified == null)
            {
                result.isSuccess = false;
                result.message = "User Modified must be included";
                return result;
            }

            // check lat/lon in range
            if (bridge.Latitude > 90 || bridge.Latitude < -90 || bridge.Longitude > 180 || bridge.Longitude < -180)
            {
                result.isSuccess = false;
                result.message = "Latitude must be between -90 and 90; Longitude must be between -180 and 180";
                return result;
            }

            if (bridge.Latitude.GetType() != typeof(Double) || bridge.Longitude.GetType() != typeof(Double))
            {
                result.isSuccess = false;
                result.message = "Latitude and Longitude must be included and of correct data type";
                return result;
            }

            try
            {
                var theBridge = db.Bridges.Where(b => b.Latitude <= bridge.Latitude + 0.0001 && b.Latitude >= bridge.Latitude - 0.0001 && b.Longitude <= bridge.Longitude + 0.0001 && b.Longitude >= bridge.Longitude - 0.0001 && b.isActive == true).FirstOrDefault();

                if (theBridge == null)
                {
                    result.isSuccess = false;
                    result.message = "Bridge not found";
                    return result;
                }

                // Check to see if all 3 down vote slots filled before allowing edit
                if (theBridge.User1Reason == null || theBridge.User2Reason == null || theBridge.User3Reason == null)
                {
                    result.message = "Bridge must have at least 3 down votes to be edited";
                    return result;
                }
            
                // Edit only one per week???????

                db.Bridges.Attach(theBridge);

                // track who made last change and when 
                theBridge.DateModified = bridge.DateModified.ToUniversalTime();
                theBridge.UserModified = bridge.UserModified;

                theBridge.NumberOfVotes = 0;    // reset user input fields
                theBridge.isLocked = false;
                theBridge.User1Reason = null;
                theBridge.User1Verified = null;
                theBridge.User2Reason = null;
                theBridge.User2Verified = null;
                theBridge.User3Reason = null;
                theBridge.User3Verified = null;

                theBridge.Country = bridge.Country;  // the location - restriction fields
                theBridge.County = bridge.County;
                theBridge.FeatureCarried = bridge.FeatureCarried;
                theBridge.FeatureCrossed = bridge.FeatureCrossed;
                theBridge.Height = bridge.Height;
                theBridge.isRposted = bridge.isRposted;
                theBridge.LocationDescription = bridge.LocationDescription;
                theBridge.OtherPosting = bridge.OtherPosting;
                theBridge.State = bridge.State;
                theBridge.Township = bridge.Township;
                theBridge.WeightCombination = bridge.WeightCombination;
                theBridge.WeightDouble = bridge.WeightDouble;
                theBridge.WeightStraight_TriAxle = bridge.WeightStraight_TriAxle;
                theBridge.WeightStraight = bridge.WeightStraight;
                theBridge.Zip = bridge.Zip;

                var updateCount = db.SaveChanges();
                if (updateCount == 1)
                {
                    result.isSuccess = true;
                    result.message = "Bridge updated successfully";
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
            bridge = db.Bridges.Where(b => b.BridgeId == id && b.isActive == true).FirstOrDefault();
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

            theBridge = db.Bridges.Where(b => b.BridgeId == bridgeId && b.isActive == true).FirstOrDefault();
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


        public Models.ApiResult decreaseVote(int bridgeId, string userName, bool isEdit)
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

            theBridge = db.Bridges.Where(b => b.BridgeId == bridgeId && b.isActive == true).FirstOrDefault();
            if (theBridge == null)
            {
                result.isSuccess = false;
                result.message = "Bridge not found";
                return result;
            }

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
                theBridge.User1Reason = isEdit;
            }
            else if (theBridge.User2Verified == null || theBridge.User2Verified == "")
            {
                theBridge.User2Verified = userName;
                theBridge.User2Reason = isEdit;
            }
            else if (theBridge.User3Verified == null || theBridge.User3Verified == "")
            {
                theBridge.User3Verified = userName;
                theBridge.User3Reason = isEdit;
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