using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.RegistratureModule.Model
{
    public class Week
    {
        public List<WeekDay> Days { get; set; }

        public void Activate(DateTime date)
        {
            for (int i = 0; i < 5; i++)
            {
                Days[i].Status = 1;
                Days[i+7].Status = 1;
            }
            Days[5].Status = 3;
            Days[6].Status = 3;
            Days[12].Status = 3;
            Days[13].Status = 3;
            (Days.Single(i => i.Date == date)).Status = 2;
        }

        public Week()
        {
            
        }
        public Week(DateTime currentDate)
        {
            Days=new List<WeekDay>(7);
            MakeDays(currentDate);
            Activate(currentDate);
        }
        public void MakeDays(DateTime currentDate)
        {

            var startDate = Utils.GetFirstDayOfWeek(currentDate);
            for (int i = 0; i < 14; i++)
            {
                Days.Add(new WeekDay(startDate.AddDays(i)));
            }
        }
    }
    public class WeekDay : BindableBase
    {
        private byte _status;
        public DateTime Date { get; set; }

        public byte Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public WeekDay()
        {
            
        }
        public WeekDay(DateTime date)
        {
            Date = date;
        }
    }
    public class CabinetReceptions
    {
        public string CabinetId { get; set; }
        public ObservableCollection<ScheduleReception> ScheduleReceptions { get; set; }
        public CabinetReceptions(int cabinet)
        {
            CabinetId = (cabinet == 0) ? "" : "Каб. " + cabinet;
            ScheduleReceptions = new ObservableCollection<ScheduleReception>();
        }

        public CabinetReceptions()
        {
            CabinetId = "";
            ScheduleReceptions = new ObservableCollection<ScheduleReception>();
        }

    }
    public class ScheduleReception
    {
        public Schedule Schedule { get; set; }
        public ObservableCollection<Reception> Receptions { get; set; }

        public ScheduleReception(Schedule schedule)
        {
            Schedule = schedule;
            Receptions=new ObservableCollection<Reception>();
        }
        public ScheduleReception(Schedule schedule, ObservableCollection<Reception> receptions)
        {
            Schedule = schedule;
            Receptions = receptions;
        }

        public ScheduleReception()
        {
            
        }
    }
}
