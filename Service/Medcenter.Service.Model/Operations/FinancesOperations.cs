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
    #region Discount

    //[Authenticate]
    [Route("/discounts", "GET")]
    public class DiscountsSelect : IReturn<DiscountsSelectResponse>
    {
    }

    public class DiscountsSelectResponse : IHasResponseStatus
    {
        public DiscountsSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Discount> Discounts { get; set; }
    }
    [Route("/discounts/manual", "GET")]
    public class DiscountsManualSelect : IReturn<DiscountsManualSelectResponse>
    {
    }

    public class DiscountsManualSelectResponse : IHasResponseStatus
    {
        public DiscountsManualSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Discount> Discounts { get; set; }
    }
    [RequiresAnyRole("Admin", "Manager")]
    [Route("/discount/save", "POST")]
    public class DiscountSave : IReturn<DiscountSaveResponse>
    {
        public Discount Discount { get; set; }
    }

    public class DiscountSaveResponse : IHasResponseStatus
    {
        public DiscountSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int DiscountId { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/discount/delete/{DiscountId}", "GET")]
    public class DiscountDelete : IReturn<DiscountDeleteResponse>
    {
        public int DiscountId { get; set; }
    }

    public class DiscountDeleteResponse : IHasResponseStatus
    {
        public DiscountDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }


    #endregion
}
