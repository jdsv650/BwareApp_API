using Bware.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public IEnumerable<Bridge> GetAll()
        {
            var result = _adapter.getAllBridges();
            return result;
        }

        // GET: api/Bridge/5
      //  public string Get(int id)     
     //   {
      //      return "value";
      //  }

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
      //  [System.Web.Http.Route("Bridge/GetByMiles")]
        public IEnumerable<Bridge> GetByMiles(double lat, double lon, int miles)
        {
            var result = _adapter.getBridgesWithinMiles(lat, lon, miles);
            return result;
        }

        // POST: api/Bridge
        public void Post(Bridge bridge)
        {
            _adapter.saveBridge(bridge);
        }

        // PUT: api/Bridge/5Adapters
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Bridge/5
        public void Delete(int id)
        {
        }
    }
}
