using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using ServiceStack.OrmLite;

namespace Medcenter.Service.Model.Operations
{
    public class PaymentService : ServiceStack.Service
    {
        public PaymentSaveResponse Post(PaymentSave req)
        {
            int id = 0;
            ResultMessage _message;
            //Only New Payment is permitted
            try
            {
                id =
                    Db.Single<int>(
                        "EXEC sp_Payments_Insert  @ReceptionId, @Cost",
                        new
                        {
                            ReceptionId = req.Payment.ReceptionId,
                            Cost = req.Payment.Cost
                        });
                _message = new ResultMessage(0, "Платёж", OperationResults.PaymentCreate);
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.PaymentCreate);
                Logger.Log("PaymentSaveResponse", e);
                throw;
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
            return new PaymentDeleteResponse { Message = _message };
        }
    }
}
