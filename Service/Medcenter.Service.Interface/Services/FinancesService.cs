﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using Newtonsoft.Json;
using ServiceStack.OrmLite;

namespace Medcenter.Service.Interface.Services
{
    public class FinancesService : ServiceStack.Service
    {
        #region Discount

        public DiscountsSelectResponse Get(DiscountsSelect req)
        {
            var rows = Db.SqlList<Discount>("EXEC sp_Discounts_Select");
            //foreach (var r in rows)
                //r.DiscountPackageIds = Db.SqlList<int>("EXEC sp_Discount_SelectGroups @DiscountId", new { DiscountId = r.Id });
            return new DiscountsSelectResponse { Discounts = new List<Discount>(rows) };
        }

        public DiscountSaveResponse Post(DiscountSave req)
        {
            int id = 0;
            ResultMessage _message;
            if (req.Discount.Id > 0) // Discount exists so we're saving 
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_Discounts_Update @Id, @Requirements, @Name, @Value, @IsGlobal", new
                    {
                        Id = req.Discount.Id,
                        ShortName = req.Discount.Value,
                        Requirements=JsonConvert.SerializeObject(req.Discount),
                        Name = req.Discount.Name,
                        Duration = req.Discount.IsGlobal
                    });
                    _message = new ResultMessage(0, "Сохранение скидки", OperationResults.DiscountSave);
                    Logger.Log("DiscountSaveResponse.Saving");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.DiscountSave);
                    Logger.Log("DiscountSaveResponse.Saving", e);
                    throw;
                }
            }
            else //New Discount
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_Discounts_Insert @Requirements, @Name, @Value, @IsGlobal", new
                    {
                        ShortName = req.Discount.Value,
                        Requirements = JsonConvert.SerializeObject(req.Discount),
                        Name = req.Discount.Name,
                        Duration = req.Discount.IsGlobal
                    });
                    _message = new ResultMessage(0, "Новая скидка", OperationResults.DiscountCreate);
                    Logger.Log("DiscountSaveResponse.NewDiscount");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.DiscountCreate);
                    Logger.Log("DiscountSaveResponse.NewDiscount", e);
                    throw;
                }
            }

            return new DiscountSaveResponse
            {
                DiscountId = id,
                Message = _message
            };
        }

        public DiscountDeleteResponse Get(DiscountDelete req)
        {
            ResultMessage _message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_Discounts_Delete @Id", new
                {
                    Id = req.DiscountId
                });
                _message = new ResultMessage(0, "Скидки:", OperationResults.DiscountDelete);
                Logger.Log("DiscountDeleteResponse");
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.DiscountDelete);
                Logger.Log("DiscountDeleteResponse", e);
                throw;
            }
            return new DiscountDeleteResponse { Message = _message };
        }

        #endregion
    }
}