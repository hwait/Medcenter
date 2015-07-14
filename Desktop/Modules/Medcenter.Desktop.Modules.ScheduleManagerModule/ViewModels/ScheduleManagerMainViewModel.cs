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
using Medcenter.Desktop.Modules.ScheduleManagerModule.Model;
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
using Telerik.Windows.Controls.Animation;

namespace Medcenter.Desktop.Modules.ScheduleManagerModule.ViewModels
{
    [Export]
    public class ScheduleManagerMainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        
        #region Properties

        private int _startHour,_endHour, _oldDoctorId, _gap;
        private string[] _cabinets;
        private readonly DelegateCommand<Schedule> _scheduleChooseCommand;
        private readonly DelegateCommand<object> _copyScheduleToNextWeekCommand;
        private readonly DelegateCommand<object> _makeStartGapedJointCommand;
        private readonly DelegateCommand<object> _makeEndGapedJointCommand;
        private readonly DelegateCommand<object> _removeScheduleCommand;
        private readonly DelegateCommand<Schedule> _saveScheduleCommand;

        #region Commands Implementation
        public ICommand ScheduleChooseCommand
        {
            get { return this._scheduleChooseCommand; }
        }
        public ICommand CopyScheduleToNextWeekCommand
        {
            get { return this._copyScheduleToNextWeekCommand; }
        }
        public ICommand MakeStartGapedJointCommand
        {
            get { return this._makeStartGapedJointCommand; }
        }
        public ICommand MakeEndGapedJointCommand
        {
            get { return this._makeEndGapedJointCommand; }
        }

        public ICommand RemoveScheduleCommand
        {
            get { return this._removeScheduleCommand; }
        }

        public ICommand SaveScheduleCommand
        {
            get { return this._saveScheduleCommand; }
        }

        #endregion

