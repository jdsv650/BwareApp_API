using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bware.Data.Model;
using Bware.Data;
using System.Web.Http;

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
    }
}