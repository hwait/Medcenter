using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using ServiceStack.OrmLite;

namespace Medcenter.Service.Interface.Services
{
    public class ScheduleService : ServiceStack.Service
    {
        public SchedulesSelectResponse Post(SchedulesSelect req)
        {
            var rows = Db.SqlList<Schedule>("EXEC sp_Schedules_Select @TimeStart, @TimeEnd", 
                new { TimeStart = req.TimeStart, TimeEnd = req.TimeEnd });
            return new SchedulesSelectResponse { Schedules = rows };
        }
        public SchedulesFullSelectResponse Post(SchedulesFullSelect req)
        {
            var rows=new List<Schedule>();
            try
            {
                var startDate = new DateTime(req.TimeStart.Year, req.TimeStart.Month, req.TimeStart.Day, 0, 0, 0);
                var endDate = new DateTime(req.TimeStart.Year, req.TimeStart.Month, req.TimeStart.Day, 23, 59, 59);

                rows = Db.SqlList<Schedule>("EXEC sp_Schedules_Select @TimeStart, @TimeEnd",
                    new { TimeStart = startDate, TimeEnd = endDate });
                foreach (var schedule in rows)
                {
                    schedule.CurrentDoctor = Db.Single<Doctor>("select * from Doctors where Id=@Id",
                        new { Id = schedule.DoctorId });
                }
            }
            catch (Exception e)
            {
                Logger.Log("SchedulesFullSelectResponse ", e);
                throw;
            }
            return new SchedulesFullSelectResponse { Schedules = rows };
        }
        public ScheduleSaveResponse Post(ScheduleSave req)
        {
            int id = 0;
            ResultMessage _message;
            if (req.Schedule.Id > 0) // Package exists so we're saving 
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_Schedules_Update @Id, @TimeStart, @TimeEnd, @CabinetId,@DoctorId", new
                    {
                        Id = req.Schedule.Id,
                        TimeStart = req.Schedule.Start,
                        TimeEnd = req.Schedule.End,
                        CabinetId = req.Schedule.CabinetId,
                        DoctorId = req.Schedule.CurrentDoctor.Id,
                    });
                    _message = new ResultMessage(0, "Расписание", OperationResults.ScheduleSave);
                    //Logger.Log("ScheduleSaveResponse.Saving");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.ScheduleSave);
                    Logger.Log("ScheduleSaveResponse.Saving ", e);
                    throw;
                }
            }
            else //New Schedule
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_Schedules_Insert @TimeStart, @TimeEnd, @CabinetId,@DoctorId", new
                    {
                        TimeStart = req.Schedule.Start,
                        TimeEnd = req.Schedule.End,
                        CabinetId = req.Schedule.CabinetId,
                        DoctorId = req.Schedule.CurrentDoctor.Id
                    });
                    _message = new ResultMessage(0, "Расписание", OperationResults.ScheduleCreate);
                    //Logger.Log("PackageSaveResponse.NewPackage");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.ScheduleCreate);
                    Logger.Log("PackageSaveResponse.NewPackage", e);
                    throw;
                }
            }

            return new ScheduleSaveResponse
            {
                ScheduleId = id,
                Message = _message
            };
        }

        public ScheduleDeleteResponse Get(ScheduleDelete req)
        {
            ResultMessage _message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_Schedules_Delete @Id", new
                {
                    Id = req.ScheduleId
                });
                _message = new ResultMessage(0, "Расписание:", OperationResults.ScheduleDelete);
                //Logger.Log("ScheduleDeleteResponse");
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.ScheduleDelete);
                Logger.Log("ScheduleDeleteResponse", e);
                throw;
            }
            return new ScheduleDeleteResponse { Message = _message };
        }
    }
}
