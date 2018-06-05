using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Nidec.Mes.Framework;

namespace Com.Nidec.Mes.SAPConnector_Client.Vo
{
    [Serializable]
    public class DispatchReportItemDtlVo : ValueObject
    {
        public int DispatchReportItemDtlId { get; set; }
        public int DispatchReportItemId { get; set; }
        public int ItemQuantity { get; set; }
        public string BoxNo { get; set; }
        public string RegistrationUserCode { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public string FactoryCode { get; set; }
        public int AffectedCount { get; set; }

        public string DispatchReportItemIds { get; set; }
    }
}
