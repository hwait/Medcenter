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
        public ICommand NormPhraseCommand
        {
            get { return this._normPhraseCommand; }
        }
        public ICommand SavePatientCommand
        {
            get { return this._savePatientCommand; }
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
            _normPhraseCommand = new DelegateCommand<Phrase>(NormPhrase);
            _savePatientCommand = new DelegateCommand<object>(SavePatient);
            _chooseParaphraseCommand = new DelegateCommand<object>(ChooseParaphrase);
            _chooseSurveyCommand = new DelegateCommand<Survey>(ChooseSurvey, CanChooseSurvey);
            _chooseReceptionCommand = new DelegateCommand<Reception>(ChooseReception);
            
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();
            IsCopying = false;
            _jsonClient.PostAsync(new SchedulesFullSelect {TimeStart = DateTime.Today})
                .Success(rs =>
                {
                    //_eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    Schedules = rs.Schedules;
                    
                })
                .Error(ex =>
                {
                    Schedules = new List<Schedule>();
                    throw ex;
                });
        }

        private void ChooseReception(Reception obj)
        {
            CurrentReception = obj;
            CurrentPatient = obj.Patient;
        }

        private bool CanChooseSurvey(Survey arg)
        {
            return true;
        }

        private void ChooseSurvey(Survey obj)
        {
            throw new NotImplementedException();
        }

        private void ChooseParaphrase(object obj)
        {
            throw new NotImplementedException();
        }

        #region CanExecute

        private bool CanNewSurvey(Survey arg)
        {
            if (IsCopying) return false;
            return !(CurrentDoctor == null ||  CurrentSurvey != null);
        }

        private bool CanSaveSurvey(Survey arg)
        {
            return !(CurrentDoctor == null || CurrentSurvey == null);
        }

        private bool CanRemoveSurvey(Survey arg)
        {
            //if (IsCopying) return false;
            return !(CurrentDoctor == null ||  CurrentSurvey == null);
        }
        private bool CanPreviewSurvey(Survey arg)
        {
            if (IsCopying) return false;
            return !(CurrentDoctor == null || CurrentSurvey == null);
        }
        #endregion

        private void ReceptionsReload()
        {
            Receptions=new List<Reception>();
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

        private void SetButtonsActive()
        {
            _newSurveyCommand.RaiseCanExecuteChanged();
            _removeSurveyCommand.RaiseCanExecuteChanged();
            _saveSurveyCommand.RaiseCanExecuteChanged();
            _previewSurveyCommand.RaiseCanExecuteChanged();
        }

        #region Positions
        private void SavePatient(object obj)
        {
            
        }
        private void NormPhrase(Phrase obj)
        {
            //obj.CutPhrase();
        }

        private void RemovePhrase(Phrase obj)
        {
            if (obj.Id == 0)
            {
                CurrentSurvey.Phrases.Remove(obj);
                RefreshPhrases();
            }
            else
                obj.RemovePhrase();
        }

        private void InsertPhrase(Phrase obj)
        {
            var n = 0;
            List<Phrase> list = CurrentSurvey.Phrases.Where(p => p.Status==4).ToList();
            if (list.Count == 0)
            {
                IncrementToEnd(obj.ShowOrder+1,1);
                CurrentSurvey.Phrases.Add(new Phrase(obj.ShowOrder+1));
            }
            else
            {
                CurrentSurvey.Phrases.RemoveAll(p => p.Status == 4);
                for (int i = 0; i < CurrentSurvey.Phrases.Count; i++)
                {
                    if (CurrentSurvey.Phrases[i].Status == 0) CurrentSurvey.Phrases[i].Status = 1;
                    CurrentSurvey.Phrases[i].ShowOrder = i;
                    if (CurrentSurvey.Phrases[i].PositionName == obj.PositionName &&
                        CurrentSurvey.Phrases[i].Text == obj.Text) n = i;
                }
                IncrementToEnd(n+1, list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Status = 1;
                    list[i].ShowOrder = n + i+1;
                }
                CurrentSurvey.Phrases.AddRange(list);
            }
            RefreshPhrases();
        }

        private void RefreshPhrases()
        {
            CurrentSurvey.Phrases = CurrentSurvey.Phrases.OrderBy(x => x.ShowOrder).ToList();
            for (int i = 0; i < CurrentSurvey.Phrases.Count; i++)
            {
                CurrentSurvey.Phrases[i].ShowOrder = i;
            }
        }

        private void IncrementToEnd(int showOrder, int inc)
        {
            if (showOrder > CurrentSurvey.Phrases.Count) return;
            for (int i = showOrder; i < CurrentSurvey.Phrases.Count; i++)
            {
                CurrentSurvey.Phrases[i].ShowOrder+=inc;
            }
        }

        #endregion
        
        #region Survey
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
            IsCopying=true;
            SetButtonsActive();
        }

        private void SaveSurvey(Survey obj)
        {
            Errors = CurrentSurvey.Validate();
            if (Errors.Count == 0)
            {
                
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                if (IsCopying)
                {
                    CurrentSurvey.Id = 0;
                    foreach (var phrase in CurrentSurvey.Phrases)
                    {
                        phrase.Status = 5;
                    }
                }
                else
                {
                    CurrentSurvey.Phrases.RemoveAll(x => x.Status == 0);
                }
                //_jsonClient.PostAsync(new SurveyPatternSave { Survey = CurrentSurvey, DoctorId = CurrentDoctor.Id, InspectionId = CurrentInspection.Id})
                //    .Success(r =>
                //    {
                //        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                //        CurrentSurvey.Id = r.SurveyId;
                //        r.Message.Message = string.Format(r.Message.Message, CurrentSurvey.Name);
                //        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                //        IsCopying = false;
                //        SurveyReload();
                //    })
                //    .Error(ex =>
                //    {
                //        throw ex;
                //    });
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
        private bool CanAddSurvey(object arg)
        {
            //return CurrentSurvey == null || CurrentSurvey.Id != 0;
            return true;
        }
        #endregion
    }
}
