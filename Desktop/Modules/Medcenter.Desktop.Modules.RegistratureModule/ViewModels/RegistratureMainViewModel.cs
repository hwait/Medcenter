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
using Medcenter.Desktop.Modules.RegistratureModule.Model;
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

        #region Commands

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
        private readonly DelegateCommand<object> _saveCityCommand;
        private readonly DelegateCommand<object> _confirmPatientCommand;
        
        

        #region Commands Declaration
        public ICommand ConfirmPatientCommand
        {
            get { return this._confirmPatientCommand; }
        }
        public ICommand SaveCityCommand
        {
            get { return this._saveCityCommand; }
        }
        public ICommand CopyPatientCommand
        {
            get { return this._copyPatientCommand; }
        }

        public ICommand SearchPatientCommand
        {
            get { return this._searchPatientCommand; }
        }

        public ICommand ReceptionChooseCommand
        {
            get { return this._receptionChooseCommand; }
        }

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

        #endregion

        #endregion

        #region Properties
        private int _startHour, _endHour;
        private string[] _cabinets;
        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get
            {
                return _currentDate;
            }
            set
            {
                SetProperty(ref _currentDate, value);
                SchedulesReload();
            }
        }

        private bool _isNewPatientPanelVisible;
        private bool _isSearchPatientPanelVisible;
        //private bool _isAddPackagesPanelVisible;
        private bool _isReceptionPanelVisible;
        private bool _isPaymentsPanelVisible;

        public bool IsNewPatientPanelVisible
        {
            get { return _isNewPatientPanelVisible; }
            set { SetProperty(ref _isNewPatientPanelVisible, value); }
        }
        public bool IsSearchPatientPanelVisible
        {
            get { return _isSearchPatientPanelVisible; }
            set { SetProperty(ref _isSearchPatientPanelVisible, value); }
        }
        //public bool IsAddPackagesPanelVisible
        //{
        //    get { return _isAddPackagesPanelVisible; }
        //    set { SetProperty(ref _isAddPackagesPanelVisible, value); }
        //}
        public bool IsReceptionPanelVisible
        {
            get { return _isReceptionPanelVisible; }
            set { SetProperty(ref _isReceptionPanelVisible, value); }
        }
        public bool IsPaymentsPanelVisible
        {
            get { return _isPaymentsPanelVisible; }
            set { SetProperty(ref _isPaymentsPanelVisible, value); }
        }

        #region Packages and Reception

        private Package _currentPackageInReception;

        public Package CurrentPackageInReception
        {
            get { return _currentPackageInReception; }
            set { SetProperty(ref _currentPackageInReception, value); }
        }

        private ListCollectionView _packagesBase;

        public ListCollectionView PackagesBase
        {
            get { return _packagesBase; }
            set { SetProperty(ref _packagesBase, value); }
        }

        #endregion

        #region Patients

        private string _patientSearchText;

        public string PatientSearchText
        {
            get { return _patientSearchText; }
            set { SetProperty(ref _patientSearchText, value); }
        }
        private List<City> _cities;

        public List<City> Cities
        {
            get { return _cities; }
            set { SetProperty(ref _cities, value); }
        }
        private List<Schedule> _schedules;

        public List<Schedule> Schedules
        {
            get { return _schedules; }
            set { _schedules=value; }
        }
        private List<Reception> _receptions;

        public List<Reception> Receptions
        {
            get { return _receptions; }
            set { _receptions = value; }
        }

        private ObservableCollection<CabinetReceptions> _dayReceptions;

        public ObservableCollection<CabinetReceptions> DayReceptions
        {
            get { return _dayReceptions; }
            set { SetProperty(ref _dayReceptions, value); }
        }
        
        private ListCollectionView _patients;

        public ListCollectionView Patients
        {
            get { return _patients; }
            set { SetProperty(ref _patients, value); }
        }

        private Patient _currentPatient;

        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set { SetProperty(ref _currentPatient, value); }
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
        public RegistratureMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            #region Properties

            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _searchPatientCommand = new DelegateCommand<object>(SearchPatient);
            _copyPatientCommand = new DelegateCommand<object>(CopyPatient);
            _receptionChooseCommand = new DelegateCommand<object>(ReceptionChoose);
            _newPatientCommand = new DelegateCommand<object>(NewPatient);
            _removePatientCommand = new DelegateCommand<object>(RemovePatient);
            _savePatientCommand = new DelegateCommand<object>(SavePatient);
            _saveCityCommand = new DelegateCommand<object>(SaveCity);
            _addPackageToReceptionCommand = new DelegateCommand<object>(AddPackageToReception);
            _removePackageFromReceptionCommand = new DelegateCommand<object>(RemovePackageFromReception);
            _confirmReceptionCommand = new DelegateCommand<object>(ConfirmReception);
            _receptionPaymentCommand = new DelegateCommand<object>(ReceptionPayment);
            _printReceptionCommand = new DelegateCommand<object>(PrintReception);
            _confirmPaymentCommand = new DelegateCommand<object>(ConfirmPayment);
            _cancelPaymentCommand = new DelegateCommand<object>(CancelPayment);
            _confirmPatientCommand = new DelegateCommand<object>(ConfirmPatient);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();

            Patients=new ListCollectionView(new List<Patient>());

            #endregion
            _startHour = int.Parse(Utils.ReadSetting("StartHour"));
            _endHour = int.Parse(Utils.ReadSetting("EndHour"));
            _cabinets = Utils.ReadSetting("Cabinets").Split(',');
            _currentDate=DateTime.Today;
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new CitiesSelect())
            .Success(ri =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                Cities = ri.Cities;
                SchedulesReload();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void SchedulesReload()
        {

            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.PostAsync(new SchedulesFullSelect { TimeStart = _currentDate })
            .Success(rs =>
            {
                //_eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                Schedules = rs.Schedules;
                //_eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.GetAsync(new ReceptionsByDateSelect { StartDate = _currentDate })
                .Success(rr =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    Receptions = rr.Receptions;
                    MakeCurrentDayReceptions();
                })
                .Error(ex =>
                {
                    Schedules = new List<Schedule>();
                    throw ex;
                });
            })
            .Error(ex =>
            {
                Schedules = new List<Schedule>();
                throw ex;
            });
        }

        #region MakeCurrentWeek

        private void ScheduleChoose(Schedule obj)
        {
            //CurrentSchedule = obj;
            //_oldDoctorId = obj.CurrentDoctor.Id;
        }
        private void MakeCurrentDayReceptions()
        {
            DayReceptions = new ObservableCollection<CabinetReceptions>();
            CabinetReceptions cabinetReceptions;
            ScheduleReception scheduleReception;
            ObservableCollection<Schedule> schedules;
            DateTime currentStartTime, startTime, endTime, startDate;
            startTime = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, _startHour, 0, 0);
            //Make a minimal start time for the current date
            endTime = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, _endHour, 59, 0);
           
            foreach (var cabinet in _cabinets)
            {
                int cab = int.Parse(cabinet);
                currentStartTime = startTime; // renew currentStartTime before start new cycle
                cabinetReceptions = new CabinetReceptions(cab);
                schedules = FilterSchedules(currentStartTime, cab);
                foreach (var schedule in schedules)
                {
                    if (currentStartTime < schedule.Start) // Have to fill empty place with white bar
                    {
                        scheduleReception=new ScheduleReception(MakeWhiteBar(currentStartTime, schedule.Start, cab));
                        currentStartTime = scheduleReception.Schedule.Start; // new start time for real Schedule following
                        cabinetReceptions.ScheduleReceptions.Add(scheduleReception);
                        scheduleReception = new ScheduleReception(schedule, GetReceptionsInSchedule(schedule));
                        cabinetReceptions.ScheduleReceptions.Add(scheduleReception);
                        currentStartTime = schedule.End; // new start time for next Schedule
                    }
                    else // Have to fill real schedule info
                    {
                        scheduleReception = new ScheduleReception(schedule, GetReceptionsInSchedule(schedule));
                        cabinetReceptions.ScheduleReceptions.Add(scheduleReception);
                        currentStartTime = schedule.End; // new start time for next Schedule
                    }
                }
                if (currentStartTime < endTime) // Last schedule is empty if a gap exists
                {
                    scheduleReception = new ScheduleReception(MakeWhiteBar(currentStartTime, endTime, cab));
                    cabinetReceptions.ScheduleReceptions.Add(scheduleReception);
                }
                DayReceptions.Add(cabinetReceptions); //Add new cabinet here
            }
        }

        private ObservableCollection<Reception> GetReceptionsInSchedule(Schedule schedule)
        {
            ObservableCollection<Reception> receptions=new ObservableCollection<Reception>();
            var list = from r in Receptions
                       where
                           r.ScheduleId == schedule.Id
                       select r;
            foreach (var r in list)
            {
                receptions.Add(r);
            }
            return receptions;
        }

        private ObservableCollection<Schedule> FilterSchedules(DateTime time, int cabinet)
        {
            var schedules = new ObservableCollection<Schedule>();
            DateTime ts = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            DateTime te = new DateTime(time.AddDays(1).Year, time.AddDays(1).Month, time.AddDays(1).Day, 0, 0, 0);
            var list = from s in Schedules
                       where
                           s.CabinetId == cabinet
                           && s.Start > ts
                           && s.End < te
                       orderby s.Start
                       select s;
            foreach (var s in list)
            {
                schedules.Add(s);
            }
            return schedules;
        }
        private ObservableCollection<Schedule> FilterSchedules(int doctorId)
        {
            var schedules = new ObservableCollection<Schedule>();
            var list = from s in Schedules
                       where
                           s.CurrentDoctor.Id == doctorId
                       select s;
            foreach (var s in list)
            {
                schedules.Add(s);
            }
            return schedules;
        }
        private Schedule GetSchedule(int id)
        {
            var list = Schedules.FirstOrDefault(s => s.Id == id);
            return list;
        }
        private Schedule MakeWhiteBar(DateTime startTime, DateTime endTime, int cabinet)
        {
            return new Schedule(startTime, endTime, cabinet);
        }

        #endregion

        private void SaveCity(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new CitySave { City = CurrentPatient.City })
            .Success(rig =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void MakePanelVisible(string panel)
        {
            IsNewPatientPanelVisible = false;
            IsSearchPatientPanelVisible = false;
            //IsAddPackagesPanelVisible = false;
            IsReceptionPanelVisible = false;
            IsPaymentsPanelVisible = false;
            switch (panel)
            {
                case "Search":
                    IsSearchPatientPanelVisible = true;
                    break;
                case "Patient":
                    IsNewPatientPanelVisible = true;
                    break;
                case "Reception":
                    IsReceptionPanelVisible = true;
                    break;
                case "Payment":
                    IsPaymentsPanelVisible = true;
                    break;
            }
        }
        #region Patient

        private void ConfirmPatient(object obj)
        {
            MakePanelVisible("");
        }

        private void SearchPatient(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new PatientsSelect{ Text = PatientSearchText })
            .Success(rig =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                foreach (var patient in rig.Patients)
                {
                    patient.City = Cities.Single(i => i.Id == patient.CityId);
                }

                Patients = new ListCollectionView(rig.Patients);
                Patients.MoveCurrentTo(null);
                Patients.CurrentChanged += Patients_CurrentChanged;
                CurrentPatient = new Patient();
                MakePanelVisible("Search");
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

        private void Patients_CurrentChanged(object sender, EventArgs e)
        {
            CurrentPatient = Patients.CurrentItem != null ? (Patient)Patients.CurrentItem : new Patient();
        }
        private void NewPatient(object obj)
        {
            CurrentPatient = new Patient();
            MakePanelVisible("Patient");
        }

        private void SavePatient(object obj)
        {
            bool isNew = CurrentPatient.Id <= 0;
            Errors = CurrentPatient.Validate();
            if (Errors.Count == 0)
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.PostAsync(new PatientSave { Patient = CurrentPatient })
                .Success(r =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    CurrentPatient.Id = r.PatientId;
                    if (isNew)
                    {
                        Patients.AddNewItem(CurrentPatient);
                    }
                    r.Message.Message = string.Format(r.Message.Message, CurrentPatient.Surname, CurrentPatient.FirstName, CurrentPatient.SecondName);
                    _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                })
                .Error(ex =>
                {
                    throw ex;
                });
            }
        }

        private void RemovePatient(object obj)
        {
            bool isNew = CurrentPatient.Id == 0;
            ConfirmationRequest.Raise(
                new Confirmation { Content = "Пациент будет удалён! Вы уверены?", Title = "Удаление пациента." },
                c =>
                {
                    if (c.Confirmed)
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                        if (isNew)
                        {
                            CurrentPatient = new Patient();
                            _newPatientCommand.RaiseCanExecuteChanged();
                            MakePanelVisible("");
                        }
                        else
                        {
                            _jsonClient.GetAsync(new PatientDelete { PatientId = CurrentPatient.Id })
                            .Success(r =>
                            {
                                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                                r.Message.Message = string.Format(r.Message.Message, CurrentPatient.Surname, CurrentPatient.FirstName, CurrentPatient.SecondName);
                                Patients.Remove(_currentPatient);
                                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                                MakePanelVisible("");
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
        
        #region Reception

        private void ConfirmReception(object obj)
        {
            throw new NotImplementedException();
        }

        private void ReceptionChoose(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Package in Reception
        private void RemovePackageFromReception(object obj)
        {
            throw new NotImplementedException();
        }

        private void AddPackageToReception(object obj)
        {
            throw new NotImplementedException();
        }
        private void ClearPatients()
        {
            Patients.MoveCurrentTo(null);
            //PatientsInDoctor.MoveCurrentTo(null);
            //CurrentPatient = new Patient();
            //CurrentPatientInDoctor = new Patient();
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

        #region Payment

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

        #endregion

    }
}
