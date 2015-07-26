using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.RegistratureModule.Model
{
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
