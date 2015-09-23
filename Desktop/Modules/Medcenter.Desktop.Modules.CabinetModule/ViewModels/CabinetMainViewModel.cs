using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
//using Syncfusion.Windows.Shared;
using Microsoft.Win32;
using ServiceStack;

namespace Medcenter.Desktop.Modules.CabinetModule.ViewModels
{
    [Export]
    public class CabinetMainViewModel : BindableBase
    {
        #region Declarations
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        private readonly DelegateCommand<Phrase> _insertPhraseCommand;
        private readonly DelegateCommand<Phrase> _removePhraseCommand;
        private readonly DelegateCommand<Phrase> _normPhraseCommand;
        private readonly DelegateCommand<Phrase> _changedPhraseCommand;
        
        private readonly DelegateCommand<object> _choosePhraseCommand;
        private readonly DelegateCommand<Phrase> _saveParaphraseCommand;
        private readonly DelegateCommand<object> _savePatientCommand;
        private readonly DelegateCommand<Survey> _previewSurveyCommand;
        private readonly DelegateCommand<Survey> _newSurveyCommand;
        private readonly DelegateCommand<Survey> _removeSurveyCommand;
        private readonly DelegateCommand<Survey> _saveSurveyCommand;
        private readonly DelegateCommand<Survey> _chooseSurveyCommand;
        private readonly DelegateCommand<Reception> _chooseReceptionCommand;
        private readonly DelegateCommand<object> _chooseParaphraseCommand;
        private int _cabinetNumber = int.Parse(Utils.ReadSetting("CabinetNumber"));
        
        #endregion

        #region ICommands 
        public ICommand ChooseParaphraseCommand
        {
            get { return this._chooseParaphraseCommand; }
        }
        public ICommand InsertPhraseCommand
        {
            get { return this._insertPhraseCommand; }
        }
        public ICommand RemovePhraseCommand
        {
            get { return this._removePhraseCommand; }
        }
        public ICommand ChangedPhraseCommand
        {
            get { return this._changedPhraseCommand; }
        }
        public ICommand NormPhraseCommand
        {
            get { return this._normPhraseCommand; }
        }
        public ICommand ChoosePhraseCommand
        {
            get { return this._choosePhraseCommand; }
        }
        
        public ICommand SavePatientCommand
        {
            get { return this._savePatientCommand; }
        }
        public ICommand SaveParaphraseCommand
        {
            get { return this._saveParaphraseCommand; }
        }
        public ICommand PreviewSurveyCommand
        {
            get { return this._previewSurveyCommand; }
        }
        public ICommand ChooseSurveyCommand
        {
            get { return this._chooseSurveyCommand; }
        }
        public ICommand ChooseReceptionCommand
        {
            get { return this._chooseReceptionCommand; }
        }
        public ICommand NewSurveyCommand
        {
            get { return this._newSurveyCommand; }
        }
        public ICommand RemoveSurveyCommand
        {
            get { return this._removeSurveyCommand; }
        }
        public ICommand SaveSurveyCommand
        {
            get { return this._saveSurveyCommand; }
        }
        #endregion

        #region Properties

        #region Doctors

        private List<Doctor> _doctors;

        public List<Doctor> Doctors
        {
            get { return _doctors; }
            set { SetProperty(ref _doctors, value); }
        }

        private Doctor _currentDoctor;

        public Doctor CurrentDoctor
        {
            get { return _currentDoctor; }
            set
            {
                SetProperty(ref _currentDoctor, value);
            }
        }

        #endregion

        #region Patients

        private List<Patient> _Patients;

        public List<Patient> Patients
        {
            get { return _Patients; }
            set { SetProperty(ref _Patients, value); }
        }

        private Patient _currentPatient;

        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set
            {
                SetProperty(ref _currentPatient, value);
            }
        }

        #endregion

        #region Schedules

        private Schedule _currentSchedule;

        public Schedule CurrentSchedule
        {
            get { return _currentSchedule; }
            set
            {
                SetProperty(ref _currentSchedule, value);
                ReceptionsReload();
            }
        }
        private List<Schedule> _schedules;

