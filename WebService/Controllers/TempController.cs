using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebService.Controllers
{
    [Authorize]
    public class TempController : ApiController
    {
        // GET: api/Temp
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Temp/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Temp
        public void Post(Bware.Data.Model.Temp temp)
        {
            
        }

        // PUT: api/Temp/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Temp/5
        public void Delete(int id)
        {
        }
    }
}
