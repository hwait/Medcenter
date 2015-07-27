using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using Newtonsoft.Json;
using ServiceStack.Messaging;
using ServiceStack.OrmLite;

namespace Medcenter.Service.Interface.Services
{
    public class ReceptionService : ServiceStack.Service
    {

        #region Reception

        public ReceptionsSelectResponse Get(ReceptionsByDateSelect req)
        {
            var startDate = new DateTime(req.StartDate.Year, req.StartDate.Month, req.StartDate.Day, 0, 0, 0);
            var endDate = new DateTime(req.StartDate.Year, req.StartDate.Month, req.StartDate.Day, 23, 59, 59);
            var rows = Db.SqlList<Reception>("EXEC sp_Receptions_Select_ByDate @Start, @End",
                new { Start = startDate, End = endDate });
            return new ReceptionsSelectResponse { Receptions = FillReceptions(rows) };
        }

        public ReceptionsSelectResponse Get(ReceptionsByIdSelect req)
        {
            var rows = Db.SqlList<Reception>("EXEC sp_Receptions_Select_ByPatient @PatientId",
                new { PatientId = req.PatientId });
            return new ReceptionsSelectResponse { Receptions = FillReceptions(rows) };
        }

        private List<Reception> FillReceptions(List<Reception> rows)
        {
            foreach (var reception in rows)
            {
                reception.Discount = Db.Single<Discount>("Select * from Discounts where Id=@Id", new { Id = reception.DiscountId });
                reception.Payments = new ObservableCollection<Payment>(
                    Db.SqlList<Payment>("EXEC sp_Reception_SelectPayments @ReceptionId", new { ReceptionId = reception.Id }));
                reception.Packages = new ObservableCollection<Package>(
                    Db.SqlList<Package>("EXEC sp_Reception_SelectPackages @ReceptionId",new { ReceptionId = reception.Id }));
            }
            return rows;
        }

        public ReceptionsStatusSetResponse Get(ReceptionsStatusSet req)
        {
            ResultMessage _message;
            try
                {
                var rows = Db.SqlList<Reception>("EXEC sp_ReceptionStatus_Update @Id, @Status",
                new { ReceptionId = req.ReceptionId, Status = req.Status });
                _message = new ResultMessage(0, "Статус", OperationResults.ReceptionStatus);
                }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.ReceptionStatus);
                Logger.Log("ReceptionsStatusSetResponse.Saving ", e);
                throw;
            }
            return new ReceptionsStatusSetResponse { Message = _message };
        }
        public ReceptionSaveResponse Post(ReceptionSave req)
        {
            int id = 0;
            ResultMessage _message;
            int? did;
            if (req.Reception.Discount == null) 
                did=null;
            else
                did=req.Reception.Discount.Id;
            int? rid;
            if (req.Reception.RefererId == 0)
                rid = null;
            else
                rid = req.Reception.RefererId;
            if (req.Reception.Id > 0) // Package exists so we're saving 
            {
                try
                {
                    id =
                        Db.Single<int>(
                            "EXEC sp_Receptions_Update @Id, @PatientId, @ScheduleId,@DiscountId,@Duration,@Status,@Start,@RefererId",
                            new
                            {
                                Id          = req.Reception.Id,
                                PatientId	= req.Reception.PatientId,
                                ScheduleId  = req.Reception.ScheduleId,
                                DiscountId = did, 
                                Duration	= req.Reception.Duration,	
                                Status		= req.Reception.Status,		
                                Start		= req.Reception.Start,
                                RefererId = rid	
                            });
                    _message = new ResultMessage(0, "Запись", OperationResults.ReceptionSave);
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.ReceptionSave);
                    Logger.Log("ReceptionSaveResponse.Saving ", e);
                    throw;
                }
            }
            else //New Reception
            {
                try
                {
                    id =
                        Db.Single<int>(
                            "EXEC sp_Receptions_Insert  @PatientId, @ScheduleId,@DiscountId,@Duration,@Status,@Start,@RefererId",
                            new
                            {
                                PatientId = req.Reception.PatientId,
                                ScheduleId = req.Reception.ScheduleId,
                                DiscountId = did,
                                Duration = req.Reception.Duration,
                                Status = req.Reception.Status,
                                Start = req.Reception.Start,
                                RefererId = rid
                            });
                    _message = new ResultMessage(0, "Запись", OperationResults.ReceptionCreate);
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.ReceptionCreate);
                    Logger.Log("ReceptionSaveResponse.NewPackage", e,req.ToString());
                    throw;
                }
            }

            return new ReceptionSaveResponse
            {
                ReceptionId = id,
                Message = _message
            };
        }

        public ReceptionDeleteResponse Get(ReceptionDelete req)
        {
            ResultMessage _message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_Receptions_Delete @Id", new
                {
                    Id = req.ReceptionId
                });
                _message = new ResultMessage(0, "Запись", OperationResults.ReceptionDelete);
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.ReceptionDelete);
                Logger.Log("ReceptionDeleteResponse", e);
                throw;
            }
            return new ReceptionDeleteResponse { Message = _message };
        }

        #endregion
    }
}
