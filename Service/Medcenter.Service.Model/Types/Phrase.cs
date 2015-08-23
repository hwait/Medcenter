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
        private byte _status;
        private string _text;
        private string _positionName;
        private int _type;
        private int _showOrder;
        private int _decorationType;

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                if (Status < 2 || Status > 3) Status = 1;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Text"));
            }
        }

        [DataMember]
        public int ParaphraseId { get; set; }
        [DataMember]
        public int PositionId { get; set; }

        [DataMember]
        public string PositionName
        {
            get { return _positionName; }
            set
            {
                _positionName = value;
                if (Status < 2 || Status > 3) Status = 1;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("PositionName"));
            }
        }

        [DataMember]
        public int V1 { get; set; }
        [DataMember]
        public int V2 { get; set; }
        [DataMember]
        public int V3 { get; set; }

        [DataMember]
        public int Type
        {
            get { return _type; }
            set
            {
                _type = value;
                if (Status < 2 || Status > 3) Status = 1;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Type"));
            }
        } //0 - header, 1 - number, 2 - string, 3 - formula

        [DataMember]
        public int ShowOrder
        {
            get { return _showOrder; }
            set
            {
                if (_showOrder != value)
                {
                    _oldShowOrder = _showOrder;
                    _showOrder = value;
                    if (Status < 2 || Status > 3) Status = 1;
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ShowOrder"));
                }
            }
        }
        private int _oldShowOrder;
        [DataMember]
        public string Deviation { get; set; }
        [DataMember]
        public string Units { get; set; }

        [DataMember]
        public int DecorationType
        {
            get { return _decorationType; }
            set
            {
                _decorationType = value;
                if (Status < 2 || Status > 3) Status = 1;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("DecorationType"));
                
            }
        }

        // 0 - in text, 1 - first paragraph, 2 - 1 + last paragraph, +10 - with Position Name
        [DataMember]
        public List<int> FormulesAffected { get; set; }

        public string ToggleShowPositionTag
        {
            get
            {
                var tag = (DecorationType < 10) ? "Default" : "Save";
                return tag;
            }
        }
        public string ToggleFirstParagraphTag
        {
            get
            {
                var tag = (DecorationType == 0 || DecorationType == 10) ? "Default" : "Save";
                return tag;
            }
        }
        public string ToggleLastParagraphTag
        {
            get
            {
                var tag = (DecorationType == 2 || DecorationType == 12) ? "Save" : "Default";
                return tag;
            }
        }
        

        public void ToggleFirstParagraph()
        {
            switch (DecorationType)
            {
                case 0:
                    DecorationType = 1;
                    break;
                case 1:
                case 2:
                    DecorationType = 0;
                    break;
                case 10:
                    DecorationType = 11;
                    break;
                case 11:
                case 12:
                    DecorationType = 10;
                    break;
            }
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ToggleFirstParagraphTag"));
                PropertyChanged(this, new PropertyChangedEventArgs("ToggleLastParagraphTag"));
            }
        }
        public void ToggleLastParagraph()
        {
            switch (DecorationType)
            {
                case 0:
                case 1:
                    DecorationType = 2;
                    break;
                case 2:
                    DecorationType = 1;
                    break;
                case 12:
                    DecorationType = 11;
                    break;
                case 10:
                case 11:
                    DecorationType = 12;
                    break;
            }
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ToggleFirstParagraphTag"));
                PropertyChanged(this, new PropertyChangedEventArgs("ToggleLastParagraphTag"));
            }
        }
        public void ToggleShowPosition()
        {
            DecorationType = (DecorationType < 10) ? DecorationType + 10 : DecorationType - 10;
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ToggleShowPositionTag"));
        }
        public void CutPhrase()
        {
            Status = Status == 4 ? _oldStatus < 3 ? _oldStatus : (byte) 1 : (byte)4;
        }
        public void RemovePhrase()
        {
            Status = Status == 3 ? _oldStatus < 3 ? _oldStatus : (byte) 1 : (byte)3;
        }
        private byte _oldStatus ;
        [DataMember]
        public byte Status
        {
            get { return _status; }
            set
            {
                _oldStatus = _status;
                _status = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        } // 1 - Changed, 2 - New, 3 - To Delete, 4 - Cut

        public event PropertyChangedEventHandler PropertyChanged;

        public Phrase()
        {
            Type = 2;
            Status = 0;
        }
        public Phrase(int showOrder)
        {
            ShowOrder = showOrder;
            Text = showOrder.ToString();
            Status = 2;
            Type = 2;
        }
    }
}
