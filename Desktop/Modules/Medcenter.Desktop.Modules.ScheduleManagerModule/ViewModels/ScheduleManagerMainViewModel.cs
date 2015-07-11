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

namespace Medcenter.Desktop.Modules.ScheduleManagerModule.ViewModels
{
    [Export]
    public class ScheduleManagerMainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        private readonly DelegateCommand<object> _copyPackageCommand;
        private readonly DelegateCommand<object> _addPackageToScheduleCommand;
        private readonly DelegateCommand<object> _removePackageFromScheduleCommand;
        private readonly DelegateCommand<object> _newPackageCommand;
        private readonly DelegateCommand<object> _removePackageCommand;
        private readonly DelegateCommand<object> _savePackageCommand;
        private readonly DelegateCommand<object> _newScheduleCommand;
        private readonly DelegateCommand<object> _removeScheduleCommand;
        private readonly DelegateCommand<object> _saveScheduleCommand;
        #region Properties

        public ICommand CopyPackageCommand
        {
            get { return this._copyPackageCommand; }
        }
        public ICommand AddPackageToScheduleCommand
        {
            get { return this._addPackageToScheduleCommand; }
        }
        public ICommand RemovePackageFromScheduleCommand
        {
            get { return this._removePackageFromScheduleCommand; }
        }
        public ICommand NewPackageCommand
        {
            get { return this._newPackageCommand; }
        }
        public ICommand RemovePackageCommand
        {
            get { return this._removePackageCommand; }
        }
        public ICommand SavePackageCommand
        {
            get { return this._savePackageCommand; }
        }
        public ICommand NewScheduleCommand
        {
            get { return this._newScheduleCommand; }
        }
        public ICommand RemoveScheduleCommand
        {
            get { return this._removeScheduleCommand; }
        }
        public ICommand SaveScheduleCommand
        {
            get { return this._saveScheduleCommand; }
        }
        private List<ResultMessage> _errors;

