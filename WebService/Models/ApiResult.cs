using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebService.Models
{
    [DataContract]
    public class ApiResult
    {
        [DataMember]
        public Boolean isSuccess { get; set; }

        [DataMember]
        public String message { get; set; }

        [DataMember]
        public Bware.Data.Model.Bridge data { get; set; }

        // please get rid of me
        [DataMember]
        public IEnumerable<Bware.Data.Model.Bridge> multipleData { get; set; }
    }
}