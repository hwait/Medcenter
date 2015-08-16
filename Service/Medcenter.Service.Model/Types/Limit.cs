using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Limit
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Result { get; set; }
        [DataMember]
        public int Gender { get; set; }
        [DataMember]
        public int AgeMin { get; set; }
        [DataMember]
        public int AgeMax { get; set; }
        [DataMember]
        public int ValueMin { get; set; }
        [DataMember]
        public int ValueMax { get; set; }
    }
}
