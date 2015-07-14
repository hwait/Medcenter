using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.ScheduleManagerModule.Model
{
    public class ScheduleCabinet
    {
        public string CabinetId { get; set; }
        public ObservableCollection<Schedule> Schedules { get; set; }

        public ScheduleCabinet(int cabinet)
        {
            CabinetId = (cabinet == 0) ? "" : "Каб. " + cabinet;
            Schedules=new ObservableCollection<Schedule>();
        }

        public ScheduleCabinet()
        {
            CabinetId = "";
            Schedules = new ObservableCollection<Schedule>();
        }
    }
    public class ScheduleDay
    {
        public DateTime Date { get; set; }
        public ObservableCollection<ScheduleCabinet> ScheduleCabinets { get; set; }

        public ScheduleDay(DateTime date)
        {
            Date = date;
            ScheduleCabinets=new ObservableCollection<ScheduleCabinet>();
        }

        public ScheduleDay()
        {
            
        }
    }
}
