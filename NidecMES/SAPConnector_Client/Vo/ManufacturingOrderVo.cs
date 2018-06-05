using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Nidec.Mes.Framework;
namespace Com.Nidec.Mes.SAPConnector_Client.Vo
{
    [Serializable]
   public class ManufacturingOrderVo : ValueObject
    {
        public string PlantCode { get; set; }
        
        public string ItemCd { get; set; }

        public string ItemName { get; set; }

        public string ItemType { get; set; }

        public string FactoryCd { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string MoNumber { get; set; }

        public string MoNumberFrom { get; set; }

        public string MoNumberTo { get; set; }

        public string WorkCenter { get; set; }

        public string Shift { get; set; }

        public string OrderType { get; set; }

        public string TargetQty { get; set; }

        public string MrpController { get; set; }

        public string ProductionDate { get; set; }

        public string Status { get; set; }

        public string IncludeBOM { get; set; }

        public string Source { get; set; }

        public string FinishDate { get; set; }

        public string FinishTime { get; set; }

        public string MesStatus { get; set; }


        public List<ManufacturingOrderVo> ManufacturingOrderListVo = new List<ManufacturingOrderVo>();

        public List<MoConfirmationMaterialVo> MoConfirmationMaterialListVo = new List<MoConfirmationMaterialVo>();

        public List<MRPControllerRangeVo> MRPControllerRangeListVo = new List<MRPControllerRangeVo>();

        public List<SapMessageVo> SapMessageListVo = new List<SapMessageVo>();
    }
}
