﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Phrase : INotifyPropertyChanged
    {
        private byte _status;
        private string _text;
        private string _resultV1;
        private string _resultV2;
        private string _resultV3;
        private string _positionName;
        private int _type;
        private int _showOrder;
        private int _decorationType;
        public bool IsLoaded { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text
        {
            get
            {
                
                return _text;
            }
            set
            {
                _text = value;
                if (!IsLoaded) return;
                if (Status < 2 || Status > 3) Status = 1;
                ParaphraseId = 0;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ValuesCount"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Status"));
                }

            }
        }

        [DataMember]
        public string ResultV1
        {
            get { return _resultV1; }
            set
            {
                _resultV1 = value;
                SetText();
                if (PropertyChanged != null&&IsLoaded)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }

        private void SetText()
        {
            if (Type == 2 && IsLoaded)
                if (ValuesCount == 3)
                    Text = string.Format(PrintName, V1, ResultV1, V2, ResultV2, V3, ResultV3);
                else if (ValuesCount == 2)
                    Text = string.Format(PrintName, V1, ResultV1, V2, ResultV2);
                else if (ValuesCount == 1)
                    Text = string.Format(PrintName, V1, ResultV1);
        }

        [DataMember]
        public string ResultV2
        {
            get { return _resultV2; }
            set
            {
                _resultV2 = value;
                SetText();
                if (PropertyChanged != null && IsLoaded)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }
        [DataMember]
        public string ResultV3
        {
            get { return _resultV3; }
            set
            {
                _resultV3 = value;
                SetText();
                if (PropertyChanged != null && IsLoaded)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }
        public int ValuesCount
        {
            get
            {
                if (string.IsNullOrEmpty(Text)) return 0;
                if (Type==2) 
                    return (PrintName.Split('{').Length - 1)/2;
                return Text.Split('{').Length - 1;
            }
        }

        public string TextOut
        {
            get
            {
                if (V3 > 0)
                    return String.Format(Text,V1,V2,V3);
                if (V2 > 0)
                    return String.Format(Text, V1, V2);
                if (V1 > 0)
                    return String.Format(Text, V1);
                return Text;
            }
            set { Text = value; }
        }
        [DataMember]
        public string PrintName { get; set; }
        [DataMember]
        public int NormTableId { get; set; }
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
                var isNotLoaded = _positionName == null;
                _positionName = value;
                if (isNotLoaded) return;
                if ((Status < 2 || Status > 3)) Status = 1;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("PositionName"));
            }
        }

        [DataMember]
        public decimal V1
        {
            get { return _v1; }
            set
            {
                _v1 = value;
                if (!IsLoaded) return;
                if (Status < 2 || Status > 3) Status = 1;
                if (ValueChanged != null) ValueChanged(this, new PropertyChangedEventArgs("V1"));
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Status"));
                }
            }
        }

        [DataMember]
        public decimal V2
        {
            get { return _v2; }
            set
            {
                _v2 = value;
                if (!IsLoaded) return;
                if (Status < 2 || Status > 3) Status = 1;
                if (ValueChanged != null) ValueChanged(this, new PropertyChangedEventArgs("V2"));
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Status"));
                }
            }
        }

        [DataMember]
        public decimal V3
        {
            get { return _v3; }
            set
            {
                _v3 = value;
                if (!IsLoaded) return;
                if (Status < 2 || Status > 3) Status = 1;
                if (ValueChanged != null) ValueChanged(this, new PropertyChangedEventArgs("V3"));
                if (PropertyChanged != null)
                {
                    
                    //PropertyChanged(this, new PropertyChangedEventArgs("V3"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Status"));
                }
            }
        }

        [DataMember]
        public int Type
        {
            get { return _type; }
            set
            {
                _type = value;
                if ((Status < 2 || Status > 3) && IsLoaded) Status = 1;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Type"));
            }
        } //0 - header, 1 - string, 2 - number, 3 - formula, 4 - drug, 5 - image

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
                    if ((Status < 2 || Status > 3) && IsLoaded) Status = 1;
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
                if ((Status < 2 || Status > 3) && IsLoaded) Status = 1;
                
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
        private decimal _v1;
        private decimal _v2;
        private decimal _v3;

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
        public event PropertyChangedEventHandler ValueChanged;

        public Phrase()
        {
            Type = 2;
            Status = 0;
        }
        public Phrase(int showOrder)
        {
            ShowOrder = showOrder;
            //Text = showOrder.ToString();
            Status = 2;
            Type = 2;
        }

        public Phrase CloneIt()
        {
            return new Phrase
            {
                IsLoaded=true,
                Id=0,
                Text = "",
                PositionName = this.PositionName,
                PositionId=this.PositionId,
                Type=this.Type,
                ShowOrder = this.ShowOrder,
                DecorationType=this.DecorationType,
                V1=this.V1,
                V2 = this.V2,
                V3 = this.V3,
                Status=2
            };
        }
    }
}
