using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Paraphrase : INotifyPropertyChanged
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int PositionId { get; set; }
        [DataMember]
        public int V1 { get; set; }
        [DataMember]
        public int V2 { get; set; }
        [DataMember]
        public int V3 { get; set; }
        [DataMember]
        public int ShowOrder { get; set; }
        [DataMember]
        public string Norm { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public int IsolationType { get; set; }
        [DataMember]
        public List<int> FormulesAffected { get; set; }
        public bool IsChanged { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
