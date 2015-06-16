using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Inspection
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public int InspectionGroupId { get; set; }
        [DataMember]
        public int Cost { get; set; }
        [DataMember]
        public List<int> InspectionGroupIds { get; set; }
        public bool IsChanged { get; set; }
        public bool IsRemoved { get; set; }
    }
}
