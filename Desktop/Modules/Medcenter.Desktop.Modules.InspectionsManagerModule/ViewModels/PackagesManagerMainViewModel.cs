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

namespace Medcenter.Desktop.Modules.PackagesManagerModule.ViewModels
{
    [Export]
    public class PackagesManagerMainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        
        private readonly DelegateCommand<object> _addPackageToGroupCommand;
        private readonly DelegateCommand<object> _removePackageFromGroupCommand;
        private readonly DelegateCommand<object> _addInspectionToPackageCommand;
        private readonly DelegateCommand<object> _removeInspectionFromPackageCommand;
        private readonly DelegateCommand<object> _addDiscountToPackageCommand;
        private readonly DelegateCommand<object> _removeDiscountFromPackageCommand;
        private readonly DelegateCommand<object> _newPackageCommand;
        private readonly DelegateCommand<object> _copyPackageCommand;
        private readonly DelegateCommand<object> _removePackageCommand;
        private readonly DelegateCommand<object> _savePackageCommand;
        private readonly DelegateCommand<object> _newInspectionCommand;
        private readonly DelegateCommand<object> _copyInspectionCommand;
        private readonly DelegateCommand<object> _removeInspectionCommand;
        private readonly DelegateCommand<object> _saveInspectionCommand;
        private readonly DelegateCommand<object> _newPackageGroupCommand;
        private readonly DelegateCommand<object> _removePackageGroupCommand;
        private readonly DelegateCommand<object> _savePackageGroupCommand;
        #region Properties
        public ICommand CopyPackageCommand
        {
            get { return this._copyPackageCommand; }
        }
        public ICommand AddPackageToGroupCommand
        {
            get { return this._addPackageToGroupCommand; }
        }
        public ICommand RemovePackageFromGroupCommand
        {
            get { return this._removePackageFromGroupCommand; }
        }
        public ICommand NewPackageCommand
        {
            get { return this._newPackageCommand; }
        }
        public ICommand AddInspectionToPackageCommand
        {
            get { return this._addInspectionToPackageCommand; }
        }
        public ICommand RemoveInspectionFromPackageCommand
        {
            get { return this._removeInspectionFromPackageCommand; }
        }
        public ICommand AddDiscountToPackageCommand
        {
            get { return this._addDiscountToPackageCommand; }
        }
        public ICommand RemoveDiscountFromPackageCommand
        {
            get { return this._removeDiscountFromPackageCommand; }
        }
        public ICommand NewInspectionCommand
        {
            get { return this._newInspectionCommand; }
        }
        public ICommand CopyInspectionCommand
        {
            get { return this._copyInspectionCommand; }
        }
        public ICommand RemoveInspectionCommand
        {
            get { return this._removeInspectionCommand; }
        }
        public ICommand SaveInspectionCommand
        {
            get { return this._saveInspectionCommand; }
        }
        public ICommand RemovePackageCommand
        {
            get { return this._removePackageCommand; }
        }
        public ICommand SavePackageCommand
        {
            get { return this._savePackageCommand; }
        }
        public ICommand NewPackageGroupCommand
        {
            get { return this._newPackageGroupCommand; }
        }
        public ICommand RemovePackageGroupCommand
        {
            get { return this._removePackageGroupCommand; }
        }
        public ICommand SavePackageGroupCommand
        {
            get { return this._savePackageGroupCommand; }
        }
        private List<ResultMessage> _errors;

