using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Nurse
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public List<int> DoctorIds { get; set; }
        public Nurse()
        {
            DoctorIds=new List<int>();
        }
    }
}
