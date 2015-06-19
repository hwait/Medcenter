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

namespace Medcenter.Desktop.Modules.InspectionsManagerModule.ViewModels
{
    [Export]
    public class InspectionsManagerMainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        private readonly DelegateCommand<object> _copyInspectionCommand;
        private readonly DelegateCommand<object> _addInspectionToGroupCommand;
        private readonly DelegateCommand<object> _removeInspectionFromGroupCommand;
        private readonly DelegateCommand<object> _newInspectionCommand;
        private readonly DelegateCommand<object> _removeInspectionCommand;
        private readonly DelegateCommand<object> _saveInspectionCommand;
        private readonly DelegateCommand<object> _newInspectionGroupCommand;
        private readonly DelegateCommand<object> _removeInspectionGroupCommand;
        private readonly DelegateCommand<object> _saveInspectionGroupCommand;
        #region Properties

        public ICommand CopyInspectionCommand
        {
            get { return this._copyInspectionCommand; }
        }
        public ICommand AddInspectionToGroupCommand
        {
            get { return this._addInspectionToGroupCommand; }
        }
        public ICommand RemoveInspectionFromGroupCommand
        {
            get { return this._removeInspectionFromGroupCommand; }
        }
        public ICommand NewInspectionCommand
        {
            get { return this._newInspectionCommand; }
        }
        public ICommand RemoveInspectionCommand
        {
            get { return this._removeInspectionCommand; }
        }
        public ICommand SaveInspectionCommand
        {
            get { return this._saveInspectionCommand; }
        }
        public ICommand NewInspectionGroupCommand
        {
            get { return this._newInspectionGroupCommand; }
        }
        public ICommand RemoveInspectionGroupCommand
        {
            get { return this._removeInspectionGroupCommand; }
        }
        public ICommand SaveInspectionGroupCommand
        {
            get { return this._saveInspectionGroupCommand; }
        }
        private List<ResultMessage> _errors;

