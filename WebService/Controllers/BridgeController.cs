using Bware.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Mvc;

namespace WebService.Controllers
{
    [Authorize]
    public class BridgeController : ApiController
    {
        Adapters.Interface.IBridgeAdapter _adapter;

        public BridgeController()
        {
            _adapter = new Adapters.Data.BridgeAdapter();
        }

        // GET: api/Bridge
       
        [HttpGet]
        public IEnumerable<Bridge> GetAll()
        {
            var result = _adapter.getAllBridges();
            return result;
        }

        [HttpGet]
      //  [System.Web.Http.Route("Bridge/GetByMiles")]
        public IEnumerable<Bridge> GetByMiles(double lat, double lon, int miles)
        {
            var result = _adapter.getBridgesWithinMiles(lat, lon, miles);
            return result;
        }

        [HttpGet]
        public Bridge GetByLocation(double lat, double lon)
        {
            var result = _adapter.getBridgeByLocation(lat, lon);
            return result;
        }

        [HttpDelete]
        public bool RemoveByLocation(double lat, double lon)
        {
            var result = _adapter.removeBridgeByLocation(lat, lon);
            return result;
        }

        // POST: api/Bridge
        [HttpPost]
        public void Create(Bridge bridge)
        {
            _adapter.saveBridge(bridge);
        }


        [HttpPost]
        public Models.ApiResult UpvoteBridge([FromUri] int bridgeId, [FromUri] string userName)
        {
            var results = _adapter.increaseVote(bridgeId, userName);
            return results;
        }

        // PUT: api/Bridge/Adapters
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Bridge/5
        public void Delete(int id)
        {
        }
    }
}
