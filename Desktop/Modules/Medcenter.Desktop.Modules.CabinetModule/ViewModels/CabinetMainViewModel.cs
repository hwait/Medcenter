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
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        private readonly DelegateCommand<object> _copyPackageCommand;
        private readonly DelegateCommand<object> _addPackageToDoctorCommand;
        private readonly DelegateCommand<object> _removePackageFromDoctorCommand;
        private readonly DelegateCommand<object> _newPackageCommand;
        private readonly DelegateCommand<object> _removePackageCommand;
        private readonly DelegateCommand<object> _savePackageCommand;
        private readonly DelegateCommand<object> _newDoctorCommand;
        private readonly DelegateCommand<object> _removeDoctorCommand;
        private readonly DelegateCommand<object> _saveDoctorCommand;
        #region Properties

        public ICommand CopyPackageCommand
        {
            get { return this._copyPackageCommand; }
        }
        public ICommand AddPackageToDoctorCommand
        {
            get { return this._addPackageToDoctorCommand; }
        }
        public ICommand RemovePackageFromDoctorCommand
        {
            get { return this._removePackageFromDoctorCommand; }
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
        public ICommand NewDoctorCommand
        {
            get { return this._newDoctorCommand; }
        }
        public ICommand RemoveDoctorCommand
        {
            get { return this._removeDoctorCommand; }
        }
        public ICommand SaveDoctorCommand
        {
            get { return this._saveDoctorCommand; }
        }
        private List<ResultMessage> _errors;

        public List<ResultMessage> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }
        private ListCollectionView _doctors;
        public ListCollectionView Doctors
        {
            get { return _doctors; }
            set { SetProperty(ref _doctors, value); }
        }
        private ListCollectionView _packagesInDoctor;
        public ListCollectionView PackagesInDoctor
        {
            get { return _packagesInDoctor; }
            set
            {
                SetProperty(ref _packagesInDoctor, value);
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
        private Package _currentPackageInDoctor;

        public Package CurrentPackageInDoctor
        {
            get { return _currentPackageInDoctor; }
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
                SetProperty(ref _currentPackageInDoctor, value);
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

        private Doctor _currentDoctor;

        public Doctor CurrentDoctor
        {
            get { return _currentDoctor; }
            set
            {
                SetProperty(ref _currentDoctor, value);
                PackagesInDoctorRefresh();
            }
        }

        #endregion

        [ImportingConstructor]
        public CabinetMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _copyPackageCommand = new DelegateCommand<object>(CopyPackage);
            _removePackageFromDoctorCommand = new DelegateCommand<object>(RemovePackageFromDoctor);
            _addPackageToDoctorCommand = new DelegateCommand<object>(AddPackageToDoctor);
            _newPackageCommand = new DelegateCommand<object>(NewPackage, CanAddPackage);
            _removePackageCommand = new DelegateCommand<object>(RemovePackage, CanRemovePackage);
            _savePackageCommand = new DelegateCommand<object>(SavePackage);
            _newDoctorCommand = new DelegateCommand<object>(NewDoctor, CanAddDoctor);
            _removeDoctorCommand = new DelegateCommand<object>(RemoveDoctor);
            _saveDoctorCommand = new DelegateCommand<object>(SaveDoctor);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();

            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new PackagesSelect())
            .Success(ri =>
            {
                //_eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                PackagesBase = new ListCollectionView(ri.Packages);
                _jsonClient.GetAsync(new DoctorsSelect())
                .Success(rig =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    Doctors = new ListCollectionView(rig.Doctors);
                    Doctors.CurrentChanged += Doctors_CurrentChanged;

                    CurrentDoctor = new Doctor();
                    PackagesInDoctor.CurrentChanged += PackagesInDoctor_CurrentChanged;
                    PackagesReload(ri.Packages);
                    Doctors.MoveCurrentTo(null);
                })
                .Error(ex =>
                {
                    throw ex;
                });
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void CopyPackage(object obj)
        {
            CurrentPackage = CurrentPackage.CopyInstance();

        }

        private bool CanRemovePackage(object arg)
        {
            return (CurrentPackage != null) ? CurrentPackage.Name != "" : false;
        }

        private void Doctors_CurrentChanged(object sender, EventArgs e)
        {
            CurrentDoctor = Doctors.CurrentItem != null ? (Doctor)Doctors.CurrentItem : new Doctor();
        }

        private void Packages_CurrentChanged(object sender, EventArgs e)
        {
            CurrentPackage = Packages.CurrentItem != null ? (Package)Packages.CurrentItem : new Package();
        }
        private void PackagesInDoctor_CurrentChanged(object sender, EventArgs e)
        {
            CurrentPackageInDoctor = PackagesInDoctor.CurrentItem != null ? (Package)PackagesInDoctor.CurrentItem : new Package();
        }

        #region Doctor

        private void NewDoctor(object obj)
        {
            CurrentDoctor = new Doctor();
        }

        private void SaveDoctor(object obj)
        {
            bool isNew = CurrentDoctor.Id <= 0;
            Errors = CurrentDoctor.Validate();
            if (Errors.Count == 0)
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.PostAsync(new DoctorSave { Doctor = CurrentDoctor })
                    .Success(r =>
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                        CurrentDoctor.Id = r.DoctorId;
                        if (isNew) Doctors.AddNewItem(CurrentDoctor);
                        r.Message.Message = string.Format(r.Message.Message, CurrentDoctor.Name);
                        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                        _newDoctorCommand.RaiseCanExecuteChanged();
                    })
                    .Error(ex =>
                    {
                        throw ex;
                    });
            }
        }

        private void RemoveDoctor(object obj)
        {
            bool isNew = CurrentDoctor.Id == 0;
            ConfirmationRequest.Raise(
                new Confirmation { Content = "Группа будет удалёна! Вы уверены?", Title = "Удаление группы инспекций." },
                c =>
                {
                    if (c.Confirmed)
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                        if (isNew)
                        {
                            CurrentDoctor = new Doctor();
                            _newDoctorCommand.RaiseCanExecuteChanged();
                        }
                        else
                        {
                            _jsonClient.GetAsync(new DoctorDelete { DoctorId = CurrentDoctor.Id })
                            .Success(r =>
                            {
                                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false); _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                                r.Message.Message = string.Format(r.Message.Message, CurrentDoctor.Name);
                                RemovePackageFromDoctorByIGID(CurrentDoctor.Id);
                                Doctors.Remove(Doctors.CurrentItem);
                                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                                _newDoctorCommand.RaiseCanExecuteChanged();
                            })
                            .Error(ex =>
                            {
                                throw ex;
                            });
                        }
                    }
                });
        }
        private bool CanAddDoctor(object arg)
        {
            //return CurrentDoctor == null || CurrentDoctor.Id != 0;
            return true;
        }
        #endregion

        #region Package

        private bool PackageFilter(object item)
        {
            Package Package = item as Package;
            return Package.DoctorIds.Contains(CurrentDoctor.Id);
        }
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
                _jsonClient.PostAsync(new PackageSave { Package = CurrentPackage })
                    .Success(r =>
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                        CurrentPackage.Id = r.PackageId;
                        if (isNew)
                        {
                            PackagesBase.AddNewItem(CurrentPackage);
                            PackagesInDoctorRefresh();
                        }
                        r.Message.Message = string.Format(r.Message.Message, CurrentPackage.Name);
                        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                        _newPackageCommand.RaiseCanExecuteChanged();
                    })
                    .Error(ex =>
                    {
                        throw ex;
                    });
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
                            _jsonClient.GetAsync(new PackageDelete { PackageId = CurrentPackage.Id })
                            .Success(r =>
                            {
                                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                                r.Message.Message = string.Format(r.Message.Message, CurrentPackage.Name);
                                RemovePackageFromDoctorByIID(_currentBasePackage.Id);
                                PackagesBase.Remove(_currentBasePackage);
                                //Packages.Remove(Packages.CurrentItem);
                                PackagesInDoctorRefresh();
                                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                                _newPackageCommand.RaiseCanExecuteChanged();
                            })
                            .Error(ex =>
                            {
                                throw ex;
                            });
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

        #region Packages in Doctor
        private bool PackagesFilter(object item)
        {
            Package Package = item as Package;
            if (CurrentDoctor == null || CurrentDoctor.Id == 0)
                return true;
            else
                return !Package.DoctorIds.Contains(CurrentDoctor.Id);
        }

        private void ClearPackages()
        {
            Packages.MoveCurrentTo(null);
            PackagesInDoctor.MoveCurrentTo(null);
            CurrentPackage = new Package();
            CurrentPackageInDoctor = new Package();
        }
        private void PackagesReload(List<Package> packages)
        {
            Packages = new ListCollectionView(packages);
            Packages.CurrentChanged += Packages_CurrentChanged;
            Packages.MoveCurrentTo(null);
            CurrentPackage = new Package();
        }
        private void PackagesInDoctorReload(List<Package> Packages)
        {
            PackagesInDoctor = new ListCollectionView(Packages);
            PackagesInDoctor.CurrentChanged += PackagesInDoctor_CurrentChanged;
            PackagesInDoctor.MoveCurrentTo(null);
            CurrentPackageInDoctor = new Package();
        }
        private void PackagesInDoctorRefresh()
        {
            var list1 = new List<Package>();
            var list2 = new List<Package>();
            foreach (Package Package in PackagesBase)
            {
                if (Package.DoctorIds != null && Package.DoctorIds.Contains(CurrentDoctor.Id)) list1.Add(Package);
                else list2.Add(Package);
            }
            PackagesInDoctorReload(list1);
            PackagesReload(list2);
        }
        private void AddPackageToDoctor(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new PackagesDoctorsBind { PackageId = CurrentPackage.Id, DoctorId = CurrentDoctor.Id })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                _currentBasePackage.DoctorIds.Add(CurrentDoctor.Id);
                //CurrentPackage.DoctorIds.Add(CurrentDoctor.Id);
                PackagesInDoctorRefresh();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void RemovePackageFromDoctor(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new PackagesDoctorsUnbind { PackageId = CurrentPackageInDoctor.Id, DoctorId = CurrentDoctor.Id })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                _currentBasePackage.DoctorIds.Remove(CurrentDoctor.Id);
                //CurrentPackageInDoctor.DoctorIds.Remove(CurrentDoctor.Id);
                PackagesInDoctorRefresh();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void RemovePackageFromDoctorByIID(int id)
        {
            foreach (Doctor ig in Doctors)
            {
                if (ig.PackageIds.Contains(id)) ig.PackageIds.Remove(id);
            }
            //PackagesInDoctorRefresh();
        }
        private void RemovePackageFromDoctorByIGID(int id)
        {
            foreach (Package i in Packages)
            {
                if (i.DoctorIds.Contains(id)) Packages.Remove(i);
            }
            PackagesInDoctorRefresh();
        }
        #endregion



    }
}
