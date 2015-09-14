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
            var rows = Db.SqlList<Reception>("EXEC sp_Receptions_Select_ByDate @Start, @End, @ScheduleId",
                new {
                    Start = startDate, 
                    End = endDate,
                    ScheduleId = req.ScheduleId
                });
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
                //reception.Discount = Db.Single<Discount>("Select * from Discounts where Id=@Id", new { Id = reception.DiscountId });
                reception.Patient = Db.Single<Patient>("Select * from Patients where Id=@Id", new { Id = reception.PatientId });
                reception.Payments = new ObservableCollection<Payment>(
                    Db.SqlList<Payment>("EXEC sp_Reception_SelectPayments @ReceptionId", new { ReceptionId = reception.Id }));
                reception.Packages = new ObservableCollection<Package>(
                    Db.SqlList<Package>("EXEC sp_Reception_SelectPackages @ReceptionId",new { ReceptionId = reception.Id }));
                foreach (var package in reception.Packages)
                {
                    package.InspectionIds=Db.SqlList<int>("EXEC sp_Package_SelectInspections @PackageId", new { PackageId = package.Id });
                }

            }
            return rows;
        }

        public ReceptionsStatusSetResponse Get(ReceptionsStatusSet req)
        {
            ResultMessage _message;
            try
            {
                var rows = Db.SqlList<Reception>("EXEC sp_ReceptionStatus_Update @Id, @Status",
                new { Id = req.ReceptionId, Status = req.Status });
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
           
            int? rid;
            if (req.Reception.RefererId == 0)
                rid = null;
            else
                rid = req.Reception.RefererId;
            if (req.Reception.Id > 0) // Reception exists so we're saving 
            {
                try
                {
                    id =
                        Db.Single<int>(
                            "EXEC sp_Receptions_Update @Id, @PatientId, @ScheduleId,@Duration,@Status,@Start,@RefererId",
                            new
                            {
                                Id          = req.Reception.Id,
                                PatientId	= req.Reception.Patient.Id,
                                ScheduleId  = req.Reception.ScheduleId,
                                Duration	= req.Reception.Duration,	
                                Status		= req.Reception.Status,		
                                Start		= req.Reception.Start,
                                RefererId = rid	
                            });
                    Db.Single<int>(
                            "EXEC sp_PackagesInReception_Delete @ReceptionId",
                            new
                            {
                                ReceptionId = req.Reception.Id
                            });
                    foreach (var package in req.Reception.Packages)
                    {
                        Db.Single<int>(
                            "EXEC sp_PackagesInReception_Insert @ReceptionId,@PackageId",
                            new
                            {
                                ReceptionId = req.Reception.Id,
                                PackageId = package.Id
                            });
                    }
                    _message = new ResultMessage(0, "Запись", OperationResults.ReceptionSave);
                    id = req.Reception.Id;
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
                            "EXEC sp_Receptions_Insert  @PatientId, @ScheduleId,@Duration,@Status,@Start,@RefererId",
                            new
                            {
                                PatientId = req.Reception.Patient.Id,
                                ScheduleId = req.Reception.ScheduleId,
                                Duration = req.Reception.Duration,
                                Status = req.Reception.Status,
                                Start = req.Reception.Start,
                                RefererId = rid
                            });
                    Db.Single<int>(
                            "EXEC sp_PackagesInReception_Delete @ReceptionId",
                            new
                            {
                                ReceptionId = id
                            });
                    foreach (var package in req.Reception.Packages)
                    {
                        Db.Single<int>(
                            "EXEC sp_PackagesInReception_Insert @ReceptionId,@PackageId",
                            new
                            {
                                ReceptionId = id,
                                PackageId = package.Id
                            });
                    }
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

        #region Payment

        public PaymentSaveResponse Post(PaymentSave req)
        {
            int id = 0;
            ResultMessage _message;
            if (req.Payment.Id > 0) // Payment exists so we're saving 
            {
                try
                {
                    id =
                        Db.Single<int>(
                            "EXEC sp_Payments_Update @Id, @ReceptionId, @DiscountId, @Cost, @FinalCost",
                            new
                            {
                                Id=req.Payment.Id,
                                ReceptionId = req.Payment.ReceptionId,
                                DiscountId = req.Payment.DiscountId,
                                Cost = req.Payment.Cost*100,
                                FinalCost = req.Payment.FinalCost
                            });
                    _message = new ResultMessage(0, "Платёж", OperationResults.PaymentCreate);
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.PaymentCreate);
                    Logger.Log("PaymentSaveResponse", e);
                    throw;
                }
            }
            else //New Payment
            {
                try
                {
                    id =
                        Db.Single<int>(
                            "EXEC sp_Payments_Insert @ReceptionId, @DiscountId, @Cost, @FinalCost",
                            new
                            {
                                ReceptionId = req.Payment.ReceptionId,
                                DiscountId = req.Payment.DiscountId,
                                Cost = req.Payment.Cost*100,
                                FinalCost = req.Payment.FinalCost
                            });
                    _message = new ResultMessage(0, "Платёж", OperationResults.PaymentCreate);
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.PaymentCreate);
                    Logger.Log("PaymentNewResponse", e);
                    throw;
                }
            }
            return new PaymentSaveResponse
            {
                PaymentId = id,
                Message = _message
            };
        }

        public PaymentDeleteResponse Get(PaymentDelete req)
        {
            ResultMessage _message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_Payments_Delete @Id, @ReceptionId", new
                {
                    Id = req.PaymentId,
                    ReceptionId = req.ReceptionId
                });
                _message = new ResultMessage(0, "Платёж", OperationResults.PaymentDelete);
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.PaymentDelete);
                Logger.Log("PaymentDeleteResponse", e);
                throw;
            }
            return new PaymentDeleteResponse {Message = _message};
        }

        #endregion

    }
}
