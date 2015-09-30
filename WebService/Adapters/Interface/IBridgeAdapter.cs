using Bware.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Adapters.Interface
{
    interface IBridgeAdapter
    {
        // IEnumerable<Bridge> getAllBridges();
        IEnumerable<Bridge> getBridgesWithinMiles(double lat, double lon, int miles);
        Bridge getBridgeByLocation(double lat, double lon);
        Models.ApiResult getByInfo(string country, string state, string county, string town = "");
        Models.ApiResult removeBridgeByLocation(double lat, double lon);
        Models.ApiResult saveBridge(Bridge bridge);
        Models.ApiResult updateBridge(Bridge bridge);
        Models.ApiResult increaseVote(int bridgeId, String userName);
        Models.ApiResult decreaseVote(int bridgeId, String userName, bool isEdit);

    }

}
