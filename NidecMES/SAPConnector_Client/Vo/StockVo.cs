using Com.Nidec.Mes.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Nidec.Mes.SAPConnector_Client.Vo
{
    [Serializable]
    public class StockVo : ValueObject
    {
        /// <summary>
        /// In-Parameters
        /// </summary>
        public string PlantCode { get; set; }
        public string FromMaterialNumber { get; set; }
        public string ToMaterialNumber { get; set; }
        public string SapBatchNumber { get; set; }
        public string VendorCode { get; set; }
        public string VendorBatchNumber { get; set; }
        public string StorageLocationCode { get; set; }

        /// <summary>
        /// Out-Parameters not included in In-Papameters
        /// </summary>
        public string ItemNumber { get; set; }

        public int DemandQty { get; set; }

        public int StockQty { get; set; }

        public string WarehouseCode { get; set; }

        public string InternalLot { get; set; }

        public string SupplierCode { get; set; }

        public string SupplierName { get; set; }

        public string SupplierLot { get; set; }

        public string OrderStr { get; set; }

        public int PlanToConsume { get; set; }

        public int StockReserve { get; set; }

        public int StockReserve2 { get; set; }

        public string VendorName { get; set; }

        public int UnrestrictedStock { get; set; }

        public List<StockVo> StockListVo = new List<StockVo>();

        public List<SapMessageVo> SapMessageListVo = new List<SapMessageVo>();
    }
}
