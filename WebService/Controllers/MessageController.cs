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
    public class MessageController : ApiController
    {
        Adapters.Interface.IMessageAdapter _adapter;

        public MessageController()
        {
            _adapter = new Adapters.Data.MessageAdapter();
        }

        [HttpGet]
        public IEnumerable<Message> GetByMiles(double lat, double lon, int miles)
        {
            var result = _adapter.getMessagesWithinMiles(lat, lon, miles);
            return result;
        }

        [HttpPost]
        public Models.ApiResult Create(Message message)
        {
            var result = _adapter.saveMessage(message);
            return result;
        }



    }
}
