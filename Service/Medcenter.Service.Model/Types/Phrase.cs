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
    public class Phrase : INotifyPropertyChanged
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public int ParaphraseId { get; set; }
        [DataMember]
        public int PositionId { get; set; }
        [DataMember]
        public string PositionName { get; set; }
        [DataMember]
        public int V1 { get; set; }
        [DataMember]
        public int V2 { get; set; }
        [DataMember]
        public int V3 { get; set; }
        [DataMember]
        public int Type { get; set; }//0 - header, 1 - number, 2 - string, 3 - formula
        [DataMember]
        public int ShowOrder { get; set; }
        [DataMember]
        public string Deviation { get; set; }
        [DataMember]
        public string Units { get; set; }
        [DataMember]
        public int DecorationType { get; set; }
        // 0 - in text, 1 - new paragraph, 2 - 1 and paragraph after, +10 - with Position Name
        [DataMember]
        public List<int> FormulesAffected { get; set; }
        public byte Status { get; set; } // 1 - Changed, 2 - New, 3 - To Delete, 4 - Copied from another Pattern
        
        public event PropertyChangedEventHandler PropertyChanged;

        public Phrase()
        {
            
        }
    }
}
