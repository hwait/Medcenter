using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class LimitsTable
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int PositionId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<Limit> Limits { get; set; }
    }
}
