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
       /* don't allow get all
        [HttpGet]
        public IEnumerable<Bridge> GetAll()
        {
            var result = _adapter.getAllBridges();
            return result;
        }
        */

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

        [HttpGet]
        public Models.ApiResult GetByInfo(string country, string state, string county, string town="")
        {
            var result = _adapter.getByInfo(country, state, county, town);
            return result;
        }

        [HttpPost]
        public Models.ApiResult RemoveByLocation(double lat, double lon)
        {
            var result = _adapter.removeBridgeByLocation(lat, lon);
            return result;
        }

        // POST: api/Bridge
        [HttpPost]
        public Models.ApiResult Create(Bridge bridge)
        {
            var result = _adapter.saveBridge(bridge);
            return result;
        }

        [HttpPost]
        public Models.ApiResult Update(Bridge bridge)
        {
            var result = _adapter.updateBridge(bridge);
            return result;
        }

        [HttpPost]
        public Models.ApiResult UpvoteBridge([FromUri] int bridgeId, [FromUri] string userName)
        {
            var results = _adapter.increaseVote(bridgeId, userName);
            return results;
        }

        [HttpPost]
        public Models.ApiResult DownVoteBridge([FromUri] int bridgeId, [FromUri] string userName, [FromUri] bool isEdit)
        {
            var results = _adapter.decreaseVote(bridgeId, userName, isEdit);
            return results;
        }

        [HttpGet]
        public IEnumerable<Models.BridgeCountResult> GetCountForStates()
        {
            var result = _adapter.getBridgeCountStates();
            return result;
        }

    }
}
