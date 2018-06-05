using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Nidec.Mes.GlobalMasterMaintenance
{
    internal class GlobalMasterDataTypeEnum
    {
        private string valueName;

        private string key;

        private GlobalMasterDataTypeEnum(string key, string valueName)
        {
            this.key = key;

            this.valueName = valueName;
        }

        public override string ToString()
        {
            return key;
        }

        public string GetValue()
        {
            return valueName;
        }

        public static readonly GlobalMasterDataTypeEnum SINGLE_LINE = new GlobalMasterDataTypeEnum(Properties.Resources.SINGLE_LINE, "1");

        public static readonly GlobalMasterDataTypeEnum SINGLE_MACHINE = new GlobalMasterDataTypeEnum(Properties.Resources.SINGLE_MACHINE, "2");

        public static readonly GlobalMasterDataTypeEnum SINGLE_LINE_MACHINE = new GlobalMasterDataTypeEnum(Properties.Resources.SINGLE_LINE_MACHINE, "3");

    }
}
