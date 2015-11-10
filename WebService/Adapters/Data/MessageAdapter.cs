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
    public class MessageAdapter : Interface.IMessageAdapter
    {
        public IEnumerable<Bware.Data.Model.Message> getMessagesWithinMiles(double lat, double lon, int miles)
        {
            var messages = new List<Message>();
            var db = new BwareContext();

            if (miles <= 0)
            {
                return null;
            }

            var sourcePoint = string.Format("POINT({0} {1})", lon.ToString().Replace(',', '.'), lat.ToString().Replace(',', '.'));
            var currentLocation = DbGeography.PointFromText(sourcePoint, 4326);
            //1609.34 meters per miles - req meters 
            messages = db.Messages.Where(m => m.MessageLocation.Distance(currentLocation) < miles * 1609.34).OrderByDescending(t => t.DateCreated).Take(1000).ToList();

            return messages;
        }


        public Models.ApiResult saveMessage(Message message)
        {
            var db = new BwareContext();
            var result = new Models.ApiResult();
            result.isSuccess = false;
            result.data = null;
            result.multipleData = null;

            if (message == null)
            {
                result.isSuccess = false;
                result.message = "Message data is null";
                return result;
            }

            if (message.UserCreated == null)
            {
                result.isSuccess = false;
                result.message = "User created can not be null";
                return result;
            }

            if (message.DateCreated == null)
            {
                result.isSuccess = false;
                result.message = "Date Created must be included";
                return result;
            }

            // check lat/lon in range
            if (message.Latitude > 90 || message.Latitude < -90 || message.Longitude > 180 || message.Longitude < -180)
            {
                result.isSuccess = false;
                result.message = "Latitude must be between -90 and 90; Longitude must be between -180 and 180";
                return result;
            }

            if (message.Latitude.GetType() != typeof(Double) || message.Longitude.GetType() != typeof(Double))
            {
                result.isSuccess = false;
                result.message = "Latitude and Longitude must be included and of correct data type";
                return result;
            }

            try
            {
                // set DbGeography
                var sourcePoint = string.Format("POINT({0} {1})", message.Longitude.ToString().Replace(',', '.'), message.Latitude.ToString().Replace(',', '.'));
                var messageLocation = DbGeography.PointFromText(sourcePoint, 4326);
                message.MessageLocation = messageLocation;

                // Store date created/modified as UTC
                message.DateCreated = message.DateCreated.ToUniversalTime();

                db.Messages.Add(message);

                var numReturned = db.SaveChanges();
                if (numReturned == 1)
                {
                    result.isSuccess = true;
                    result.message = "Message saved successfully";
                    return result;
                }
                else
                {
                    result.isSuccess = false;
                    result.message = "Error saving message record to database";
                    return result;
                }
            }
            catch
            {
                result.isSuccess = false;
                result.message = "Error saving message record to database";
                return result;
            }
        }
    }
}