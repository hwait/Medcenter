using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Phrase> _phrases;

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int DoctorId { get; set; }
        [DataMember]
        public int InspectionId { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Header { get; set; }
        [DataMember]
        public string Picture { get; set; }
        [DataMember]
        public string Signature { get; set; }
        [DataMember]
        public byte PictureType { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public Doctor CurrentDoctor { get; set; }
        [DataMember]
        public Patient CurrentPatient { get; set; }

        [DataMember]
        public ObservableCollection<Phrase> Phrases
        {
            get { return _phrases; }
            set
            {
                _phrases = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Phrases"));
            }
        }

        public void ActuateProperties()
        {
            //if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Phrases"));
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
        private ObservableCollection<Paraphrase> _paraphrases;
        private List<Paraphrase> _paraphrasesBase;

        public ObservableCollection<Paraphrase> Paraphrases
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

        public void MoveParaphraseUp(Paraphrase paraphrase)
        {
            var so = paraphrase.ShowOrder;
            var list =
                ParaphrasesBase.FindAll(p => p.PositionId == paraphrase.PositionId && p.ShowOrder < so)
                    .OrderByDescending(i => i.ShowOrder)
                    .ToList();
            if (list.Count == 0) return;
            paraphrase.ShowOrder = list[0].ShowOrder;
            list[0].ShowOrder = so;
            paraphrase.Status = 1;
            list[0].Status = 1;
            FilterParaphrases(paraphrase.PositionId);
        }
        public void MoveParaphraseDown(Paraphrase paraphrase)
        {
            var so = paraphrase.ShowOrder;
            var list =
                ParaphrasesBase.FindAll(p => p.PositionId == paraphrase.PositionId && p.ShowOrder > so)
                    .OrderBy(i => i.ShowOrder)
                    .ToList();
            if (list.Count == 0) return;
            paraphrase.ShowOrder = list[0].ShowOrder;
            list[0].ShowOrder = so;
            paraphrase.Status = 1;
            list[0].Status = 1;
            FilterParaphrases(paraphrase.PositionId);
        }
        public void FilterParaphrases(int positionId)
        {
            Paraphrases = new ObservableCollection<Paraphrase>(ParaphrasesBase.FindAll(d => d.PositionId == positionId).OrderBy(i=>i.ShowOrder));
        }
        public void SetPresettedParaphrases(string preset)
        {
            foreach (var phrase in Phrases)
            {
                if (phrase.Type!=1) continue;
                var paraphrase = (preset=="") ? new Paraphrase() : ParaphrasesBase.FirstOrDefault(d => d.PositionId == phrase.PositionId&&d.PresetId==preset);
                if (paraphrase==null) continue;
                phrase.Text = paraphrase.Text;
                if (paraphrase.V1 > 0) phrase.V1 = paraphrase.V1;
                if (paraphrase.V2 > 0) phrase.V2 = paraphrase.V2;
                if (paraphrase.V3 > 0) phrase.V3 = paraphrase.V3;
                phrase.ParaphraseId = paraphrase.Id;
            }
        }
        public void AddParaphrase(Paraphrase paraphrase)
        {
            ParaphrasesBase.Add(paraphrase);
            FilterParaphrases(paraphrase.PositionId);
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

        public Survey(int doctorId, int inspectionId)
        {
            DoctorId = doctorId;
            InspectionId = inspectionId;
            Phrases = new ObservableCollection<Phrase>();
            ParaphrasesBase = new List<Paraphrase>();
            Phrases.Add(new Phrase(0));
        }

        public Survey()
        {
            Phrases = new ObservableCollection<Phrase>();
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