        public List<ResultMessage> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }
        private ListCollectionView _schedule;
        public ListCollectionView Schedule
        {
            get { return _schedule; }
            set { SetProperty(ref _schedule, value); }
        }
        private ListCollectionView _packagesInSchedule;
        public ListCollectionView PackagesInSchedule
        {
            get { return _packagesInSchedule; }
            set
            {
                SetProperty(ref _packagesInSchedule, value);
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
        private Package _currentPackageInSchedule;

        public Package CurrentPackageInSchedule
        {
            get { return _currentPackageInSchedule; }
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
                SetProperty(ref _currentPackageInSchedule, value);
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

        private Schedule _currentSchedule;

        public Schedule CurrentSchedule
        {
            get { return _currentSchedule; }
            set
            {
                SetProperty(ref _currentSchedule, value);
            }
        }

        #endregion

        [ImportingConstructor]
        public ScheduleManagerMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _copyPackageCommand = new DelegateCommand<object>(CopyPackage);
            _newPackageCommand = new DelegateCommand<object>(NewPackage, CanAddPackage);
            _removePackageCommand = new DelegateCommand<object>(RemovePackage, CanRemovePackage);
            _savePackageCommand = new DelegateCommand<object>(SavePackage);
            _newScheduleCommand = new DelegateCommand<object>(NewSchedule, CanAddSchedule);
            _removeScheduleCommand = new DelegateCommand<object>(RemoveSchedule);
            _saveScheduleCommand = new DelegateCommand<object>(SaveSchedule);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();

            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            //_jsonClient.GetAsync(new PackagesSelect())
            //.Success(ri =>
            //{
            //    //_eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //    PackagesBase = new ListCollectionView(ri.Packages);
            //    _jsonClient.GetAsync(new ScheduleSelect())
            //    .Success(rig =>
            //    {
            //        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //        Schedule = new ListCollectionView(rig.Schedule);
            //        Schedule.CurrentChanged += Schedule_CurrentChanged;

            //        CurrentSchedule = new Schedule();
            //        PackagesInSchedule.CurrentChanged += PackagesInSchedule_CurrentChanged;
            //        PackagesReload(ri.Packages);
            //        Schedule.MoveCurrentTo(null);
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

        private void CopyPackage(object obj)
        {
            CurrentPackage = CurrentPackage.CopyInstance();

        }

        private bool CanRemovePackage(object arg)
        {
            return (CurrentPackage != null) ? CurrentPackage.Name != "" : false;
        }

        private void Schedule_CurrentChanged(object sender, EventArgs e)
        {
            CurrentSchedule = Schedule.CurrentItem != null ? (Schedule)Schedule.CurrentItem : new Schedule();
        }

        private void Packages_CurrentChanged(object sender, EventArgs e)
        {
            CurrentPackage = Packages.CurrentItem != null ? (Package)Packages.CurrentItem : new Package();
        }
        private void PackagesInSchedule_CurrentChanged(object sender, EventArgs e)
        {
            CurrentPackageInSchedule = PackagesInSchedule.CurrentItem != null ? (Package)PackagesInSchedule.CurrentItem : new Package();
        }

        #region Schedule

        private void NewSchedule(object obj)
        {
            CurrentSchedule = new Schedule();
        }

        private void SaveSchedule(object obj)
        {
            bool isNew = CurrentSchedule.Id <= 0;
            Errors = CurrentSchedule.Validate();
            if (Errors.Count == 0)
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                //_jsonClient.PostAsync(new scheduleave { Schedule = CurrentSchedule })
                //    .Success(r =>
                //    {
                //        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                //        CurrentSchedule.Id = r.ScheduleId;
                //        if (isNew) Schedule.AddNewItem(CurrentSchedule);
                //        r.Message.Message = string.Format(r.Message.Message, CurrentSchedule.Name);
                //        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                //        _newScheduleCommand.RaiseCanExecuteChanged();
                //    })
                //    .Error(ex =>
                //    {
                //        throw ex;
                //    });
            }
        }

        private void RemoveSchedule(object obj)
        {
            bool isNew = CurrentSchedule.Id == 0;
            ConfirmationRequest.Raise(
                new Confirmation { Content = "Группа будет удалёна! Вы уверены?", Title = "Удаление группы инспекций." },
                c =>
                {
                    if (c.Confirmed)
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                        if (isNew)
                        {
                            CurrentSchedule = new Schedule();
                            _newScheduleCommand.RaiseCanExecuteChanged();
                        }
                        else
                        {
                            //_jsonClient.GetAsync(new ScheduleDelete { ScheduleId = CurrentSchedule.Id })
                            //.Success(r =>
                            //{
                            //    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false); _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                            //    r.Message.Message = string.Format(r.Message.Message, CurrentSchedule.Name);
                            //    RemovePackageFromScheduleByIGID(CurrentSchedule.Id);
                            //    Schedule.Remove(Schedule.CurrentItem);
                            //    _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                            //    _newScheduleCommand.RaiseCanExecuteChanged();
                            //})
                            //.Error(ex =>
                            //{
                            //    throw ex;
                            //});
                        }
                    }
                });
        }
        private bool CanAddSchedule(object arg)
        {
            //return CurrentSchedule == null || CurrentSchedule.Id != 0;
            return true;
        }
        #endregion

        #region Package
        private void NewPackage(object obj)
        {
            CurrentPackage = new Package();
        }

        private void SavePackage(object obj)
        {
            bool isNew = CurrentPackage.Id <= 0;
            Errors = CurrentPackage.Validate();
            if (Errors.Count == 0)
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                //_jsonClient.PostAsync(new PackageSave { Package = CurrentPackage })
                //    .Success(r =>
                //    {
                //        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                //        CurrentPackage.Id = r.PackageId;
                //        if (isNew)
                //        {
                //            PackagesBase.AddNewItem(CurrentPackage);
                //            PackagesInScheduleRefresh();
                //        }
                //        r.Message.Message = string.Format(r.Message.Message, CurrentPackage.Name);
                //        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                //        _newPackageCommand.RaiseCanExecuteChanged();
                //    })
                //    .Error(ex =>
                //    {
                //        throw ex;
                //    });
            }
        }

        private void RemovePackage(object obj)
        {
            bool isNew = CurrentPackage.Id == 0;
            ConfirmationRequest.Raise(
                new Confirmation { Content = "Инспекция будет удалёна! Вы уверены?", Title = "Удаление инспекции." },
                c =>
                {
                    if (c.Confirmed)
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                        if (isNew)
                        {
                            CurrentPackage = new Package();
                            _newPackageCommand.RaiseCanExecuteChanged();
                        }
                        else
                        {
                            //_jsonClient.GetAsync(new PackageDelete { PackageId = CurrentPackage.Id })
                            //.Success(r =>
                            //{
                            //    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                            //    r.Message.Message = string.Format(r.Message.Message, CurrentPackage.Name);
                            //    RemovePackageFromScheduleByIID(_currentBasePackage.Id);
                            //    PackagesBase.Remove(_currentBasePackage);
                            //    //Packages.Remove(Packages.CurrentItem);
                            //    PackagesInScheduleRefresh();
                            //    _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                            //    _newPackageCommand.RaiseCanExecuteChanged();
                            //})
                            //.Error(ex =>
                            //{
                            //    throw ex;
                            //});
                        }
                    }
                });
        }
        private bool CanAddPackage(object arg)
        {
            //return CurrentPackage == null || CurrentPackage.Id != 0;
            return true;
        }

        #endregion
    }
}
