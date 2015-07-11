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
    }
    public class ScheduleDay
    {
        public DateTime Date { get; set; }
        public ObservableCollection<ScheduleCabinet> ScheduleCabinets { get; set; }
    }
}