        public List<Schedule> Schedules
        {
            get { return _schedules; }
            set
            {
                SetProperty(ref _schedules, value);
            }
        }
        #endregion

        #region Receptions

        private Reception _currentReception;

        public Reception CurrentReception
        {
            get { return _currentReception; }
            set
            {
                SetProperty(ref _currentReception, value);
            }
        }
        private List<Reception> _receptions;

        public List<Reception> Receptions
        {
            get { return _receptions; }
            set
            {
                SetProperty(ref _receptions, value);
            }
        }
        #endregion

        #region Surveys

        private List<Survey> _surveys;

        public List<Survey> Surveys
        {
            get { return _surveys; }
            set { SetProperty(ref _surveys, value); }
        }

        private Survey _currentSurvey;

        public Survey CurrentSurvey
        {
            get { return _currentSurvey; }
            set
            {
                SetProperty(ref _currentSurvey, value);
                _newSurveyCommand.RaiseCanExecuteChanged();
            }
        }
        
        #endregion

        #region Paraphrases

        private List<Paraphrase> _paraphrases;

        public List<Paraphrase> Paraphrases
        {
            get { return _paraphrases; }
            set { SetProperty(ref _paraphrases, value); }
        }

        private Paraphrase _currentParaphrase;

        public Paraphrase CurrentParaphrase
        {
            get { return _currentParaphrase; }
            set
            {
                SetProperty(ref _currentParaphrase, value);
            }
        }

        private List<Paraphrase> _paraphrasesBase;

        public List<Paraphrase> ParaphrasesBase
        {
            get { return _paraphrasesBase; }
            set { SetProperty(ref _paraphrasesBase, value); }
        }

       
        #endregion

        #region Phrases

        private Phrase _currentPhrase;

        public Phrase CurrentPhrase
        {
            get { return _currentPhrase; }
            set
            {
                SetProperty(ref _currentPhrase, value);
            }
        }
        
        #endregion

        #region Others

        public bool IsCopying
        {
            get { return _isCopying; }
            set { SetProperty(ref _isCopying, value); }
        }

        private List<ResultMessage> _errors;
        private bool _isCopying;

