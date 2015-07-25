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
    [RequiresAnyRole("Admin", "Manager")]
    [Route("/payment/save", "POST")]
    public class PaymentSave : IReturn<PaymentSaveResponse>
    {
        public Payment Payment { get; set; }
    }

    public class PaymentSaveResponse : IHasResponseStatus
    {
        public PaymentSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int PaymentId { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/payment/delete/{PaymentId}/{ReceptionId}", "GET")]
    public class PaymentDelete : IReturn<PaymentDeleteResponse>
    {
        public int PaymentId { get; set; }
        public int ReceptionId { get; set; }
    }

    public class PaymentDeleteResponse : IHasResponseStatus
    {
        public PaymentDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

}
