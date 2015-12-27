using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Models
{
    [DataContract]
    public class UserEmail
    {
        [DataMember]
        public string email { get; set; }
    }
}
