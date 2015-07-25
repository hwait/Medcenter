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

namespace Medcenter.Desktop.Modules.RegistratureModule.ViewModels
{
    [Export]
    public class RegistratureMainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        private readonly DelegateCommand<object> _newPatientCommand;
        private readonly DelegateCommand<object> _removePatientCommand;
        private readonly DelegateCommand<object> _savePatientCommand;
        private readonly DelegateCommand<object> _copyPatientCommand;
        private readonly DelegateCommand<object> _searchPatientCommand;
        private readonly DelegateCommand<object> _receptionChooseCommand;
        private readonly DelegateCommand<object> _addPackageToReceptionCommand;
        private readonly DelegateCommand<object> _removePackageFromReceptionCommand;


        private readonly DelegateCommand<object> _confirmReceptionCommand;
        private readonly DelegateCommand<object> _receptionPaymentCommand;

        private readonly DelegateCommand<object> _printReceptionCommand;
        private readonly DelegateCommand<object> _confirmPaymentCommand;
        private readonly DelegateCommand<object> _cancelPaymentCommand;
        
 
        #region Properties

        public ICommand ConfirmPaymentCommand
        {
            get { return this._confirmPaymentCommand; }
        }
        public ICommand CancelPaymentCommand
        {
            get { return this._cancelPaymentCommand; }
        }
        public ICommand PrintReceptionCommand
        {
            get { return this._printReceptionCommand; }
        }
        public ICommand NewPatientCommand
        {
            get { return this._newPatientCommand; }
        }
        public ICommand RemovePatientCommand
        {
            get { return this._removePatientCommand; }
        }
        public ICommand SavePatientCommand
        {
            get { return this._savePatientCommand; }
        }
        public ICommand AddPackageToReceptionCommand
        {
            get { return this._addPackageToReceptionCommand; }
        }
        public ICommand RemovePackageFromReceptionCommand
        {
            get { return this._removePackageFromReceptionCommand; }
        }
        public ICommand ConfirmReceptionCommand
        {
            get { return this._confirmReceptionCommand; }
        }
        public ICommand ReceptionPaymentCommand
        {
            get { return this._receptionPaymentCommand; }
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
        private Package _currentPackageInReception;
        public Package CurrentPackageInReception
        {
            get { return _currentPackageInReception; }
            set
            {
                SetProperty(ref _currentPackageInReception, value);
            }
        }


        private ListCollectionView _PatientsBase;
        public ListCollectionView PatientsBase
        {
            get { return _PatientsBase; }
            set { SetProperty(ref _PatientsBase, value); }
        }
        private ListCollectionView _patients;
        public ListCollectionView Patients
        {
            get { return _patients; }
            set { SetProperty(ref _patients, value); }
        }
        private Patient _currentPatientInDoctor;

        public Patient CurrentPatientInDoctor
        {
            get { return _currentPatientInDoctor; }
            set
            {
                if (value.Id == 0) _currentBasePatient = new Patient();
                else
                {
                    for (int i = 0; i < PatientsBase.Count; i++)
                    {
                        if (((Patient)PatientsBase.GetItemAt(i)).Id == value.Id)
                            _currentBasePatient = (Patient)PatientsBase.GetItemAt(i);
                    }
                }
                SetProperty(ref _currentPatientInDoctor, value);
            }
        }
        private Patient _currentPatient;

        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set
            {
                if (value.Id == 0) _currentBasePatient = new Patient();
                else
                {
                    for (int i = 0; i < PatientsBase.Count; i++)
                    {
                        if (((Patient)PatientsBase.GetItemAt(i)).Id == value.Id)
                            _currentBasePatient = (Patient)PatientsBase.GetItemAt(i);
                    }
                }
                SetProperty(ref _currentPatient, value);

            }
        }
        private Patient _currentBasePatient;

        private Doctor _currentDoctor;

        public Doctor CurrentDoctor
        {
            get { return _currentDoctor; }
            set
            {
                SetProperty(ref _currentDoctor, value);
                PatientsInDoctorRefresh();
            }
        }

        #endregion

