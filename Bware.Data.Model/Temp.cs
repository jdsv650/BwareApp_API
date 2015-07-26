using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Bware.Data.Model
{
    [DataContract]
    public class Temp
    {
        [DataMember]
        public int TempId { get; set; }
        [DataMember]
        public String TempString { get; set; }
    }
}
