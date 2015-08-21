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

namespace Medcenter.Desktop.Modules.SurveysManagerModule.ViewModels
{
    [Export]
    public class SurveysManagerMainViewModel : BindableBase
    {
        #region Declarations
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        private readonly DelegateCommand<Phrase> _insertPhraseCommand;
        private readonly DelegateCommand<Phrase> _removePhraseCommand;
        private readonly DelegateCommand<Phrase> _cutPhraseCommand;
        private readonly DelegateCommand<Phrase> _toggleLastParagraphCommand;
        private readonly DelegateCommand<Phrase> _toggleFirstParagraphCommand;
        private readonly DelegateCommand<Phrase> _toggleShowPositionCommand;
        private readonly DelegateCommand<Survey> _previewSurveyCommand;
        private readonly DelegateCommand<Survey> _newSurveyCommand;
        private readonly DelegateCommand<Survey> _removeSurveyCommand;
        private readonly DelegateCommand<Survey> _saveSurveyCommand;
        #endregion

        #region ICommands 

        public ICommand InsertPhraseCommand
        {
            get { return this._insertPhraseCommand; }
        }
        public ICommand RemovePhraseCommand
        {
            get { return this._removePhraseCommand; }
        }
        public ICommand CutPhraseCommand
        {
            get { return this._cutPhraseCommand; }
        }
        public ICommand ToggleLastParagraphCommand
        {
            get { return this._toggleLastParagraphCommand; }
        }
        public ICommand ToggleFirstParagraphCommand
        {
            get { return this._toggleFirstParagraphCommand; }
        }
        public ICommand ToggleShowPositionCommand
        {
            get { return this._toggleShowPositionCommand; }
        }
        public ICommand PreviewSurveyCommand
        {
            get { return this._previewSurveyCommand; }
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
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.GetAsync(new InspectionsInDoctorSelect { DoctorId = _currentDoctor.Id })
                .Success(r =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    Inspections = r.Inspections;
                })
                .Error(ex =>
                {
                    throw ex;
                });
            }
        }

        #endregion

        #region Inspections

        private List<Inspection> _inspections;

        public List<Inspection> Inspections
        {
            get { return _inspections; }
            set { SetProperty(ref _inspections, value); }
        }

        private Inspection _currentInspection;

        public Inspection CurrentInspection
        {
            get { return _currentInspection; }
            set
            {
                SetProperty(ref _currentInspection, value);
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.GetAsync(new InspectionsInDoctorSelect { DoctorId = _currentDoctor.Id })
                .Success(r =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    Inspections = r.Inspections;
                })
                .Error(ex =>
                {
                    throw ex;
                });
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
            set { SetProperty(ref _currentSurvey, value); }
        }

        #endregion
        
        #region Others

        private List<ResultMessage> _errors;

        public List<ResultMessage> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }

        #endregion

        #endregion

        [ImportingConstructor]
        public SurveysManagerMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _insertPhraseCommand = new DelegateCommand<Phrase>(InsertPhrase);
            _removePhraseCommand = new DelegateCommand<Phrase>(RemovePhrase);
            _cutPhraseCommand = new DelegateCommand<Phrase>(CutPhrase);
            _toggleFirstParagraphCommand = new DelegateCommand<Phrase>(ToggleFirstParagraph);
            _toggleLastParagraphCommand = new DelegateCommand<Phrase>(ToggleLastParagraph);
            _toggleShowPositionCommand = new DelegateCommand<Phrase>(ToggleShowPosition);
            _previewSurveyCommand = new DelegateCommand<Survey>(PreviewSurvey);
            _newSurveyCommand = new DelegateCommand<Survey>(NewSurvey);
            _removeSurveyCommand = new DelegateCommand<Survey>(RemoveSurvey);
            _saveSurveyCommand = new DelegateCommand<Survey>(SaveSurvey);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();
            CurrentSurvey=new Survey();

            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new DoctorsSelect())
            .Success(rig =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                Doctors = rig.Doctors;
            })
            .Error(ex =>
            {
                throw ex;
            });

            //_eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            //_jsonClient.GetAsync(new PackagesSelect())
            //.Success(ri =>
            //{
            //    //_eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //    PackagesBase = new ListCollectionView(ri.Packages);
            //    _jsonClient.GetAsync(new SurveysSelect())
            //    .Success(rig =>
            //    {
            //        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //        Surveys = new ListCollectionView(rig.Surveys);
            //        Surveys.CurrentChanged += Surveys_CurrentChanged;

            //        CurrentSurvey = new Survey();
            //        PackagesInSurvey.CurrentChanged += PackagesInSurvey_CurrentChanged;
            //        PackagesReload(ri.Packages);
            //        Surveys.MoveCurrentTo(null);
            //    })
            //    .Error(ex =>
            //    {
            //        throw ex;
            //    });
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

        private void ToggleShowPosition(Phrase obj)
        {
            throw new NotImplementedException();
        }

        private void ToggleLastParagraph(Phrase obj)
        {
            throw new NotImplementedException();
        }

        private void ToggleFirstParagraph(Phrase obj)
        {
            throw new NotImplementedException();
        }

        private void CutPhrase(Phrase obj)
        {
            throw new NotImplementedException();
        }

        private void RemovePhrase(Phrase obj)
        {
            throw new NotImplementedException();
        }

        private void InsertPhrase(Phrase obj)
        {
            throw new NotImplementedException();
        }

        #region Survey

        private void NewSurvey(Survey obj)
        {
            CurrentSurvey = new Survey();
        }

        private void SaveSurvey(Survey obj)
        {
            //bool isNew = CurrentSurvey.Id <= 0;
            //Errors = CurrentSurvey.Validate();
            //if (Errors.Count == 0)
            //{
            //    _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            //    _jsonClient.PostAsync(new SurveySave { Survey = CurrentSurvey })
            //        .Success(r =>
            //        {
            //            _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //            CurrentSurvey.Id = r.SurveyId;
            //            if (isNew) Surveys.AddNewItem(CurrentSurvey);
            //            r.Message.Message = string.Format(r.Message.Message, CurrentSurvey.Name);
            //            _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
            //            _newSurveyCommand.RaiseCanExecuteChanged();
            //        })
            //        .Error(ex =>
            //        {
            //            throw ex;
            //        });
            //}
        }

        private void RemoveSurvey(Survey obj)
        {
            //bool isNew = CurrentSurvey.Id == 0;
            //ConfirmationRequest.Raise(
            //    new Confirmation { Content = "Группа будет удалёна! Вы уверены?", Title = "Удаление группы инспекций." },
            //    c =>
            //    {
            //        if (c.Confirmed)
            //        {
            //            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            //            if (isNew)
            //            {
            //                CurrentSurvey = new Survey();
            //                _newSurveyCommand.RaiseCanExecuteChanged();
            //            }
            //            else
            //            {
            //                _jsonClient.GetAsync(new SurveyDelete { SurveyId = CurrentSurvey.Id })
            //                .Success(r =>
            //                {
            //                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false); _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //                    r.Message.Message = string.Format(r.Message.Message, CurrentSurvey.Name);
            //                    RemovePackageFromSurveyByIGID(CurrentSurvey.Id);
            //                    Surveys.Remove(Surveys.CurrentItem);
            //                    _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
            //                    _newSurveyCommand.RaiseCanExecuteChanged();
            //                })
            //                .Error(ex =>
            //                {
            //                    throw ex;
            //                });
            //            }
            //        }
            //    });
        }
        private bool CanAddSurvey(object arg)
        {
            //return CurrentSurvey == null || CurrentSurvey.Id != 0;
            return true;
        }
        #endregion
        
    }
}
