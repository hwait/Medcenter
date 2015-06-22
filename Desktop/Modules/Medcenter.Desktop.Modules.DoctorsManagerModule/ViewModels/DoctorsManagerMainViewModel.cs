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

namespace Medcenter.Desktop.Modules.DoctorsManagerModule.ViewModels
{
    [Export]
    public class DoctorsManagerMainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        private readonly DelegateCommand<object> _copyInspectionCommand;
        private readonly DelegateCommand<object> _addInspectionToDoctorCommand;
        private readonly DelegateCommand<object> _removeInspectionFromDoctorCommand;
        private readonly DelegateCommand<object> _newInspectionCommand;
        private readonly DelegateCommand<object> _removeInspectionCommand;
        private readonly DelegateCommand<object> _saveInspectionCommand;
        private readonly DelegateCommand<object> _newDoctorCommand;
        private readonly DelegateCommand<object> _removeDoctorCommand;
        private readonly DelegateCommand<object> _saveDoctorCommand;
        #region Properties

        public ICommand CopyInspectionCommand
        {
            get { return this._copyInspectionCommand; }
        }
        public ICommand AddInspectionToDoctorCommand
        {
            get { return this._addInspectionToDoctorCommand; }
        }
        public ICommand RemoveInspectionFromDoctorCommand
        {
            get { return this._removeInspectionFromDoctorCommand; }
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
        private ListCollectionView _Doctors;
        public ListCollectionView Doctors
        {
            get { return _Doctors; }
            set { SetProperty(ref _Doctors, value); }
        }
        private ListCollectionView _InspectionsInDoctor;
        public ListCollectionView InspectionsInDoctor
        {
            get { return _InspectionsInDoctor; }
            set
            {
                SetProperty(ref _InspectionsInDoctor, value);
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
        private Inspection _CurrentInspectionInDoctor;

        public Inspection CurrentInspectionInDoctor
        {
            get { return _CurrentInspectionInDoctor; }
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
                SetProperty(ref _CurrentInspectionInDoctor, value);
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

        private Doctor _CurrentDoctor;

        public Doctor CurrentDoctor
        {
            get { return _CurrentDoctor; }
            set
            {
                SetProperty(ref _CurrentDoctor, value);
                InspectionsInDoctorRefresh();
            }
        }

        #endregion

        [ImportingConstructor]
        public DoctorsManagerMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _copyInspectionCommand = new DelegateCommand<object>(CopyInspection);
            _removeInspectionFromDoctorCommand = new DelegateCommand<object>(RemoveInspectionFromDoctor);
            _addInspectionToDoctorCommand = new DelegateCommand<object>(AddInspectionToDoctor);
            _newInspectionCommand = new DelegateCommand<object>(NewInspection, CanAddInspection);
            _removeInspectionCommand = new DelegateCommand<object>(RemoveInspection, CanRemoveInspection);
            _saveInspectionCommand = new DelegateCommand<object>(SaveInspection);
            _newDoctorCommand = new DelegateCommand<object>(NewDoctor, CanAddDoctor);
            _removeDoctorCommand = new DelegateCommand<object>(RemoveDoctor);
            _saveDoctorCommand = new DelegateCommand<object>(SaveDoctor);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();

            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new InspectionsSelect())
            .Success(ri =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                InspectionsBase = new ListCollectionView(ri.Inspections);
                _jsonClient.GetAsync(new DoctorsSelect())
                .Success(rig =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    Doctors = new ListCollectionView(rig.Doctors);
                    Doctors.CurrentChanged += Doctors_CurrentChanged;

                    CurrentDoctor = new Doctor();
                    InspectionsInDoctor.CurrentChanged += InspectionsInDoctor_CurrentChanged;
                    InspectionsReload(ri.Inspections);
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

        private void CopyInspection(object obj)
        {
            CurrentInspection = CurrentInspection.CopyInstance();

        }

        private bool CanRemoveInspection(object arg)
        {
            return (CurrentInspection != null) ? CurrentInspection.Name != "" : false;
        }

        private void Doctors_CurrentChanged(object sender, EventArgs e)
        {
            CurrentDoctor = Doctors.CurrentItem != null ? (Doctor)Doctors.CurrentItem : new Doctor();
        }

        private void Inspections_CurrentChanged(object sender, EventArgs e)
        {
            CurrentInspection = Inspections.CurrentItem != null ? (Inspection)Inspections.CurrentItem : new Inspection();
        }
        private void InspectionsInDoctor_CurrentChanged(object sender, EventArgs e)
        {
            CurrentInspectionInDoctor = InspectionsInDoctor.CurrentItem != null ? (Inspection)InspectionsInDoctor.CurrentItem : new Inspection();
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
                                RemoveInspectionFromDoctorByIGID(CurrentDoctor.Id);
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

        #region Inspection

        private bool InspectionFilter(object item)
        {
            Inspection inspection = item as Inspection;
            return inspection.DoctorIds.Contains(CurrentDoctor.Id);
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
                _jsonClient.PostAsync(new InspectionSave { Inspection = CurrentInspection })
                    .Success(r =>
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                        CurrentInspection.Id = r.InspectionId;
                        if (isNew)
                        {
                            InspectionsBase.AddNewItem(CurrentInspection);
                            InspectionsInDoctorRefresh();
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
                                RemoveInspectionFromDoctorByIID(_currentBaseInspection.Id);
                                InspectionsBase.Remove(_currentBaseInspection);
                                //Inspections.Remove(Inspections.CurrentItem);
                                InspectionsInDoctorRefresh();
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

        #region Inspections in Doctor
        private bool InspectionsFilter(object item)
        {
            Inspection inspection = item as Inspection;
            if (CurrentDoctor == null || CurrentDoctor.Id == 0)
                return true;
            else
                return !inspection.DoctorIds.Contains(CurrentDoctor.Id);
        }

        private void ClearInspections()
        {
            Inspections.MoveCurrentTo(null);
            InspectionsInDoctor.MoveCurrentTo(null);
            CurrentInspection = new Inspection();
            CurrentInspectionInDoctor = new Inspection();
        }
        private void InspectionsReload(List<Inspection> inspections)
        {
            Inspections = new ListCollectionView(inspections);
            Inspections.CurrentChanged += Inspections_CurrentChanged;
            Inspections.MoveCurrentTo(null);
            CurrentInspection = new Inspection();
        }
        private void InspectionsInDoctorReload(List<Inspection> inspections)
        {
            InspectionsInDoctor = new ListCollectionView(inspections);
            InspectionsInDoctor.CurrentChanged += InspectionsInDoctor_CurrentChanged;
            InspectionsInDoctor.MoveCurrentTo(null);
            CurrentInspectionInDoctor = new Inspection();
        }
        private void InspectionsInDoctorRefresh()
        {
            var list1 = new List<Inspection>();
            var list2 = new List<Inspection>();
            foreach (Inspection inspection in InspectionsBase)
            {
                if (inspection.DoctorIds != null && inspection.DoctorIds.Contains(CurrentDoctor.Id)) list1.Add(inspection);
                else list2.Add(inspection);
            }
            InspectionsInDoctorReload(list1);
            InspectionsReload(list2);
        }
        private void AddInspectionToDoctor(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new InspectionsDoctorsBind { InspectionId = CurrentInspection.Id, DoctorId = CurrentDoctor.Id })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                _currentBaseInspection.DoctorIds.Add(CurrentDoctor.Id);
                //CurrentInspection.DoctorIds.Add(CurrentDoctor.Id);
                InspectionsInDoctorRefresh();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void RemoveInspectionFromDoctor(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new InspectionsDoctorsUnbind { InspectionId = CurrentInspectionInDoctor.Id, DoctorId = CurrentDoctor.Id })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                _currentBaseInspection.DoctorIds.Remove(CurrentDoctor.Id);
                //CurrentInspectionInDoctor.DoctorIds.Remove(CurrentDoctor.Id);
                InspectionsInDoctorRefresh();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void RemoveInspectionFromDoctorByIID(int id)
        {
            foreach (Doctor ig in Doctors)
            {
                if (ig.InspectionIds.Contains(id)) ig.InspectionIds.Remove(id);
            }
            //InspectionsInDoctorRefresh();
        }
        private void RemoveInspectionFromDoctorByIGID(int id)
        {
            foreach (Inspection i in Inspections)
            {
                if (i.DoctorIds.Contains(id)) Inspections.Remove(i);
            }
            InspectionsInDoctorRefresh();
        }
        #endregion



    }
}
