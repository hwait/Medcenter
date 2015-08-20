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
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        private readonly DelegateCommand<object> _insertPhraseCommand;
        private readonly DelegateCommand<object> _removePhraseCommand;
        private readonly DelegateCommand<object> _cutPhraseCommand;
        private readonly DelegateCommand<object> _toggleLastParagraphCommand;
        private readonly DelegateCommand<object> _toggleFirstParagraphCommand;
        private readonly DelegateCommand<object> _toggleShowPositionCommand;
        private readonly DelegateCommand<object> _previewSurveyCommand;
        private readonly DelegateCommand<object> _newSurveyCommand;
        private readonly DelegateCommand<object> _removeSurveyCommand;
        private readonly DelegateCommand<object> _saveSurveyCommand;
        #region Properties

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
        public ICommand _ToggleShowPositionCommand
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
        private List<ResultMessage> _errors;

        public List<ResultMessage> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }
        private ListCollectionView _Surveys;
        public ListCollectionView Surveys
        {
            get { return _Surveys; }
            set { SetProperty(ref _Surveys, value); }
        }
        private ListCollectionView _packagesInSurvey;
        public ListCollectionView PackagesInSurvey
        {
            get { return _packagesInSurvey; }
            set
            {
                SetProperty(ref _packagesInSurvey, value);
            }
        }
        private ListCollectionView _packagesBase;
        public ListCollectionView PackagesBase
        {
            get { return _packagesBase; }
            set { SetProperty(ref _packagesBase, value); }
        }
        private ListCollectionView _packages;
        public ListCollectionView Packages
        {
            get { return _packages; }
            set { SetProperty(ref _packages, value); }
        }
        private Package _currentPackageInSurvey;

        public Package CurrentPackageInSurvey
        {
            get { return _currentPackageInSurvey; }
            set
            {
                if (value.Id == 0) _currentBasePackage = new Package();
                else
                {
                    for (int i = 0; i < PackagesBase.Count; i++)
                    {
                        if (((Package)PackagesBase.GetItemAt(i)).Id == value.Id)
                            _currentBasePackage = (Package)PackagesBase.GetItemAt(i);
                    }
                }
                SetProperty(ref _currentPackageInSurvey, value);
            }
        }
        private Package _currentPackage;

        public Package CurrentPackage
        {
            get { return _currentPackage; }
            set
            {
                if (value.Id == 0) _currentBasePackage = new Package();
                else
                {
                    for (int i = 0; i < PackagesBase.Count; i++)
                    {
                        if (((Package)PackagesBase.GetItemAt(i)).Id == value.Id)
                            _currentBasePackage = (Package)PackagesBase.GetItemAt(i);
                    }
                }
                SetProperty(ref _currentPackage, value);

            }
        }
        private Package _currentBasePackage;

        private Survey _currentSurvey;

        public Survey CurrentSurvey
        {
            get { return _currentSurvey; }
            set
            {
                SetProperty(ref _currentSurvey, value);
            }
        }

        #endregion

        [ImportingConstructor]
        public SurveysManagerMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _insertPhraseCommand = new DelegateCommand<object>(InsertPhrase);
            _removePhraseCommand = new DelegateCommand<object>(RemovePhrase);
            _cutPhraseCommand = new DelegateCommand<object>(CutPhrase);
            _toggleFirstParagraphCommand = new DelegateCommand<object>(ToggleFirstParagraph);
            _toggleLastParagraphCommand = new DelegateCommand<object>(ToggleLastParagraph);
            _toggleShowPositionCommand = new DelegateCommand<object>(ToggleShowPosition);
            _previewSurveyCommand = new DelegateCommand<object>(PreviewSurvey);
            _newSurveyCommand = new DelegateCommand<object>(NewSurvey);
            _removeSurveyCommand = new DelegateCommand<object>(RemoveSurvey);
            _saveSurveyCommand = new DelegateCommand<object>(SaveSurvey);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();

            CurrentSurvey=new Survey();

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

        private void PreviewSurvey(object obj)
        {
            throw new NotImplementedException();
        }

        private void ToggleShowPosition(object obj)
        {
            throw new NotImplementedException();
        }

        private void ToggleLastParagraph(object obj)
        {
            throw new NotImplementedException();
        }

        private void ToggleFirstParagraph(object obj)
        {
            throw new NotImplementedException();
        }

        private void CutPhrase(object obj)
        {
            throw new NotImplementedException();
        }

        private void RemovePhrase(object obj)
        {
            throw new NotImplementedException();
        }

        private void InsertPhrase(object obj)
        {
            throw new NotImplementedException();
        }

        #region Survey

        private void NewSurvey(object obj)
        {
            CurrentSurvey = new Survey();
        }

        private void SaveSurvey(object obj)
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

        private void RemoveSurvey(object obj)
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
