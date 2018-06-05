using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.SAPConnector_Client.Vo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Nidec.Mes.SAPConnector_Client.Dao
{
   public class GetStockDao : AbstractSAPDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject vo)
        {
                       
            StockVo inVo = (StockVo)vo;

            //create command
            SAPCommandAdapter sapCommandAdapter = base.GetSAPCommandAdaptor(trxContext, SAPRFCNameEnum.RFC_STOCK.GetValue());
            
            //create parameter
            SAPParameterList sapParameter = sapCommandAdapter.CreateParameterList();
            sapParameter.AddParameter("IM_PLANT", inVo.PlantCode);
            sapParameter.AddParameter("IM_FROM_MATERIAL", inVo.FromMaterialNumber);
            sapParameter.AddParameter("IM_TO_MATERIAL", inVo.ToMaterialNumber);
            sapParameter.AddParameter("IM_LGORT", inVo.StorageLocationCode);

            SAPFunction sapFuntion = sapCommandAdapter.Execute(trxContext, sapParameter);

            DataTable sapTable = sapFuntion.GetSAPTable("TB_STOCK_DATA");

            //StockVo outVo = new StockVo();
            string stockQty;
            StockVo outVo = new StockVo();

            foreach (DataRow dr in sapTable.Rows)
            {
                StockVo currOutVo = new StockVo();
                stockQty = ConvertNull<string>(dr, "UNRESTRICTED_STCK");
                currOutVo.UnrestrictedStock = (int)Convert.ToDecimal(stockQty);

                currOutVo.ItemNumber = ConvertNull<string>(dr,"MATERIAL");
                currOutVo.StockQty  = (int)Convert.ToDecimal(stockQty);
                currOutVo.WarehouseCode = ConvertNull<string>(dr, "LGORT");
                currOutVo.InternalLot = ConvertNull<string>(dr, "BATCH");
                currOutVo.SupplierCode = ConvertNull<string>(dr, "VENDOR");
                currOutVo.SupplierName = ConvertNull<string>(dr, "V_DESC");
                currOutVo.OrderStr = ConvertNull<string>(dr, "GR_DATE") + ConvertNull<string>(dr, "GR_TIME") + ConvertNull<string>(dr, "BATCH");
                currOutVo.PlanToConsume = currOutVo.UnrestrictedStock;
                currOutVo.StockReserve = currOutVo.UnrestrictedStock;
                currOutVo.StockReserve2 = currOutVo.UnrestrictedStock;

                currOutVo.SapBatchNumber = ConvertNull<string>(dr,"BATCH");
                currOutVo.VendorBatchNumber = ConvertNull<string>(dr,"V_BATCH");
                
                outVo.StockListVo.Add(currOutVo);
            }


            List<SapMessageVo> messageList = new List<SapMessageVo>();
            DataTable sapMessageTable = sapFuntion.GetSAPTable("TB_RETURN");
            foreach (DataRow dr in sapMessageTable.Rows)
            {
                SapMessageVo message = new SapMessageVo
                {
                    MessageType = ConvertNull<string>(dr, "TYPE"),
                    MessageClassId = ConvertNull<string>(dr, "ID"),
                    MessageNumber = ConvertNull<string>(dr, "NUMBER"),
                    LogNumber = ConvertNull<string>(dr, "MESSAGE"),
                    LogMessageNumber = ConvertNull<string>(dr, "LOG_NO"),
                    MessageVariable1 = ConvertNull<string>(dr, "MESSAGE_V1"),
                    MessageVariable2 = ConvertNull<string>(dr, "MESSAGE_V2"),
                    MessageVariable3 = ConvertNull<string>(dr, "MESSAGE_V3"),
                    MessageVariable4 = ConvertNull<string>(dr, "MESSAGE_V4")
                };
                messageList.Add(message);
            }

            outVo.SapMessageListVo = messageList;
            
            return outVo;
        }
    }
}
