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
    public class Survey : INotifyPropertyChanged
    {
        private List<Phrase> _phrases;

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Header { get; set; }
        [DataMember]
        public Doctor CurrentDoctor { get; set; }
        [DataMember]
        public Patient CurrentPatient { get; set; }

        [DataMember]
        public List<Phrase> Phrases
        {
            get { return _phrases; }
            set
            {
                _phrases = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Phrases"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Survey()
        {
            Phrases=new List<Phrase>();
            Phrases.Add(new Phrase());
        }
    }
}
