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
        IEnumerable<Bridge> getAllBridges();
      
    }
}