        public List<ResultMessage> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }

        #endregion

        #endregion

        [ImportingConstructor]
        public CabinetMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
           
            _newSurveyCommand = new DelegateCommand<Survey>(NewSurvey, CanNewSurvey);
            _previewSurveyCommand = new DelegateCommand<Survey>(PreviewSurvey, CanPreviewSurvey);
            _removeSurveyCommand = new DelegateCommand<Survey>(RemoveSurvey, CanRemoveSurvey);
            _saveSurveyCommand = new DelegateCommand<Survey>(SaveSurvey, CanSaveSurvey);
            _insertPhraseCommand = new DelegateCommand<Phrase>(InsertPhrase);
            _removePhraseCommand = new DelegateCommand<Phrase>(RemovePhrase);
            _changedPhraseCommand = new DelegateCommand<Phrase>(ChangedPhrase);
            _normPhraseCommand = new DelegateCommand<Phrase>(NormPhrase);
            _choosePhraseCommand = new DelegateCommand<object>(ChoosePhrase);
            _saveParaphraseCommand = new DelegateCommand<Phrase>(SaveParaphrase);
            _savePatientCommand = new DelegateCommand<object>(SavePatient);
            _chooseParaphraseCommand = new DelegateCommand<object>(ChooseParaphrase);
            _chooseSurveyCommand = new DelegateCommand<Survey>(ChooseSurvey, CanChooseSurvey);
            _chooseReceptionCommand = new DelegateCommand<Reception>(ChooseReception);
            
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();
            IsCopying = false;
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.PostAsync(new SchedulesFullSelect {TimeStart = DateTime.Today})
                .Success(rs =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    Schedules = rs.Schedules;
                })
                .Error(ex =>
                {
                    Schedules = new List<Schedule>();
                    throw ex;
                });
        }

        private void SetParaphrase(object obj)
        {
            throw new NotImplementedException();
        }

        private void ChangedPhrase(Phrase obj)
        {
            //if (obj!=null&&obj.Text.Split('{').Length>0)
            //    this.OnPropertyChanged(() => this.CurrentSurvey);
        }

        #region Reception

        private void ChooseReception(Reception obj)
        {
            CurrentReception = obj;
            CurrentPatient = obj.Patient;
            Surveys = new List<Survey>();
            //CurrentSurvey = new Survey();
            CurrentSurvey = null;
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.PostAsync(new SurveySelect {Reception = CurrentReception, DoctorId = CurrentSchedule.DoctorId})
                .Success(rs =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    Surveys = rs.Surveys;
                    foreach (var survey in Surveys)
                    {
                        foreach (var phrase in survey.Phrases)
                        {
                            phrase.IsLoaded = true;
                            phrase.ValueChanged += phrase_ValueChanged;
                        }
                    }
                })
                .Error(ex =>
                {
                    Schedules = new List<Schedule>();
                    throw ex;
                });
        }

        void phrase_ValueChanged(object sender, PropertyChangedEventArgs e)
        {
           
        }
        private void ReceptionsReload()
        {
            Receptions = new List<Reception>();
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new ReceptionsByDateSelect { StartDate = DateTime.Today, ScheduleId = CurrentSchedule.Id })
            .Success(rr =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                Receptions = rr.Receptions;
            })
            .Error(ex =>
            {
                throw ex;
            });
        }
        #endregion

        #region Survey

        private bool CanChooseSurvey(Survey arg)
        {
            return true;
        }

        private void ChooseSurvey(Survey obj)
        {
            CurrentSurvey = obj;
            SetButtonsActive();
        }
        private void SurveyReload()
        {

            //if (_currentInspection==null) return;
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            //_jsonClient.GetAsync(new SurveyPatternSelect { DoctorId = _currentDoctor.Id, InspectionId = _currentInspection.Id })
            //.Success(r =>
            //{
            //    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //    CurrentSurvey = r.Survey;
            //    if (CurrentSurvey!=null)
            //    {
            //        IsCopying = false;
            //        foreach (var phrase in CurrentSurvey.Phrases)
            //        {
            //            phrase.Status = 0;
            //        }
            //    }
            //    SetButtonsActive();
            //})
            //.Error(ex =>
            //{
            //    throw ex;
            //});
        }
        private void PreviewSurvey(Survey obj)
        {
            throw new NotImplementedException();
        }
        private void NewSurvey(Survey obj)
        {
            CurrentSurvey = new Survey();
            SetButtonsActive();
        }
        private void CopySurvey(Survey obj)
        {
            IsCopying = true;
            SetButtonsActive();
        }

        private void SaveSurvey(Survey obj)
        {
            Errors = CurrentSurvey.Validate();
            if (Errors.Count == 0)
            {
                var phrases = new ObservableCollection<Phrase>(CurrentSurvey.Phrases.Where(phrase => phrase.Status > 0 && phrase.Id==0).ToList());
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.PostAsync(new SurveySave { Phrases = phrases, SurveyId = CurrentSurvey.Id})
                    .Success(r =>
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                        CurrentSurvey.Phrases = r.Phrases;
                        this.OnPropertyChanged(() => this.CurrentSurvey);
                    })
                    .Error(ex =>
                    {
                        throw ex;
                    });
            }
        }

        private void RemoveSurvey(Survey obj)
        {
            if (IsCopying)
            {
                IsCopying = false;
                SetButtonsActive();
                return;
            }
            bool isNew = CurrentSurvey.Id == 0;
            ConfirmationRequest.Raise(
                new Confirmation { Content = "Бланк будет удалён! Вы уверены?", Title = "Удаление бланка." },
                c =>
                {
                    if (c.Confirmed)
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                        if (isNew)
                        {
                            CurrentSurvey = new Survey();
                            _newSurveyCommand.RaiseCanExecuteChanged();
                        }
                        else
                        {
                            _jsonClient.GetAsync(new SurveyPatternDelete { PatternId = CurrentSurvey.Id })
                            .Success(r =>
                            {
                                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false); _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                                r.Message.Message = string.Format(r.Message.Message, CurrentSurvey.Name);
                                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                                SurveyReload();
                                _newSurveyCommand.RaiseCanExecuteChanged();
                            })
                            .Error(ex =>
                            {
                                throw ex;
                            });
                        }
                    }
                });
        }
        #endregion
        
        #region Phrases
        
        private void ChoosePhrase(object phrase)
        {
            CurrentPhrase = (Phrase) phrase;
            CurrentSurvey.FilterParaphrases(phrase == null ? 0 : ((Phrase)phrase).PositionId);
        }

        private void NormPhrase(Phrase obj)
        {
            //obj.CutPhrase();
        }

        private void RemovePhrase(Phrase phrase)
        {
            var phrases = CurrentSurvey.Phrases.Where(p => p.PositionId == phrase.PositionId).ToList();
            if (phrases.Count > 1)
            {
                if (phrases[phrases.Count - 1].Id == 0)
                    CurrentSurvey.Phrases.Remove(phrases[phrases.Count - 1]);
                else
                    phrases[phrases.Count - 1].Status = 3;
            }
            else
                phrases[phrases.Count - 1].Text = "";
        }

        private void InsertPhrase(Phrase phrase)
        {
            CurrentSurvey.Phrases.Insert(GetPhraseIndex(phrase)+1, phrase.CloneIt());
            CurrentSurvey.ActuateProperties();
            //this.OnPropertyChanged(() => this.CurrentSurvey);
        }

        private int GetPhraseIndex(Phrase phrase)
        {
            for (int i = 0; i < CurrentSurvey.Phrases.Count; i++)
            {
                if (CurrentSurvey.Phrases[i] == phrase)
                    return i;
            }
            return CurrentSurvey.Phrases.Count-1;
        }
        #endregion
        
        #region Paraphrase
        private void ChooseParaphrase(object obj)
        {
            var paraphrase = (Paraphrase) obj;
            CurrentPhrase.Text = paraphrase.Text;
            CurrentPhrase.V1 = paraphrase.V1;
            CurrentPhrase.V2 = paraphrase.V2;
            CurrentPhrase.V3 = paraphrase.V3;
            CurrentPhrase.ParaphraseId = paraphrase.Id;
        }
        private void SaveParaphrase(Phrase obj)
        {
            var paraphrase=new Paraphrase(obj);
            paraphrase.ShowOrder = CurrentSurvey.Paraphrases.Max(p => p.ShowOrder);
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.PostAsync(new ParaphraseSave { Paraphrase = paraphrase, DoctorId = CurrentSchedule.DoctorId, InspectionId = CurrentSurvey.InspectionId })
                .Success(rs =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    paraphrase.Id = rs.ParaphraseId;
                    CurrentSurvey.AddParaphrase(paraphrase);
                })
                .Error(ex =>
                {
                    Schedules = new List<Schedule>();
                    throw ex;
                });
        }

        #endregion
        
        #region Other

        private void SavePatient(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.PostAsync(new PatientClarify { Patient = CurrentPatient })
                .Success(r =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    r.Message.Message = string.Format(r.Message.Message, CurrentPatient.Surname, CurrentPatient.FirstName, CurrentPatient.SecondName);
                    _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                })
                .Error(ex =>
                {
                    Schedules = new List<Schedule>();
                    throw ex;
                });
        }

        private void SetButtonsActive()
        {
            _newSurveyCommand.RaiseCanExecuteChanged();
            _removeSurveyCommand.RaiseCanExecuteChanged();
            _saveSurveyCommand.RaiseCanExecuteChanged();
            _previewSurveyCommand.RaiseCanExecuteChanged();
        }
        private bool CanNewSurvey(Survey arg)
        {
            return CurrentSurvey != null;
        }

        private bool CanSaveSurvey(Survey arg)
        {
            return CurrentSurvey != null;
        }

        private bool CanRemoveSurvey(Survey arg)
        {
            return CurrentSurvey != null;
        }
        private bool CanPreviewSurvey(Survey arg)
        {
            return CurrentSurvey != null;
        }
        #endregion

    }
}