        [ImportingConstructor]
        public RegistratureMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _searchPatientCommand = new DelegateCommand<object>(SearchPatient);
            _copyPatientCommand = new DelegateCommand<object>(CopyPatient);
            _receptionChooseCommand = new DelegateCommand<object>(ReceptionChoose);
            _newPatientCommand = new DelegateCommand<object>(NewPatient, CanAddPatient);
            _removePatientCommand = new DelegateCommand<object>(RemovePatient, CanRemovePatient);
            _savePatientCommand = new DelegateCommand<object>(SavePatient);
            _addPackageToReceptionCommand = new DelegateCommand<object>(AddPackageToReception);
            _removePackageFromReceptionCommand = new DelegateCommand<object>(RemovePackageFromReception);
            _confirmReceptionCommand = new DelegateCommand<object>(ConfirmReception);
            _receptionPaymentCommand = new DelegateCommand<object>(ReceptionPayment);

            _printReceptionCommand = new DelegateCommand<object>(PrintReception);
            _confirmPaymentCommand = new DelegateCommand<object>(ConfirmPayment);
            _cancelPaymentCommand = new DelegateCommand<object>(CancelPayment);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();

            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            //_jsonClient.GetAsync(new PatientsSelect())
            //.Success(ri =>
            //{
            //    //_eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //    PatientsBase = new ListCollectionView(ri.Patients);
            //    _jsonClient.GetAsync(new DoctorsSelect())
            //    .Success(rig =>
            //    {
            //        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //        Doctors = new ListCollectionView(rig.Doctors);
            //        Doctors.CurrentChanged += Doctors_CurrentChanged;

            //        CurrentDoctor = new Doctor();
            //        PatientsInDoctor.CurrentChanged += PatientsInDoctor_CurrentChanged;
            //        PatientsReload(ri.Patients);
            //        Doctors.MoveCurrentTo(null);
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

        private void CancelPayment(object obj)
        {
            throw new NotImplementedException();
        }

        private void ConfirmPayment(object obj)
        {
            throw new NotImplementedException();
        }

        private void PrintReception(object obj)
        {
            throw new NotImplementedException();
        }

        private void ReceptionPayment(object obj)
        {
            throw new NotImplementedException();
        }

        private void ConfirmReception(object obj)
        {
            throw new NotImplementedException();
        }

        private void RemovePackageFromReception(object obj)
        {
            throw new NotImplementedException();
        }

        private void AddPackageToReception(object obj)
        {
            throw new NotImplementedException();
        }

        private void ReceptionChoose(object obj)
        {
            throw new NotImplementedException();
        }

        private void SearchPatient(object obj)
        {
        }

        private bool CanRemovePatient(object arg)
        {
            return true;
        }

        private void Doctors_CurrentChanged(object sender, EventArgs e)
        {
            CurrentDoctor = Doctors.CurrentItem != null ? (Doctor)Doctors.CurrentItem : new Doctor();
        }