        public List<ResultMessage> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }
        private ListCollectionView _packageGroups;
        public ListCollectionView PackageGroups
        {
            get { return _packageGroups; }
            set { SetProperty(ref _packageGroups, value); }
        }
        private ListCollectionView _packagesInGroup;
        public ListCollectionView PackagesInGroup
        {
            get { return _packagesInGroup; }
            set
            {
                SetProperty(ref _packagesInGroup, value); 
            }
        }


        private ListCollectionView _inspectionsInPackage;
        public ListCollectionView InspectionsInPackage
        {
            get { return _inspectionsInPackage; }
            set
            {
                SetProperty(ref _inspectionsInPackage, value);
            }
        }
        private Inspection _currentInspectionInPackage;

        public Inspection CurrentInspectionInPackage
        {
            get { return _currentInspectionInPackage; }
            set
            {
                if (value.Id == 0) _currentBaseInspection = new Inspection();
                else
                {
                    for (int i = 0; i < PackagesBase.Count; i++)
                    {
                        if (((Package)PackagesBase.GetItemAt(i)).Id == value.Id)
                            _currentBasePackage = (Package)PackagesBase.GetItemAt(i);
                    }
                }
                SetProperty(ref _currentInspectionInPackage, value);
            }
        }

        private ListCollectionView _discountsInPackage;
        public ListCollectionView DiscountsInPackage
        {
            get { return _discountsInPackage; }
            set
            {
                SetProperty(ref _discountsInPackage, value);
            }
        }
        private Discount _currentDiscountInPackage;

        public Discount CurrentDiscountInPackage
        {
            get { return _currentDiscountInPackage; }
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
                SetProperty(ref _currentDiscountInPackage, value);
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

        private ListCollectionView _discountsBase;
        public ListCollectionView DiscountsBase
        {
            get { return _discountsBase; }
            set { SetProperty(ref _discountsBase, value); }
        }
        private ListCollectionView _discounts;
        public ListCollectionView Discounts
        {
            get { return _discounts; }
            set { SetProperty(ref _discounts, value); }
        }
        private ListCollectionView _inspectionsBase;
        public ListCollectionView InspectionsBase
        {
            get { return _inspectionsBase; }
            set { SetProperty(ref _inspectionsBase, value); }
        }
        private ListCollectionView _inspections;
        public ListCollectionView Inspections
        {
            get { return _inspections; }
            set { SetProperty(ref _inspections, value); }
        }
        private Package _currentPackageInGroup;

        public Package CurrentPackageInGroup
        {
            get { return _currentPackageInGroup; }
            set
            {
                if (value.Id == 0) _currentBasePackage = new Package();
                else
                {
                    for (int i = 0; i < PackagesBase.Count; i++)
                    {
                        if (((Package) PackagesBase.GetItemAt(i)).Id == value.Id)
                            _currentBasePackage = (Package) PackagesBase.GetItemAt(i);
                    }
                }
                SetProperty(ref _currentPackageInGroup, value);
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
                        if (((Package) PackagesBase.GetItemAt(i)).Id == value.Id)
                            _currentBasePackage = (Package) PackagesBase.GetItemAt(i);
                    }
                }
                SetProperty(ref _currentPackage, value);
                InspectionsInPackageRefresh();
                DiscountsInPackageRefresh();
            }
        }
        private Package _currentBasePackage;

        private PackageGroup _currentPackageGroup;

        public PackageGroup CurrentPackageGroup
        {
            get { return _currentPackageGroup; }
            set
            {
                SetProperty(ref _currentPackageGroup, value);
                PackagesInGroupRefresh();
            }
        }
        private Inspection _currentInspection;

        public Inspection CurrentInspection
        {
            get { return _currentInspection; }
            set
            {
                if (value.Id == 0) _currentBaseInspection = new Inspection();
                else
                {
                    for (int i = 0; i < InspectionsBase.Count; i++)
                    {
                        if (((Inspection)InspectionsBase.GetItemAt(i)).Id == value.Id)
                            _currentBaseInspection = (Inspection)InspectionsBase.GetItemAt(i);
                    }
                }
                SetProperty(ref _currentInspection, value);

            }
        }
        private Inspection _currentBaseInspection;

        
        private Discount _currentBaseDiscount;

        private Discount _currentDiscount;

        public Discount CurrentDiscount
        {
            get { return _currentDiscount; }
            set
            {
                SetProperty(ref _currentDiscount, value);
                //DiscountsInPackageRefresh();
            }
        }
        #endregion

        [ImportingConstructor]
        public PackagesManagerMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            
            _removePackageFromGroupCommand = new DelegateCommand<object>(RemovePackageFromGroup);
            _addPackageToGroupCommand = new DelegateCommand<object>(AddPackageToGroup);
            _copyPackageCommand = new DelegateCommand<object>(CopyPackage);
            _newPackageCommand = new DelegateCommand<object>(NewPackage, CanAddPackage);
            _removePackageCommand = new DelegateCommand<object>(RemovePackage, CanRemovePackage);
            _savePackageCommand = new DelegateCommand<object>(SavePackage);
            _copyInspectionCommand = new DelegateCommand<object>(CopyInspection);
            _newInspectionCommand = new DelegateCommand<object>(NewInspection, CanAddInspection);
            _removeInspectionCommand = new DelegateCommand<object>(RemoveInspection, CanRemoveInspection);
            _saveInspectionCommand = new DelegateCommand<object>(SaveInspection);
            _newPackageGroupCommand = new DelegateCommand<object>(NewPackageGroup, CanAddPackageGroup);
            _removePackageGroupCommand = new DelegateCommand<object>(RemovePackageGroup);
            _savePackageGroupCommand = new DelegateCommand<object>(SavePackageGroup);
            _removeDiscountFromPackageCommand = new DelegateCommand<object>(RemoveDiscountFromPackage);
            _addDiscountToPackageCommand = new DelegateCommand<object>(AddDiscountToPackage);
            _removeInspectionFromPackageCommand = new DelegateCommand<object>(RemoveInspectionFromPackage);
            _addInspectionToPackageCommand = new DelegateCommand<object>(AddInspectionToPackage);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();

            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new PackagesSelect())
            .Success(ri =>
            {
                PackagesBase = new ListCollectionView(ri.Packages);

                _jsonClient.GetAsync(new PackageGroupsSelect())
                .Success(rig =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    PackageGroups = new ListCollectionView(rig.PackageGroups);
                    _jsonClient.GetAsync(new InspectionsSelect())
                    .Success(rinsp =>
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                        //Inspections = new ListCollectionView(rinsp.Inspections);
                        InspectionsBase = new ListCollectionView(rinsp.Inspections);
                        
                        _jsonClient.GetAsync(new DiscountsSelect())
                        .Success(rdisc =>
                        {
                            _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                            //Discounts = new ListCollectionView(rdisc.Discounts);
                            DiscountsBase = new ListCollectionView(rdisc.Discounts);
                            
                            PackageGroups.CurrentChanged += PackageGroups_CurrentChanged;
                            CurrentPackageGroup = new PackageGroup();
                            CurrentInspection = new Inspection();
                            //CurrentDiscount = new Discount();
                            
                            //Inspections.CurrentChanged += Inspections_CurrentChanged;
                            //Discounts.CurrentChanged += Discounts_CurrentChanged;
                            PackagesReload(ri.Packages);
                            PackagesInGroup.CurrentChanged += PackagesInGroup_CurrentChanged;
                            PackageGroups.MoveCurrentTo(null);
                            Packages.MoveCurrentTo(null);
                            InspectionsInPackageRefresh();
                            DiscountsInPackageRefresh();
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

        private bool CanRemoveInspection(object arg)
        {
            return (CurrentInspection!=null)?CurrentInspection.Name != "":false;
        }

        private bool CanRemovePackage(object arg)
        {
            return (CurrentPackage != null) ? CurrentPackage.Name != "" : false;
        }

        private void PackageGroups_CurrentChanged(object sender, EventArgs e)
        {
            CurrentPackageGroup = PackageGroups.CurrentItem != null ? (PackageGroup)PackageGroups.CurrentItem : new PackageGroup();
        }

        private void Packages_CurrentChanged(object sender, EventArgs e)
        {
            CurrentPackage = Packages.CurrentItem != null ? (Package)Packages.CurrentItem : new Package();
        }
        private void PackagesInGroup_CurrentChanged(object sender, EventArgs e)
        {
            CurrentPackageInGroup = PackagesInGroup.CurrentItem != null ? (Package)PackagesInGroup.CurrentItem : new Package();
        }


        private void Inspections_CurrentChanged(object sender, EventArgs e)
        {
            CurrentInspection = Inspections.CurrentItem != null ? (Inspection)Inspections.CurrentItem : new Inspection();
        }

        private void InspectionsInPackage_CurrentChanged(object sender, EventArgs e)
        {
            CurrentInspectionInPackage = InspectionsInPackage.CurrentItem != null ? (Inspection)InspectionsInPackage.CurrentItem : new Inspection();
        }
        private void Discounts_CurrentChanged(object sender, EventArgs e)
        {
            CurrentDiscount = Discounts.CurrentItem != null ? (Discount)Discounts.CurrentItem : new Discount();
        }

        private void DiscountsInPackage_CurrentChanged(object sender, EventArgs e)
        {
            CurrentDiscountInPackage = DiscountsInPackage.CurrentItem != null ? (Discount)DiscountsInPackage.CurrentItem : new Discount();
        }
        #region PackageGroup

        private void NewPackageGroup(object obj)
        {
            CurrentPackageGroup=new PackageGroup();
        }

        private void SavePackageGroup(object obj)
        {
            bool isNew = CurrentPackageGroup.Id <= 0;
            Errors = CurrentPackageGroup.Validate();
            if (Errors.Count == 0)
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.PostAsync(new PackageGroupSave {PackageGroup = CurrentPackageGroup})
                    .Success(r =>
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                        CurrentPackageGroup.Id = r.PackageGroupId;
                        if (isNew) PackageGroups.AddNewItem(CurrentPackageGroup);
                        r.Message.Message = string.Format(r.Message.Message, CurrentPackageGroup.Name);
                        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                        _newPackageGroupCommand.RaiseCanExecuteChanged();
                        CurrentPackageGroup = new PackageGroup();
                    })
                    .Error(ex =>
                    {
                        throw ex;
                    });
            }
        }

        private void RemovePackageGroup(object obj)
        {
            bool isNew = CurrentPackageGroup.Id == 0;
            ConfirmationRequest.Raise(
                new Confirmation { Content = "Группа будет удалёна! Вы уверены?", Title = "Удаление группы инспекций." },
                c =>
                {
                    if (c.Confirmed)
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                        if (isNew)
                        {
                            CurrentPackageGroup = new PackageGroup();
                            _newPackageGroupCommand.RaiseCanExecuteChanged();
                        }
                        else
                        {
                            _jsonClient.GetAsync(new PackageGroupDelete { PackageGroupId = CurrentPackageGroup.Id })
                            .Success(r =>
                            {
                                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false); _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                                r.Message.Message = string.Format(r.Message.Message, CurrentPackageGroup.Name);
                                RemovePackageFromGroupByIGID(CurrentPackageGroup.Id);
                                PackageGroups.Remove(PackageGroups.CurrentItem);
                                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                                _newPackageGroupCommand.RaiseCanExecuteChanged();
                            })
                            .Error(ex =>
                            {
                                throw ex;
                            });
                        }
                    }
                });
        }
        private bool CanAddPackageGroup(object arg)
        {
            //return CurrentPackageGroup == null || CurrentPackageGroup.Id != 0;
            return true;
        }
        #endregion

        #region Package

        private bool PackageFilter(object item)
        {
            Package Package = item as Package;
            return Package.PackageGroupIds.Contains(CurrentPackageGroup.Id);
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
                _jsonClient.PostAsync(new PackageSave {Package = CurrentPackage})
                    .Success(r =>
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                        CurrentPackage.Id = r.PackageId;
                        if (isNew)
                        {
                            PackagesBase.AddNewItem(CurrentPackage);
                            PackagesInGroupRefresh();
                        }
                        r.Message.Message = string.Format(r.Message.Message, CurrentPackage.Name);
                        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                        _newPackageCommand.RaiseCanExecuteChanged();
                        CurrentPackage = new Package();
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
                                RemovePackageFromGroupByIID(_currentBasePackage.Id);
                                PackagesBase.Remove(_currentBasePackage);
                                //Packages.Remove(Packages.CurrentItem);
                                PackagesInGroupRefresh();
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

        #region Packages in Group
        private bool PackagesFilter(object item)
        {
            Package Package = item as Package;
            if (CurrentPackageGroup == null || CurrentPackageGroup.Id == 0)
                return true;
            else
                return !Package.PackageGroupIds.Contains(CurrentPackageGroup.Id);
        }

        private void ClearPackages()
        {
            Packages.MoveCurrentTo(null);
            PackagesInGroup.MoveCurrentTo(null);
            CurrentPackage=new Package();
            CurrentPackageInGroup=new Package();
        }
        private void PackagesReload(List<Package> packages)
        {
            Packages = new ListCollectionView(packages);
            Packages.CurrentChanged += Packages_CurrentChanged;
            Packages.MoveCurrentTo(null);
            CurrentPackage = new Package();
        }
        private void PackagesInGroupReload(List<Package> Packages)
        {
            PackagesInGroup = new ListCollectionView(Packages);
            PackagesInGroup.CurrentChanged += PackagesInGroup_CurrentChanged;
            PackagesInGroup.MoveCurrentTo(null);
            CurrentPackageInGroup = new Package();
        }
        private void PackagesInGroupRefresh()
        {
            var list1 = new List<Package>();
            var list2 = new List<Package>();
            foreach (Package package in PackagesBase)
            {
                if (package.PackageGroupIds!=null&&package.PackageGroupIds.Contains(CurrentPackageGroup.Id)) list1.Add(package);
                else list2.Add(package);
            }
            PackagesInGroupReload(list1);
            PackagesReload(list2);
        }
        private void AddPackageToGroup(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new PackagesGroupsBind { PackageId = CurrentPackage.Id,PackageGroupId = CurrentPackageGroup.Id })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                _currentBasePackage.PackageGroupIds.Add(CurrentPackageGroup.Id);
                //CurrentPackage.PackageGroupIds.Add(CurrentPackageGroup.Id);
                PackagesInGroupRefresh();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void RemovePackageFromGroup(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new PackagesGroupsUnbind { PackageId = CurrentPackageInGroup.Id, PackageGroupId = CurrentPackageGroup.Id })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                _currentBasePackage.PackageGroupIds.Remove(CurrentPackageGroup.Id);
                //CurrentPackageInGroup.PackageGroupIds.Remove(CurrentPackageGroup.Id);
                PackagesInGroupRefresh();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void RemovePackageFromGroupByIID(int id)
        {
            foreach (PackageGroup ig in PackageGroups)
            {
                if (ig.PackageIds.Contains(id)) ig.PackageIds.Remove(id);
            }
            //PackagesInGroupRefresh();
        }
        private void RemovePackageFromGroupByIGID(int id)
        {
            foreach (Package i in Packages)
            {
                if (i.PackageGroupIds.Contains(id)) Packages.Remove(i);
            }
            PackagesInGroupRefresh();
        }
        #endregion

        #region Inspection

        private void NewInspection(object obj)
        {
            CurrentInspection = new Inspection();
        }
        private void CopyInspection(object obj)
        {
            CurrentInspection = CurrentInspection.CopyInstance();
        }
        private void SaveInspection(object obj)
        {
            bool isNew = CurrentInspection.Id <= 0;
            Errors = CurrentInspection.Validate();
            if (Errors.Count == 0)
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.PostAsync(new InspectionSave { Inspection = CurrentInspection })
                    .Success(r =>
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                        CurrentInspection.Id = r.InspectionId;
                        if (isNew)
                        {
                            InspectionsBase.AddNewItem(CurrentInspection);
                            InspectionsInPackageRefresh();
                        }
                        r.Message.Message = string.Format(r.Message.Message, CurrentInspection.Name);
                        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                        _newInspectionCommand.RaiseCanExecuteChanged();
                        CurrentInspection = new Inspection();
                    })
                    .Error(ex =>
                    {
                        throw ex;
                    });
            }
        }

        private void RemoveInspection(object obj)
        {
            bool isNew = CurrentInspection.Id == 0;
            ConfirmationRequest.Raise(
                new Confirmation { Content = "Инспекция будет удалёна! Вы уверены?", Title = "Удаление инспекции." },
                c =>
                {
                    if (c.Confirmed)
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                        if (isNew)
                        {
                            CurrentInspection = new Inspection();
                            _newInspectionCommand.RaiseCanExecuteChanged();
                        }
                        else
                        {
                            _jsonClient.GetAsync(new InspectionDelete { InspectionId = CurrentInspection.Id })
                            .Success(r =>
                            {
                                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                                r.Message.Message = string.Format(r.Message.Message, CurrentInspection.Name);
                                RemoveInspectionFromPackageByIID(_currentBaseInspection.Id);
                                InspectionsBase.Remove(_currentBaseInspection);
                                //Inspections.Remove(Inspections.CurrentItem);
                                InspectionsInPackageRefresh();
                                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                                _newInspectionCommand.RaiseCanExecuteChanged();
                            })
                            .Error(ex =>
                            {
                                throw ex;
                            });
                        }
                    }
                });
        }
        private bool CanAddInspection(object arg)
        {
            //return CurrentInspection == null || CurrentInspection.Id != 0;
            return true;
        }

        #endregion

        #region Inspections in Package

        private void ClearInspections()
        {
            Inspections.MoveCurrentTo(null);
            InspectionsInPackage.MoveCurrentTo(null);
            CurrentInspection = new Inspection();
            CurrentInspectionInPackage = new Inspection();
        }
        private void InspectionsReload(List<Inspection> inspections)
        {
            Inspections = new ListCollectionView(inspections);
            Inspections.CurrentChanged += Inspections_CurrentChanged;
            Inspections.MoveCurrentTo(null);
            CurrentInspection = new Inspection();
        }
        private void InspectionsInPackageReload(List<Inspection> Inspections)
        {
            InspectionsInPackage = new ListCollectionView(Inspections);
            InspectionsInPackage.CurrentChanged += InspectionsInPackage_CurrentChanged;
            InspectionsInPackage.MoveCurrentTo(null);
            CurrentInspectionInPackage = new Inspection();
        }
        private void InspectionsInPackageRefresh()
        {
            var list1 = new List<Inspection>();
            var list2 = new List<Inspection>();
            foreach (Inspection inspection in InspectionsBase)
            {
                if (inspection.PackageIds != null && inspection.PackageIds.Contains(CurrentPackage.Id)) list1.Add(inspection);
                else list2.Add(inspection);
            }
            InspectionsInPackageReload(list1);
            InspectionsReload(list2);
        }
        private void AddInspectionToPackage(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new InspectionsPackagesBind { InspectionId = CurrentInspection.Id, PackageId = CurrentPackage.Id })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                _currentBaseInspection.PackageIds.Add(CurrentPackage.Id);
                //CurrentInspection.InspectionPackageIds.Add(CurrentInspectionPackage.Id);
                InspectionsInPackageRefresh();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void RemoveInspectionFromPackage(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new InspectionsPackagesUnbind { InspectionId = CurrentInspectionInPackage.Id, PackageId = CurrentPackage.Id })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                _currentBaseInspection.PackageIds.Remove(CurrentPackage.Id);
                //CurrentInspectionInPackage.InspectionPackageIds.Remove(CurrentInspectionPackage.Id);
                InspectionsInPackageRefresh();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void RemoveInspectionFromPackageByIID(int id)
        {
            foreach (Package p in Packages)
            {
                if (p.InspectionIds.Contains(id)) p.InspectionIds.Remove(id);
            }
            //InspectionsInPackageRefresh();
        }
        private void RemoveInspectionFromPackageByPID(int id)
        {
            foreach (Inspection i in Inspections)
            {
                if (i.PackageIds.Contains(id)) Inspections.Remove(i);
            }
            InspectionsInPackageRefresh();
        }
        #endregion

        #region Discounts in Package

        private void ClearDiscounts()
        {
            Discounts.MoveCurrentTo(null);
            DiscountsInPackage.MoveCurrentTo(null);
            CurrentDiscount = new Discount();
            CurrentDiscountInPackage = new Discount();
        }
        private void DiscountsReload(List<Discount> discounts)
        {
            Discounts = new ListCollectionView(discounts);
            Discounts.CurrentChanged += Discounts_CurrentChanged;
            Discounts.MoveCurrentTo(null);
            CurrentDiscount = new Discount();
        }
        private void DiscountsInPackageReload(List<Discount> discounts)
        {
            DiscountsInPackage = new ListCollectionView(discounts);
            DiscountsInPackage.CurrentChanged += DiscountsInPackage_CurrentChanged;
            DiscountsInPackage.MoveCurrentTo(null);
            CurrentDiscountInPackage = new Discount();
        }
        private void DiscountsInPackageRefresh()
        {
            var list1 = new List<Discount>();
            var list2 = new List<Discount>();
            foreach (Discount discount in DiscountsBase)
            {
                if (discount.PackageIds != null && discount.PackageIds.Contains(CurrentPackage.Id)) list1.Add(discount);
                else list2.Add(discount);
            }
            DiscountsInPackageReload(list1);
            DiscountsReload(list2);
        }
        private void AddDiscountToPackage(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new DiscountsPackagesBind { DiscountId = CurrentDiscount.Id, PackageId = CurrentPackage.Id })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                _currentBaseDiscount.PackageIds.Add(CurrentPackage.Id);
                //CurrentDiscount.DiscountPackageIds.Add(CurrentDiscountPackage.Id);
                DiscountsInPackageRefresh();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void RemoveDiscountFromPackage(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new DiscountsPackagesUnbind { DiscountId = CurrentDiscountInPackage.Id, PackageId = CurrentPackage.Id })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                _currentBaseDiscount.PackageIds.Remove(CurrentPackage.Id);
                //CurrentDiscountInPackage.DiscountPackageIds.Remove(CurrentDiscountPackage.Id);
                DiscountsInPackageRefresh();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void RemoveDiscountFromPackageByIID(int id)
        {
            foreach (Package p in Packages)
            {
                if (p.DiscountIds.Contains(id)) p.DiscountIds.Remove(id);
            }
            //DiscountsInPackageRefresh();
        }
        private void RemoveDiscountFromPackageByPID(int id)
        {
            foreach (Discount i in Discounts)
            {
                if (i.PackageIds.Contains(id)) Discounts.Remove(i);
            }
            DiscountsInPackageRefresh();
        }
        #endregion
    }
}
