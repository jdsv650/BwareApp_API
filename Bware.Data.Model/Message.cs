using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bware.Data.Model
{
    [DataContract]
    public class Message
    {
        public DbGeography MessageLocation { get; set; }

        [DataMember]
        public int MessageId { get; set; }
        [DataMember]
        public String MessageText { get; set; }
        [DataMember]
        public String UserCreated { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }

    }
}
