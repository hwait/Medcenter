using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class InspectionGroup
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public byte Row { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public List<int> InspectionIds { get; set; }
        public bool IsChanged { get; set; }

        public bool IsRemoved { get; set; }
    }
}
