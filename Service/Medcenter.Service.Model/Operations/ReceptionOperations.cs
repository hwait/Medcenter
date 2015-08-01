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
    #region Reception

    [Authenticate]
    [Route("/receptions/{StartDate}", "GET")]
    public class ReceptionsByDateSelect : IReturn<ReceptionsSelectResponse>
    {
        public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
    }
    [Authenticate]
    [Route("/reception/{PatientId}", "GET")]
    public class ReceptionsByIdSelect : IReturn<ReceptionsSelectResponse>
    {
        public int PatientId { get; set; }
    }
    [Authenticate]
    [Route("/reception/status/{ReceptionId}/{Status}", "GET")]
    public class ReceptionsStatusSet : IReturn<ReceptionsStatusSetResponse>
    {
        public int ReceptionId { get; set; }
        public int Status { get; set; }
    }
    public class ReceptionsStatusSetResponse : IHasResponseStatus
    {
        public ReceptionsStatusSetResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class ReceptionsSelectResponse : IHasResponseStatus
    {
        public ReceptionsSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Reception> Receptions { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/reception/save", "POST")]
    public class ReceptionSave : IReturn<ReceptionSaveResponse>
    {
        public Reception Reception { get; set; }
    }

    public class ReceptionSaveResponse : IHasResponseStatus
    {
        public ReceptionSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int ReceptionId { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/reception/delete/{ReceptionId}", "GET")]
    public class ReceptionDelete : IReturn<ReceptionDeleteResponse>
    {
        public int ReceptionId { get; set; }
    }

    public class ReceptionDeleteResponse : IHasResponseStatus
    {
        public ReceptionDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    #endregion

    #region Payment

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

    #endregion

}
