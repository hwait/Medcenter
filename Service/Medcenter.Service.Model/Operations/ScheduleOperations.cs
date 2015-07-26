using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Types;
using ServiceStack;

namespace Medcenter.Service.Model.Operations
{
    [Authenticate]
    [Route("/schedules", "POST")]
    public class SchedulesSelect : IReturn<SchedulesSelectResponse>
    {
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
    }

    public class SchedulesSelectResponse : IHasResponseStatus
    {
        public SchedulesSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
    //[Authenticate]
    [Route("/schedules/full/{TimeStart}", "POST")]
    public class SchedulesFullSelect : IReturn<SchedulesFullSelectResponse>
    {
        public DateTime TimeStart { get; set; }
    }

    public class SchedulesFullSelectResponse : IHasResponseStatus
    {
        public SchedulesFullSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
    [RequiresAnyRole("Admin", "Manager")]
    [Route("/schedule/save", "POST")]
    public class ScheduleSave : IReturn<ScheduleSaveResponse>
    {
        public Schedule Schedule { get; set; }
    }

    public class ScheduleSaveResponse : IHasResponseStatus
    {
        public ScheduleSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int ScheduleId { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/schedule/delete/{ScheduleId}", "GET")]
    public class ScheduleDelete : IReturn<ScheduleDeleteResponse>
    {
        public int ScheduleId { get; set; }
    }

    public class ScheduleDeleteResponse : IHasResponseStatus
    {
        public ScheduleDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
