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

        private int _startHour;
        private int _endHour;
        private string[] _cabinets;
        private readonly DelegateCommand<object> _copyScheduleToNextWeekCommand;
        private readonly DelegateCommand<object> _makeStartJointCommand;
        private readonly DelegateCommand<object> _makeStartGapedJointCommand;
        private readonly DelegateCommand<object> _makeEndJointCommand;
        private readonly DelegateCommand<object> _makeEndGapedJointCommand;
        private readonly DelegateCommand<object> _removeScheduleCommand;
        private readonly DelegateCommand<object> _saveScheduleCommand;

        #region Commands Implementation

        public ICommand CopyScheduleToNextWeekCommand
        {
            get { return this._copyScheduleToNextWeekCommand; }
        }

        public ICommand MakeStartJointCommand
        {
            get { return this._makeStartJointCommand; }
        }

        public ICommand MakeStartGapedJointCommand
        {
            get { return this._makeStartGapedJointCommand; }
        }

        public ICommand MakeEndJointCommand
        {
            get { return this._makeEndJointCommand; }
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

        private DateTime _currentDate;
        private ObservableCollection<Doctor> _doctors;
        private ObservableCollection<Schedule> _schedules;
        private Schedule _currentSchedule;
        private ObservableCollection<ScheduleDay> _currentWeek;
        private List<ResultMessage> _errors;
        
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
            set { SetProperty(ref _currentSchedule, value); }
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
            _copyScheduleToNextWeekCommand = new DelegateCommand<object>(CopyScheduleToNextWeek);
            _makeStartJointCommand = new DelegateCommand<object>(MakeStartJoint, CanMakeStartJoint);
            _makeStartGapedJointCommand = new DelegateCommand<object>(MakeStartGapedJoint, CanMakeStartGapedJoint);
            _makeEndJointCommand = new DelegateCommand<object>(MakeEndJoint, CanMakeEndJoint);
            _makeEndGapedJointCommand = new DelegateCommand<object>(MakeEndGapedJoint, CanMakeEndGapedJoint);
            _removeScheduleCommand = new DelegateCommand<object>(RemoveSchedule);
            _saveScheduleCommand = new DelegateCommand<object>(SaveSchedule);

            _startHour = int.Parse(Utils.ReadSetting("StartHour"));
            _endHour = int.Parse(Utils.ReadSetting("EndHour"));
            _cabinets = Utils.ReadSetting("Cabinets").Split(',');

            _currentDate=DateTime.Now;

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
            DateTime startDate = Utils.GetFirstDayOfWeek(_currentDate);
            DateTime endDate = Utils.GetLastDayOfWeek(_currentDate);

            MakeCurrentWeek(startDate, endDate,_cabinets);
        }

        private void MakeCurrentWeek(DateTime startDate, DateTime endDate, string[] cabinets)
        {
            CurrentWeek=new ObservableCollection<ScheduleDay>();
            ScheduleDay scheduleDay;
            ScheduleCabinet scheduleCabinet;
            while (startDate<=endDate)
            {
                scheduleDay = new ScheduleDay(startDate);
                foreach (var cabinet in cabinets)
                {
                    scheduleCabinet = new ScheduleCabinet(cabinet);

                }
                startDate = startDate.AddDays(1);
            }
        }

        private void CopyScheduleToNextWeek(object obj)
        {
            //
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
                        }
                        else
                        {
                            //_jsonClient.GetAsync(new ScheduleDelete { ScheduleId = CurrentSchedule.Id })
                            //.Success(r =>
                            //{
                            //    _eventAggregator.GetEvent<IsBusyEvent>().Publish(false); _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
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
                        }
                    }
                });
        }
        #endregion

        #region Joints
        private void MakeStartJoint(object obj)
        {
            
        }
        private bool CanMakeStartJoint(object arg)
        {
            return true;
        }

        private void MakeStartGapedJoint(object obj)
        {

        }
        private bool CanMakeStartGapedJoint(object arg)
        {
            return true;
        }
        private void MakeEndJoint(object obj)
        {

        }
        private bool CanMakeEndJoint(object arg)
        {
            return true;
        }

        private void MakeEndGapedJoint(object obj)
        {

        }
        private bool CanMakeEndGapedJoint(object arg)
        {
            return true;
        }
        #endregion
    }
}
