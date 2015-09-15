﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;

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
        public DateTime Date { get; set; }
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

        #region Paraphrases
        private Paraphrase _currentParaphrase;

        public Paraphrase CurrentParaphrase
        {
            get { return _currentParaphrase; }
            set
            {
                _currentParaphrase = value;
                //if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Paraphrases"));
            }
        }
        private List<Paraphrase> _paraphrases;
        private List<Paraphrase> _paraphrasesBase;

        public List<Paraphrase> Paraphrases
        {
            get { return _paraphrases; }
            set
            {
                _paraphrases = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Paraphrases"));
            }
        }
        [DataMember]
        public List<Paraphrase> ParaphrasesBase
        {
            get { return _paraphrasesBase; }
            set
            {
                _paraphrasesBase = value;
                //if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ParaphrasesBase"));
            }
        }
        public void FilterPhrases(int positionId)
        {
            Paraphrases = ParaphrasesBase.FindAll(d => d.PositionId == positionId);
        }
        public void AddParaphrase(Paraphrase paraphrase)
        {
            ParaphrasesBase.Add(paraphrase);
            FilterPhrases(paraphrase.PositionId);
        }
        #endregion

        public byte Status { get; set; } // 1 - normal, 4 - selected, 2 - saved, 3 - not saved
        private byte _oldStatus = 1;
        private bool _isActive=false;

        public void Choose()
        {
            if (_isActive)
            {
                Status = _oldStatus;
            }
            else
            {
                _oldStatus = Status;
                Status = 4;
            }
            _isActive = !_isActive;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public Survey()
        {
            Phrases=new List<Phrase>();
            ParaphrasesBase=new List<Paraphrase>();
            Phrases.Add(new Phrase(0));
        }
        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            if (string.IsNullOrEmpty(Name)) em.Add(new ResultMessage(2, "Наименование:", OperationErrors.EmptyString));
            if (string.IsNullOrEmpty(ShortName)) em.Add(new ResultMessage(2, "Краткое наименование:", OperationErrors.EmptyString));

            return em;
        }
    }
}