        private void Patients_CurrentChanged(object sender, EventArgs e)
        {
            CurrentPatient = Patients.CurrentItem != null ? (Patient)Patients.CurrentItem : new Patient();
        }
        private void PatientsInDoctor_CurrentChanged(object sender, EventArgs e)
        {
            //CurrentPatientInDoctor = PatientsInDoctor.CurrentItem != null ? (Patient)PatientsInDoctor.CurrentItem : new Patient();
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
                        }
                        else
                        {
                            _jsonClient.GetAsync(new DoctorDelete { DoctorId = CurrentDoctor.Id })
                            .Success(r =>
                            {
                                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false); _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                                r.Message.Message = string.Format(r.Message.Message, CurrentDoctor.Name);
                                CopyPatientByIGID(CurrentDoctor.Id);
                                Doctors.Remove(Doctors.CurrentItem);
                                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
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

        #region Patient
        private void NewPatient(object obj)
        {
            CurrentPatient = new Patient();
        }

        private void SavePatient(object obj)
        {
            //bool isNew = CurrentPatient.Id <= 0;
            //Errors = CurrentPatient.Validate();
            //if (Errors.Count == 0)
            //{
            //    _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            //    _jsonClient.PostAsync(new PatientSave { Patient = CurrentPatient })
            //        .Success(r =>
            //        {
            //            _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //            CurrentPatient.Id = r.PatientId;
            //            if (isNew)
            //            {
            //                PatientsBase.AddNewItem(CurrentPatient);
            //                PatientsInDoctorRefresh();
            //            }
            //            r.Message.Message = string.Format(r.Message.Message, CurrentPatient.Name);
            //            _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
            //            _newPatientCommand.RaiseCanExecuteChanged();
            //        })
            //        .Error(ex =>
            //        {
            //            throw ex;
            //        });
            //}
        }

        private void RemovePatient(object obj)
        {
            //bool isNew = CurrentPatient.Id == 0;
            //ConfirmationRequest.Raise(
            //    new Confirmation { Content = "Инспекция будет удалёна! Вы уверены?", Title = "Удаление инспекции." },
            //    c =>
            //    {
            //        if (c.Confirmed)
            //        {
            //            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            //            if (isNew)
            //            {
            //                CurrentPatient = new Patient();
            //                _newPatientCommand.RaiseCanExecuteChanged();
            //            }
            //            else
            //            {
            //                _jsonClient.GetAsync(new PatientDelete { PatientId = CurrentPatient.Id })
            //                .Success(r =>
            //                {
            //                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //                    r.Message.Message = string.Format(r.Message.Message, CurrentPatient.Name);
            //                    CopyPatientByIID(_currentBasePatient.Id);
            //                    PatientsBase.Remove(_currentBasePatient);
            //                    //Patients.Remove(Patients.CurrentItem);
            //                    PatientsInDoctorRefresh();
            //                    _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
            //                    _newPatientCommand.RaiseCanExecuteChanged();
            //                })
            //                .Error(ex =>
            //                {
            //                    throw ex;
            //                });
            //            }
            //        }
            //    });
        }
        private bool CanAddPatient(object arg)
        {
            //return CurrentPatient == null || CurrentPatient.Id != 0;
            return true;
        }

        #endregion

        #region Patients in Doctor
        private void ClearPatients()
        {
            Patients.MoveCurrentTo(null);
            //PatientsInDoctor.MoveCurrentTo(null);
            CurrentPatient = new Patient();
            CurrentPatientInDoctor = new Patient();
        }
        private void PatientsReload(List<Patient> Patients)
        {
            //Patients = new ListCollectionView(Patients);
            //Patients.CurrentChanged += Patients_CurrentChanged;
            //Patients.MoveCurrentTo(null);
            //CurrentPatient = new Patient();
        }
        private void PatientsInDoctorReload(List<Patient> Patients)
        {
            //PatientsInDoctor = new ListCollectionView(Patients);
            //PatientsInDoctor.CurrentChanged += PatientsInDoctor_CurrentChanged;
            //PatientsInDoctor.MoveCurrentTo(null);
            //CurrentPatientInDoctor = new Patient();
        }
        private void PatientsInDoctorRefresh()
        {
            //var list1 = new List<Patient>();
            //var list2 = new List<Patient>();
            //foreach (Patient Patient in PatientsBase)
            //{
            //    if (Patient.DoctorIds != null && Patient.DoctorIds.Contains(CurrentDoctor.Id)) list1.Add(Patient);
            //    else list2.Add(Patient);
            //}
            //PatientsInDoctorReload(list1);
            //PatientsReload(list2);
        }
        private void AddPatientToDoctor(object obj)
        {
            //_eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            //_jsonClient.GetAsync(new PatientsDoctorsBind { PatientId = CurrentPatient.Id, DoctorId = CurrentDoctor.Id })
            //.Success(r =>
            //{
            //    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //    _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
            //    _currentBasePatient.DoctorIds.Add(CurrentDoctor.Id);
            //    //CurrentPatient.DoctorIds.Add(CurrentDoctor.Id);
            //    PatientsInDoctorRefresh();
            //})
            //.Error(ex =>
            //{
            //    throw ex;
            //});
        }

        private void CopyPatient(object obj)
        {
            //_eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            //_jsonClient.GetAsync(new PatientsDoctorsUnbind { PatientId = CurrentPatientInDoctor.Id, DoctorId = CurrentDoctor.Id })
            //.Success(r =>
            //{
            //    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //    _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
            //    _currentBasePatient.DoctorIds.Remove(CurrentDoctor.Id);
            //    //CurrentPatientInDoctor.DoctorIds.Remove(CurrentDoctor.Id);
            //    PatientsInDoctorRefresh();
            //})
            //.Error(ex =>
            //{
            //    throw ex;
            //});
        }

        private void CopyPatientByIID(int id)
        {
            //foreach (Doctor ig in Doctors)
            //{
            //    if (ig.PatientIds.Contains(id)) ig.PatientIds.Remove(id);
            //}
            //PatientsInDoctorRefresh();
        }
        private void CopyPatientByIGID(int id)
        {
            //foreach (Patient i in Patients)
            //{
            //    if (i.DoctorIds.Contains(id)) Patients.Remove(i);
            //}
            //PatientsInDoctorRefresh();
        }
        #endregion



    }
}