        private DateTime _currentDate, _startDate, _endDate;
        private ObservableCollection<Doctor> _doctors;
        private ObservableCollection<Schedule> _schedules;
        private Schedule _currentSchedule;
        private ObservableCollection<ScheduleDay> _currentWeek;
        private List<ResultMessage> _errors;
        private string[] _cabinetHours;
        #region Binding Fields Implementation

        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set { SetProperty(ref _currentDate, value); }
        }

        public ObservableCollection<Doctor> Doctors
        {
            get { return _doctors; }
            set { SetProperty(ref _doctors, value); }
        }

        public ObservableCollection<Schedule> Schedules
        {
            get { return _schedules; }
            set { SetProperty(ref _schedules, value); }
        }

        public Schedule CurrentSchedule
        {
            get { return _currentSchedule; }
            set
            {
                SetProperty(ref _currentSchedule, value);
            }
        }
        public string[] CabinetHours
        {
            get { return _cabinetHours; }
            set { SetProperty(ref _cabinetHours, value); }
        }
        public ObservableCollection<ScheduleDay> CurrentWeek
        {
            get { return _currentWeek; }
            set { SetProperty(ref _currentWeek, value); }
        }
        public List<ResultMessage> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }
        #endregion

        #endregion

        [ImportingConstructor]
        public ScheduleManagerMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _scheduleChooseCommand = new DelegateCommand<Schedule>(ScheduleChoose);
            _copyScheduleToNextWeekCommand = new DelegateCommand<object>(CopyScheduleToNextWeek);
            _makeStartGapedJointCommand = new DelegateCommand<object>(MakeStartGapedJoint, CanMakeStartGapedJoint);
            _makeEndGapedJointCommand = new DelegateCommand<object>(MakeEndGapedJoint, CanMakeEndGapedJoint);
            _removeScheduleCommand = new DelegateCommand<object>(RemoveSchedule);
            _saveScheduleCommand = new DelegateCommand<Schedule>(SaveSchedule);

            _startHour = int.Parse(Utils.ReadSetting("StartHour"));
            _endHour = int.Parse(Utils.ReadSetting("EndHour"));
            _cabinets = Utils.ReadSetting("Cabinets").Split(',');
            _gap = int.Parse(Utils.ReadSetting("RestGap"));
            _currentDate=DateTime.Now;
            MakeCabinetHours();
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();

            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);

            _jsonClient.GetAsync(new DoctorsSelect())
            .Success(rig =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                Doctors = new ObservableCollection<Doctor>(rig.Doctors);
            })
            .Error(ex =>
            {
                throw ex;
            });

            //Get Schedules here

            Schedules=new ObservableCollection<Schedule>();
            _startDate = Utils.GetFirstDayOfWeek(_currentDate);
            _endDate = Utils.GetLastDayOfWeek(_currentDate);

            MakeCurrentWeek();
        }

        private void MakeCabinetHours()
        {
            CabinetHours = new string[_endHour - _startHour+1];
            for (int i = _startHour; i <= _endHour; i++)
            {
                CabinetHours[i - _startHour] = i.ToString("00");
            }
            
        }

        private void ScheduleChoose(Schedule obj)
        {
            CurrentSchedule = obj;
            _oldDoctorId = obj.CurrentDoctor.Id;
        }

        #region MakeCurrentWeek

        private void MakeCurrentWeek()
        {
            CurrentWeek = new ObservableCollection<ScheduleDay>();
            ScheduleDay scheduleDay;
            ScheduleCabinet scheduleCabinet;
            DateTime currentStartTime, startTime, endTime, startDate;
            ObservableCollection<Schedule> schedules;
            startDate = _startDate;
            while (startDate <= _endDate)
            {
                startTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, _startHour, 0, 0);
                    //Make a minimal start time for the current date
                endTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, _endHour, 59, 0);
                    //Make a max start time for the current date
                scheduleDay = new ScheduleDay(startDate);
                foreach (var cabinet in _cabinets)
                {
                    int cab = int.Parse(cabinet);
                    currentStartTime = startTime; // renew currentStartTime before start new cycle
                    scheduleCabinet = new ScheduleCabinet(cab);
                    schedules = FilterSchedules(currentStartTime, cab);
                    foreach (var schedule in schedules)
                    {
                        if (currentStartTime < schedule.Start) // Have to fill empty place with white bar
                        {
                            scheduleCabinet.Schedules.Add(MakeWhiteBar(currentStartTime, schedule.Start, cab));
                            currentStartTime = schedule.Start; // new start time for real Schedule following
                            scheduleCabinet.Schedules.Add(schedule);
                            currentStartTime = schedule.End; // new start time for next Schedule
                        }
                        else // Have to fill real schedule info
                        {
                            scheduleCabinet.Schedules.Add(schedule);
                            currentStartTime = schedule.End; // new start time for next Schedule
                        }
                    }
                    if (currentStartTime < endTime) // Last schedule is empty if a gap exists
                        scheduleCabinet.Schedules.Add(MakeWhiteBar(currentStartTime, endTime, cab));
                    scheduleDay.ScheduleCabinets.Add(scheduleCabinet); //Add new cabinet here
                }
                //scheduleDay.ScheduleCabinets.Add(TimeRuler(startDate));
                CurrentWeek.Add(scheduleDay); // Add new scheduleDay here
                startDate = startDate.AddDays(1);
            }
        }

        private ScheduleCabinet TimeRuler(DateTime startDate)
        {
            ScheduleCabinet cabinet=new ScheduleCabinet();
            for (int i = _startHour; i <= _endHour; i++)
            {
                cabinet.Schedules.Add(
                    new Schedule
                    {
                        Id = 0,
                        CurrentDoctor = new Doctor {Id = 0, Name = "", ShortName = "", Color = Colors.Black.ToString()},
                        Start = new DateTime(startDate.Year, startDate.Month, startDate.Day, i, 0, 0),
                        End = new DateTime(startDate.Year, startDate.Month, startDate.Day, i, 1, 0)
                    });
                cabinet.Schedules.Add(
                    new Schedule
                    {
                        Id = 0,
                        CurrentDoctor = new Doctor (i.ToString()),
                        Start = new DateTime(startDate.Year, startDate.Month, startDate.Day, i, 1, 0),
                        End = new DateTime(startDate.Year, startDate.Month, startDate.Day, i, 59, 0)
                    });
            }
            return cabinet;
        }

        private ObservableCollection<Schedule> FilterSchedules(DateTime time, int cabinet)
        {
            var schedules = new ObservableCollection<Schedule>();
            DateTime ts = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            DateTime te = new DateTime(time.Year, time.Month, time.Day + 1, 0, 0, 0);
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
            return new Schedule(startTime, endTime,cabinet);
        }

        #endregion

        private void CopyScheduleToNextWeek(object obj)
        {
            //
        }

        #region Schedule

        private void SaveSchedule(Schedule schedule)
        {
            //CurrentSchedule = schedule;
            bool isNew = schedule.Id <= 0;
            Errors = schedule.Validate();
            if (Errors.Count == 0)
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                //_jsonClient.PostAsync(new scheduleave { Schedule = CurrentSchedule })
                //    .Success(r =>
                //    {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
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

                if (isNew)
                {
                    schedule.Id = 1; // Get Id from service
                    
                    CopyScheduleToWeekdays(schedule);
                    schedule.ResetFlags();
                    Schedules.Add(schedule);
                }
                else
                {
                    if (CurrentSchedule.ReplaceEverywhere)
                    {
                        ObservableCollection<Schedule> schedules = FilterSchedules(_oldDoctorId);
                        foreach (var s in schedules)
                        {
                            s.CurrentDoctor = schedule.CurrentDoctor;
                        }
                        _oldDoctorId = schedule.CurrentDoctor.Id;
                        
                    }
                }
                MakeCurrentWeek();
                CurrentSchedule = new Schedule();
            }
        }

        private void CopyScheduleToWeekdays(Schedule schedule)
        {
            if (schedule.Monday) CopyScheduleToWeekday(schedule, _startDate);
            if (schedule.Tuesday) CopyScheduleToWeekday(schedule, _startDate.AddDays(1));
            if (schedule.Wednesday) CopyScheduleToWeekday(schedule, _startDate.AddDays(2));
            if (schedule.Thursday) CopyScheduleToWeekday(schedule, _startDate.AddDays(3));
            if (schedule.Friday) CopyScheduleToWeekday(schedule, _startDate.AddDays(4));
            if (schedule.Saturday) CopyScheduleToWeekday(schedule, _startDate.AddDays(5));
            if (schedule.Sunday) CopyScheduleToWeekday(schedule, _startDate.AddDays(6));
        }

        private void CopyScheduleToWeekday(Schedule schedule, DateTime date)
        {
            DateTime ds = new DateTime(date.Year, date.Month, date.Day, schedule.Start.Hour, schedule.Start.Minute,0);
            DateTime de = new DateTime(date.Year, date.Month, date.Day, schedule.End.Hour, schedule.End.Minute, 0);
            Schedule newSchedule = new Schedule(ds, de, schedule.CabinetId);
            if (Schedules.Any(s =>
                ((s.Start >= newSchedule.Start && s.Start < newSchedule.End) ||
                (s.End > newSchedule.Start && s.End <= newSchedule.End) ||
                (s.Start <= newSchedule.Start && s.End >= newSchedule.End)) && s.CabinetId == newSchedule.CabinetId)
                || (schedule.Start == newSchedule.Start && schedule.End == newSchedule.End)) 
                return;
            newSchedule.CurrentDoctor = schedule.CurrentDoctor;
            SaveSchedule(newSchedule);
        }

        private void RemoveSchedule(object obj)
        {
            bool isNew = CurrentSchedule.Id == 0;
            if (isNew)
            {
                MakeCurrentWeek();
                CurrentSchedule = new Schedule();
            }
            else
                ConfirmationRequest.Raise(
                    new Confirmation
                    {
                        Content = "Расписание будет удалёно! Вы уверены?",
                        Title = "Удаление расписания."
                    },
                    c =>
                    {
                        if (c.Confirmed)
                        {
                            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                            if (isNew)
                            {
                                CurrentSchedule = new Schedule();
                            }
                            else
                            {
                                //_jsonClient.GetAsync(new ScheduleDelete { ScheduleId = CurrentSchedule.Id })
                                //.Success(r =>
                                //{
                                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                                //    r.Message.Message = string.Format(r.Message.Message, CurrentSchedule.Name);
                                //    RemoveScheduleFromScheduleByIGID(CurrentSchedule.Id);
                                //    Schedule.Remove(Schedule.CurrentItem);
                                //    _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                                //    _newScheduleCommand.RaiseCanExecuteChanged();
                                //})
                                //.Error(ex =>
                                //{
                                //    throw ex;
                                //});
                                Schedules.Remove(CurrentSchedule);
                                MakeCurrentWeek();
                                CurrentSchedule = new Schedule();
                            }
                        }
                    });
        }
        #endregion

        #region Joints
        
        private void MakeStartGapedJoint(object obj)
        {
            var t = CurrentSchedule.Start.AddMinutes(_gap);
            CurrentSchedule.StartHour = t.Hour.ToString("00");
            CurrentSchedule.StartMinute = t.Minute.ToString("00");
            SetProperty(ref _currentSchedule, CurrentSchedule);
        }
        private bool CanMakeStartGapedJoint(object arg)
        {
            return true;
        }
        private void MakeEndGapedJoint(object obj)
        {
            var t = CurrentSchedule.Start.AddMinutes(_gap*-1);
            CurrentSchedule.StartHour = t.Hour.ToString("00");
            CurrentSchedule.StartMinute = t.Minute.ToString("00");
            SetProperty(ref _currentSchedule, CurrentSchedule);
        }
        private bool CanMakeEndGapedJoint(object arg)
        {
            return true;
        }
        #endregion
    }
}
