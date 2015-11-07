using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bware.Data.Model;

namespace WebService.Adapters.Interface
{
    interface IMessageAdapter
    {
        IEnumerable<Message> getMessagesWithinMiles(double lat, double lon, int miles);
        Models.ApiResult saveMessage(Message message);


    }
}
