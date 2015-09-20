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
        private string _text;
        private int _showOrder;
        private int _v1;
        private int _v2;
        private int _v3;
        public bool IsLoaded { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int PositionId { get; set; }

        [DataMember]
        public int V1
        {
            get { return _v1; }
            set
            {
                _v1 = value;
                if ((Status < 2 || Status > 3) && IsLoaded) Status = 1;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        }

        [DataMember]
        public int V2
        {
            get { return _v2; }
            set
            {
                _v2 = value;
                if ((Status < 2 || Status > 3) && IsLoaded) Status = 1;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        }

        [DataMember]
        public int V3
        {
            get { return _v3; }
            set
            {
                _v3 = value;
                if ((Status < 2 || Status > 3) && IsLoaded) Status = 1;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        }

        [DataMember]
        public int ShowOrder
        {
            get { return _showOrder; }
            set
            {
                _showOrder = value;
                if ((Status < 2 || Status > 3) && IsLoaded) Status = 1;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        }

        [DataMember]
        public string Norm { get; set; }

        [DataMember]
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                if ((Status < 2 || Status > 3) && IsLoaded) Status = 1;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        }

        [DataMember]
        public int IsolationType { get; set; }
        [DataMember]
        public int PresetId { get; set; }
        [DataMember]
        public List<int> FormulesAffected { get; set; }
        public bool IsChanged { get; set; }
        [DataMember]
        public byte Status { get; set; }
        public Paraphrase()
        {
            FormulesAffected=new List<int>();
            Status = 0;
        }
        public Paraphrase(Phrase phrase)
        {
            PositionId = phrase.PositionId;
            V1 = phrase.V1;
            V2 = phrase.V2;
            V3 = phrase.V3;
            ShowOrder = phrase.ShowOrder;
            Text = phrase.Text;
            FormulesAffected = new List<int>();
            Status = 0;
        }

        public Paraphrase(Paraphrase phrase)
        {
            V1 = phrase.V1;
            V2 = phrase.V2;
            V3 = phrase.V3;
            Text = phrase.Text;
            FormulesAffected = new List<int>();
            Status = 2;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