        public List<ResultMessage> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }
        private ListCollectionView _inspectionGroups;
        public ListCollectionView InspectionGroups
        {
            get { return _inspectionGroups; }
            set { SetProperty(ref _inspectionGroups, value); }
        }
        private ListCollectionView _inspectionsInGroup;
        public ListCollectionView InspectionsInGroup
        {
            get { return _inspectionsInGroup; }
            set
            {
                SetProperty(ref _inspectionsInGroup, value); 
            }
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
        private Inspection _currentInspectionInGroup;

        public Inspection CurrentInspectionInGroup
        {
            get { return _currentInspectionInGroup; }
            set
            {
                if (value.Id == 0) _currentBaseInspection = new Inspection();
                else
                {
                    for (int i = 0; i < InspectionsBase.Count; i++)
                    {
                        if (((Inspection) InspectionsBase.GetItemAt(i)).Id == value.Id)
                            _currentBaseInspection = (Inspection) InspectionsBase.GetItemAt(i);
                    }
                }
                SetProperty(ref _currentInspectionInGroup, value);
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
                        if (((Inspection) InspectionsBase.GetItemAt(i)).Id == value.Id)
                            _currentBaseInspection = (Inspection) InspectionsBase.GetItemAt(i);
                    }
                }
                SetProperty(ref _currentInspection, value);

            }
        }
        private Inspection _currentBaseInspection;

        private InspectionGroup _currentInspectionGroup;

        public InspectionGroup CurrentInspectionGroup
        {
            get { return _currentInspectionGroup; }
            set
            {
                SetProperty(ref _currentInspectionGroup, value);
                InspectionsInGroupRefresh();
            }
        }

        #endregion

        [ImportingConstructor]
        public InspectionsManagerMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _copyInspectionCommand = new DelegateCommand<object>(CopyInspection);
            _removeInspectionFromGroupCommand = new DelegateCommand<object>(RemoveInspectionFromGroup);
            _addInspectionToGroupCommand = new DelegateCommand<object>(AddInspectionToGroup);
            _newInspectionCommand = new DelegateCommand<object>(NewInspection, CanAddInspection);
            _removeInspectionCommand = new DelegateCommand<object>(RemoveInspection, CanRemoveInspection);
            _saveInspectionCommand = new DelegateCommand<object>(SaveInspection);
            _newInspectionGroupCommand = new DelegateCommand<object>(NewInspectionGroup, CanAddInspectionGroup);
            _removeInspectionGroupCommand = new DelegateCommand<object>(RemoveInspectionGroup);
            _saveInspectionGroupCommand = new DelegateCommand<object>(SaveInspectionGroup);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();

            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new InspectionsSelect())
            .Success(ri =>
            {
                
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                InspectionsBase = new ListCollectionView(ri.Inspections);
                
                
                _jsonClient.GetAsync(new InspectionGroupsSelect())
                .Success(rig =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    InspectionGroups = new ListCollectionView(rig.InspectionGroups);
                    InspectionGroups.CurrentChanged += InspectionGroups_CurrentChanged;
                    
                    CurrentInspectionGroup = new InspectionGroup();
                    InspectionsInGroup.CurrentChanged += InspectionsInGroup_CurrentChanged;
                    InspectionsReload(ri.Inspections);
                    InspectionGroups.MoveCurrentTo(null);
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

        private void CopyInspection(object obj)
        {
            CurrentInspection = CurrentInspection.CopyInstance();

        }

        private bool CanRemoveInspection(object arg)
        {
            return (CurrentInspection!=null)?CurrentInspection.Name != "":false;
        }
        
        private void InspectionGroups_CurrentChanged(object sender, EventArgs e)
        {
            CurrentInspectionGroup = InspectionGroups.CurrentItem != null ? (InspectionGroup)InspectionGroups.CurrentItem : new InspectionGroup();
        }

        private void Inspections_CurrentChanged(object sender, EventArgs e)
        {
            CurrentInspection = Inspections.CurrentItem != null ? (Inspection)Inspections.CurrentItem : new Inspection();
        }
        private void InspectionsInGroup_CurrentChanged(object sender, EventArgs e)
        {
            CurrentInspectionInGroup = InspectionsInGroup.CurrentItem != null ? (Inspection)InspectionsInGroup.CurrentItem : new Inspection();
        }

        #region InspectionGroup

        private void NewInspectionGroup(object obj)
        {
            CurrentInspectionGroup=new InspectionGroup();
        }

        private void SaveInspectionGroup(object obj)
        {
            bool isNew = CurrentInspectionGroup.Id <= 0;
            Errors = CurrentInspectionGroup.Validate();
            if (Errors.Count == 0)
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.PostAsync(new InspectionGroupSave {InspectionGroup = CurrentInspectionGroup})
                    .Success(r =>
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                        CurrentInspectionGroup.Id = r.InspectionGroupId;
                        if (isNew) InspectionGroups.AddNewItem(CurrentInspectionGroup);
                        r.Message.Message = string.Format(r.Message.Message, CurrentInspectionGroup.Name);
                        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                        _newInspectionGroupCommand.RaiseCanExecuteChanged();
                    })
                    .Error(ex =>
                    {
                        throw ex;
                    });
            }
        }

        private void RemoveInspectionGroup(object obj)
        {
            bool isNew = CurrentInspectionGroup.Id == 0;
            ConfirmationRequest.Raise(
                new Confirmation { Content = "Группа будет удалёна! Вы уверены?", Title = "Удаление группы инспекций." },
                c =>
                {
                    if (c.Confirmed)
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                        if (isNew)
                        {
                            CurrentInspectionGroup = new InspectionGroup();
                            _newInspectionGroupCommand.RaiseCanExecuteChanged();
                        }
                        else
                        {
                            _jsonClient.GetAsync(new InspectionGroupDelete { InspectionGroupId = CurrentInspectionGroup.Id })
                            .Success(r =>
                            {
                                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false); _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                                r.Message.Message = string.Format(r.Message.Message, CurrentInspectionGroup.Name);
                                RemoveInspectionFromGroupByIGID(CurrentInspectionGroup.Id);
                                InspectionGroups.Remove(InspectionGroups.CurrentItem);
                                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                                _newInspectionGroupCommand.RaiseCanExecuteChanged();
                            })
                            .Error(ex =>
                            {
                                throw ex;
                            });
                        }
                    }
                });
        }
        private bool CanAddInspectionGroup(object arg)
        {
            //return CurrentInspectionGroup == null || CurrentInspectionGroup.Id != 0;
            return true;
        }
        #endregion

        #region Inspection

        private bool InspectionFilter(object item)
        {
            Inspection inspection = item as Inspection;
            return inspection.InspectionGroupIds.Contains(CurrentInspectionGroup.Id);
        }
        private void NewInspection(object obj)
        {
            CurrentInspection = new Inspection();
        }

        private void SaveInspection(object obj)
        {
            bool isNew = CurrentInspection.Id <= 0;
            Errors = CurrentInspection.Validate();
            if (Errors.Count == 0)
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.PostAsync(new InspectionSave {Inspection = CurrentInspection})
                    .Success(r =>
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                        CurrentInspection.Id = r.InspectionId;
                        if (isNew)
                        {
                            InspectionsBase.AddNewItem(CurrentInspection);
                            InspectionsInGroupRefresh();
                        }
                        r.Message.Message = string.Format(r.Message.Message, CurrentInspection.Name);
                        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                        _newInspectionCommand.RaiseCanExecuteChanged();
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
                                RemoveInspectionFromGroupByIID(_currentBaseInspection.Id);
                                InspectionsBase.Remove(_currentBaseInspection);
                                //Inspections.Remove(Inspections.CurrentItem);
                                InspectionsInGroupRefresh();
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

        #region Inspections in Group
        private bool InspectionsFilter(object item)
        {
            Inspection inspection = item as Inspection;
            if (CurrentInspectionGroup == null || CurrentInspectionGroup.Id == 0)
                return true;
            else
                return !inspection.InspectionGroupIds.Contains(CurrentInspectionGroup.Id);
        }

        private void ClearInspections()
        {
            Inspections.MoveCurrentTo(null);
            InspectionsInGroup.MoveCurrentTo(null);
            CurrentInspection=new Inspection();
            CurrentInspectionInGroup=new Inspection();
        }
        private void InspectionsReload(List<Inspection> inspections)
        {
            Inspections = new ListCollectionView(inspections);
            Inspections.CurrentChanged += Inspections_CurrentChanged;
            Inspections.MoveCurrentTo(null);
            CurrentInspection = new Inspection();
        }
        private void InspectionsInGroupReload(List<Inspection> inspections)
        {
            InspectionsInGroup = new ListCollectionView(inspections);
            InspectionsInGroup.CurrentChanged += InspectionsInGroup_CurrentChanged;
            InspectionsInGroup.MoveCurrentTo(null);
            CurrentInspectionInGroup = new Inspection();
        }
        private void InspectionsInGroupRefresh()
        {
            var list1 = new List<Inspection>();
            var list2 = new List<Inspection>();
            foreach (Inspection inspection in InspectionsBase)
            {
                if (inspection.InspectionGroupIds!=null&&inspection.InspectionGroupIds.Contains(CurrentInspectionGroup.Id)) list1.Add(inspection);
                else list2.Add(inspection);
            }
            InspectionsInGroupReload(list1);
            InspectionsReload(list2);
        }
        private void AddInspectionToGroup(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new InspectionsGroupsBind { InspectionId = CurrentInspection.Id,InspectionGroupId = CurrentInspectionGroup.Id })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                _currentBaseInspection.InspectionGroupIds.Add(CurrentInspectionGroup.Id);
                //CurrentInspection.InspectionGroupIds.Add(CurrentInspectionGroup.Id);
                InspectionsInGroupRefresh();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void RemoveInspectionFromGroup(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new InspectionsGroupsUnbind { InspectionId = CurrentInspectionInGroup.Id, InspectionGroupId = CurrentInspectionGroup.Id })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                _currentBaseInspection.InspectionGroupIds.Remove(CurrentInspectionGroup.Id);
                //CurrentInspectionInGroup.InspectionGroupIds.Remove(CurrentInspectionGroup.Id);
                InspectionsInGroupRefresh();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void RemoveInspectionFromGroupByIID(int id)
        {
            foreach (InspectionGroup ig in InspectionGroups)
            {
                if (ig.InspectionIds.Contains(id)) ig.InspectionIds.Remove(id);
            }
            //InspectionsInGroupRefresh();
        }
        private void RemoveInspectionFromGroupByIGID(int id)
        {
            foreach (Inspection i in Inspections)
            {
                if (i.InspectionGroupIds.Contains(id)) Inspections.Remove(i);
            }
            InspectionsInGroupRefresh();
        }
        #endregion


      
    }
}
