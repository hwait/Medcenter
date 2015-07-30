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
using System.Windows.Controls;
using Telerik.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Desktop.Modules.RegistratureModule.Model;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Misc;
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
    public class RegistratureMainViewModel : BindableBase, INavigationAware
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
        private readonly DelegateCommand<Reception> _receptionChooseCommand;
        private readonly DelegateCommand<object> _addPackageToReceptionCommand;
        private readonly DelegateCommand<object> _removePackageFromReceptionCommand;
        private readonly DelegateCommand<object> _confirmReceptionCommand;
        private readonly DelegateCommand<object> _removeReceptionCommand;
        private readonly DelegateCommand<object> _payReceptionCommand;
        private readonly DelegateCommand<object> _saveReceptionCommand;
        private readonly DelegateCommand<object> _receptionPaymentCommand;
        private readonly DelegateCommand<Visual> _printReceptionCommand;
        private readonly DelegateCommand<object> _confirmPaymentCommand;
        private readonly DelegateCommand<object> _cancelPaymentCommand;
        private readonly DelegateCommand<object> _saveCityCommand;
        private readonly DelegateCommand<object> _confirmPatientCommand;
        private readonly DelegateCommand<PackageGroup> _packagesGroupChooseCommand;
        private readonly DelegateCommand<WeekDay> _chooseWeekDayCommand;
        

        #region Commands Declaration
        public ICommand ChooseWeekDayCommand
        {
            get { return this._chooseWeekDayCommand; }
        }
        public ICommand RemoveReceptionCommand
        {
            get { return this._removeReceptionCommand; }
        }
        public ICommand PayReceptionCommand
        {
            get { return this._payReceptionCommand; }
        }
        public ICommand SaveReceptionCommand
        {
            get { return this._saveReceptionCommand; }
        }
        public ICommand PackagesGroupChooseCommand
        {
            get { return this._packagesGroupChooseCommand; }
        }
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
        

        #region Packages and Reception

        private Package _currentPackage;
        public Package CurrentPackage
        {
            get { return _currentPackage; }
            set { SetProperty(ref _currentPackage, value); }
        }
        private Package _currentPackageInReception;
        public Package CurrentPackageInReception
        {
            get { return _currentPackageInReception; }
            set { SetProperty(ref _currentPackageInReception, value); }
        }
        private List<Package> _packagesBase;

        public List<Package> PackagesBase
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
        private List<List<PackageGroup>> _packageGroupsRows;

        public List<List<PackageGroup>> РackageGroupsRows
        {
            get { return _packageGroupsRows; }
            set { SetProperty(ref _packageGroupsRows, value); }
        }
        #endregion

        #region Patient

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

        #region Reception

        private Reception _currentReception;

        public Reception CurrentReception
        {
            get { return _currentReception; }
            set { SetProperty(ref _currentReception, value); }
        }
        private List<Discount> _discounts;

        public List<Discount> Discounts
        {
            get { return _discounts; }
            set { SetProperty(ref _discounts, value); }
        }
        private Discount _currentDiscount;

        public Discount CurrentDiscount
        {
            get
            {
                //var id = (CurrentReception == null) ? 0 : CurrentReception.Discount.Id;
                //return (Discounts==null)?new Discount() : Discounts.Single(p => p.Id == id);
                return _currentDiscount;
            }
            set
            {
                CurrentReception.Discount = value;
                SetProperty(ref _currentDiscount, value);
            }
        }
        private List<Schedule> _schedules;

        public List<Schedule> Schedules
        {
            get { return _schedules; }
            set { _schedules = value; }
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
        #endregion

        #region Others

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
                CurrentWeek = new Week(_currentDate);
                SchedulesReload();
            }
        }

        private Week _currentWeek;
        public Week CurrentWeek
        {
            get
            {
                return _currentWeek;
            }
            set
            {
                SetProperty(ref _currentWeek, value);
            }
        }

        private bool _isNewPatientPanelVisible;
        private bool _isSearchPatientPanelVisible;
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
            _receptionChooseCommand = new DelegateCommand<Reception>(ReceptionChoose);
            _newPatientCommand = new DelegateCommand<object>(NewPatient);
            _removePatientCommand = new DelegateCommand<object>(RemovePatient);
            _savePatientCommand = new DelegateCommand<object>(SavePatient);
            _saveCityCommand = new DelegateCommand<object>(SaveCity);
            _addPackageToReceptionCommand = new DelegateCommand<object>(AddPackageToReception);
            _removePackageFromReceptionCommand = new DelegateCommand<object>(RemovePackageFromReception);
            _confirmReceptionCommand = new DelegateCommand<object>(ConfirmReception);
            _receptionPaymentCommand = new DelegateCommand<object>(ReceptionPayment);
            _printReceptionCommand = new DelegateCommand<Visual>(PrintReception);
            _chooseWeekDayCommand = new DelegateCommand<WeekDay>(ChooseWeekDay);
            
            _removeReceptionCommand = new DelegateCommand<object>(RemoveReception);
            _payReceptionCommand = new DelegateCommand<object>(PayReception);
            _saveReceptionCommand = new DelegateCommand<object>(SaveReception);

            _confirmPaymentCommand = new DelegateCommand<object>(ConfirmPayment);
            _cancelPaymentCommand = new DelegateCommand<object>(CancelPayment);
            _confirmPatientCommand = new DelegateCommand<object>(ConfirmPatient);
            _packagesGroupChooseCommand = new DelegateCommand<PackageGroup>(PackagesGroupChoose);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();

            Patients=new ListCollectionView(new List<Patient>());

            #endregion
            
            _startHour = int.Parse(Utils.ReadSetting("StartHour"));
            _endHour = int.Parse(Utils.ReadSetting("EndHour"));
            _cabinets = Utils.ReadSetting("Cabinets").Split(',');
            CurrentDate=DateTime.Today;
            //CurrentWeek = new Week(_currentDate);
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new CitiesSelect())
            .Success(ri =>
            {
                Cities = ri.Cities;
                //SchedulesReload();
                _jsonClient.GetAsync(new PackageGroupsSelect())
                .Success(pg =>
                {
                    РackageGroupsRows = GetРackageGroupsRows(pg.PackageGroups, int.Parse(Utils.ReadSetting("PackageGroupRowNumber")));

                    _jsonClient.GetAsync(new PackagesSelect())
                    .Success(p =>
                    {
                        PackagesBase=p.Packages;
                        MakeDiscounts();
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

        private void ChooseWeekDay(WeekDay day)
        {
            _currentDate = day.Date;
            CurrentWeek.Activate(day.Date);
            SchedulesReload();
        }

        private void MakeDiscounts()
        {
            Discounts = new List<Discount>();
            Discounts.Add(new Discount());
            _jsonClient.GetAsync(new DiscountsManualSelect())
            .Success(d =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                Discounts.AddRange(d.Discounts);
            })
            .Error(ex =>
            {
                throw ex;
            });
            SchedulesReload();
        }

        #region Navigation

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            MakeDiscounts();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion


        #region Visualization
        private void PackagesGroupChoose(PackageGroup pg)
        {
            PackagesRefresh(pg);
        }
        private void PackagesRefresh(PackageGroup packageGroup)
        {
            CurrentPackage=new Package();
            List<Package> list =
                packageGroup.PackageIds.Select(packageId => PackagesBase.Single(i => i.Id == packageId)).ToList();
            Packages = new ListCollectionView(list);

        }

        private List<List<PackageGroup>> GetРackageGroupsRows(List<PackageGroup> packageGroups, int n)
        {
            List<List<PackageGroup>> packageGroupsRows = new List<List<PackageGroup>>();
            for (int i = 0; i < n; i++)
            {
                var list = from r in packageGroups
                    where
                        r.Row == i
                    select r;
                packageGroupsRows.Add(list.ToList());
            }
            return packageGroupsRows;
        }

        private void SchedulesReload()
        {
            
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.PostAsync(new SchedulesFullSelect {TimeStart = _currentDate})
                .Success(rs =>
                {
                    //_eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    Schedules = rs.Schedules;
                    //_eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                    _jsonClient.GetAsync(new ReceptionsByDateSelect {StartDate = _currentDate})
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
                default:
                    if (CurrentReception != null && CurrentReception.Duration > 0) IsReceptionPanelVisible = true;
                    break;
            }
        }
        #endregion
        
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
            Receptions = Receptions.OrderBy(o => o.Start).ToList();
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
                        //currentStartTime = scheduleReception.Schedule.Start; // new start time for real Schedule following
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
            DateTime currentStartTime, startTime, endTime;
            Reception reception;
            ObservableCollection<Reception> receptions=new ObservableCollection<Reception>();
            var list = from r in Receptions
                        where
                            r.ScheduleId == schedule.Id
                        select r;
            startTime = schedule.Start;
            foreach (var r in list)
            {
                if (startTime < r.Start) // Have to fill empty place with white bar
                {
                    reception = new Reception(schedule.Id, startTime, (int) r.Start.Subtract(startTime).TotalMinutes); // white bar
                    receptions.Add(reception);
                    receptions.Add(r);
                    startTime = r.Start.AddMinutes(r.Duration); // new start time for next Reception
                }
                else // Have to fill real schedule info
                {
                    receptions.Add(r);
                    startTime = r.Start.AddMinutes(r.Duration); // new start time for next Reception
                }
            }
            if (startTime < schedule.End) // Last Reception is empty if a gap exists
            {
                reception = new Reception(schedule.Id, startTime, (int)schedule.End.Subtract(startTime).TotalMinutes); // white bar
                receptions.Add(reception);
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

        #region Patient
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
        private void ConfirmPatient(object obj)
        {
            MakePanelVisible("");
        }

        private void SearchPatient(object obj)
        {
            if (PatientSearchText.IsNullOrEmpty()||PatientSearchText.Length < 3) return;
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

        private void SaveReception(object obj)
        {
            bool isNew = CurrentReception.Id <= 0;
            Errors = CurrentReception.Validate();
            if (CurrentReception.Patient == null || CurrentReception.Patient.Id == 0)
                Errors.Add(new ResultMessage(2, "Пациент:", OperationErrors.VariantNotChosen));
            //Reception nextReception = GetNextReception(CurrentReception);
            CurrentReception.MaxDuration = GetMaxDuration(CurrentReception);
            CurrentReception.CalcDuration();
            var end = GetTimeEnd(CurrentReception.Start, CurrentReception.Duration);
            if (Receptions.Any(s =>
                (s.Start >= CurrentReception.Start && s.Start < end) && s.ScheduleId == CurrentReception.ScheduleId && s.Id != CurrentReception.Id))
                Errors.Add(new ResultMessage(2, "Время:", OperationErrors.ReceptionTimeWromg));
            if (Receptions.Any(s =>
                (GetTimeEnd(s.Start, s.Duration) > CurrentReception.Start && GetTimeEnd(s.Start, s.Duration) < end) && s.ScheduleId == CurrentReception.ScheduleId && s.Id != CurrentReception.Id))
                Errors.Add(new ResultMessage(2, "Время:", OperationErrors.ReceptionTimeWromg));
            if (Receptions.Any(s => (s.Start <= CurrentReception.Start && GetTimeEnd(s.Start, s.Duration) >= end) && s.ScheduleId == CurrentReception.ScheduleId && s.Id != CurrentReception.Id))
                Errors.Add(new ResultMessage(2, "Время:", OperationErrors.ReceptionTimeWromg));
            
            if (Errors.Count == 0)
            {
                if (CurrentReception.Status == (byte)ReceptopnStatuses.Empty) CurrentReception.Status = (byte)ReceptopnStatuses.Enlisted;
                CurrentReception.PatientId = CurrentReception.Patient.Id;
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.PostAsync(new ReceptionSave { Reception = CurrentReception })
                .Success(r =>
                {
                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    CurrentReception.Id = r.ReceptionId;
                    if (isNew)
                    {
                        Receptions.Add(CurrentReception);
                        
                    }
                    r.Message.Message = string.Format(r.Message.Message, CurrentReception.Start.ToString("hh:mm"));
                    _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                    ClearReceptionForms();
                    MakeCurrentDayReceptions();
                })
                .Error(ex =>
                {
                    throw ex;
                });
            }
        }

        private DateTime GetTimeEnd(DateTime start, int duration)
        {
            return start.AddMinutes(duration);
        }

        private void ClearReceptionForms()
        {
            CurrentReception = new Reception();
            Patients = new ListCollectionView(new List<Patient>());
            CurrentPatient=new Patient();
            PatientSearchText = "";
            MakePanelVisible("");
        }

        private void ConfirmReception(object obj)
        {
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new ReceptionsStatusSet { ReceptionId = CurrentReception.Id, Status = (int) ReceptopnStatuses.Confirmed })
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                CurrentReception.Status = (byte)ReceptopnStatuses.Confirmed;
                MakeCurrentDayReceptions();
                CurrentReception = new Reception();
                ClearReceptionForms();
            })
            .Error(ex =>
            {
                throw ex;
            });
        }

       

        private void RemoveReception(object obj)
        {
            bool isNew = CurrentReception.Id == 0;
            if (!isNew)
            {
                ConfirmationRequest.Raise(
                    new Confirmation
                    {
                        Content = "Приём будет удалён! Вы уверены?",
                        Title = "Удаление приёма."
                    },
                    c =>
                    {
                        if (c.Confirmed)
                        {
                            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                            _jsonClient.GetAsync(new ReceptionDelete { ReceptionId = CurrentReception.Id })
                                .Success(r =>
                                {
                                    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                                    Receptions.Remove(CurrentReception);
                                    r.Message.Message = string.Format(r.Message.Message, CurrentReception.Start.ToString("hh:mm"));
                                    MakeCurrentDayReceptions();
                                    CurrentReception = new Reception();
                                    ClearReceptionForms();
                                })
                                .Error(ex =>
                                {
                                    throw ex;
                                });
                        }
                    });
            }
            else
            {
                MakeCurrentDayReceptions();
                CurrentReception = new Reception();
            }
        }
        private void ReceptionChoose(Reception obj)
        {
            CurrentReception = obj;
            CurrentReception.MaxDuration = GetMaxDuration(CurrentReception);
            MakePanelVisible("Reception");
            CurrentDiscount=Discounts.Single(i => i.Id == CurrentReception.DiscountId);
            //CurrentReception.Discount = Discounts.Single(i => i.Id == CurrentReception.DiscountId);
        }

        private int GetMaxDuration(Reception currentReception)
        {
            var next=Receptions.FirstOrDefault(s => s.Start > currentReception.Start && s.ScheduleId == currentReception.ScheduleId);
            int max = next == null ? (int) Schedules.FirstOrDefault(s => s.Id == CurrentReception.ScheduleId).End.Subtract(CurrentReception.Start).TotalMinutes : (int) next.Start.Subtract(CurrentReception.Start).TotalMinutes;
            return max;
            //return Receptions.SkipWhile(i => i.Id != CurrentReception.Id).Skip(1).FirstOrDefault();
        }

        #endregion

        #region Package in Reception
        private void AddPackageToReception(object obj)
        {
            if (CurrentPackage == null) return;
            if (!CurrentReception.Packages.Contains(CurrentPackage)) CurrentReception.Packages.Add(CurrentPackage);
            CurrentReception.Calc();
            CurrentReception.CalcDuration();
        }

        private void RemovePackageFromReception(object obj)
        {
            CurrentReception.Packages.Remove(CurrentPackageInReception);
            CurrentReception.Calc();
            CurrentReception.CalcDuration();
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

        #endregion

        #region Payment
        private void PayReception(object obj)
        {
            MakePanelVisible("Payment");
            CurrentReception.ActuateProperties();
        }
        private void CancelPayment(object obj)
        {
            MakePanelVisible("Reception"); 
        }

        private void ConfirmPayment(object obj)
        {
            throw new NotImplementedException();
        }

        private void PrintReception(Visual obj)
        {
            PrintDialog printDlg = new PrintDialog();
            printDlg.PrintVisual(obj, "");

        }

        private void ReceptionPayment(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
