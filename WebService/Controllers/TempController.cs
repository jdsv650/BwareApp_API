using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebService.Controllers
{
    [RoutePrefix("Api/Temp")]
    public class TempController : ApiController
    {
        // GET: api/Temp
        public IEnumerable<string> GetAll()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Temp/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Tem
        [HttpPost]
        public void CreateTemp(Bware.Data.Model.Temp temp)
        {
    
            using (var context = new Bware.Data.BwareContext())
            {
                context.Temps.Add(new Bware.Data.Model.Temp()
                {
                    TempId = temp.TempId,
                    TempString = temp.TempString,
                });

               context.SaveChanges();

            }
        }

        // PUT: api/Temp/5
        [HttpPost]
        public void PutTemp(int id, [FromBody]string value)
        {
            Console.WriteLine(value);
        }

        // DELETE: api/Temp/5
        public void Delete(int id)
        {
        }
    }
}
