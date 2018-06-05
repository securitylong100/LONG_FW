using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Nidec.Mes.Framework;
namespace Com.Nidec.Mes.Framework_Server
{
    public class DefaultRemoteCbmInvokerProxy : MarshalByRefObject, RemoteCbmInvokerProxy
    {

        /// <summary>
        /// cbmcontainer instance
        /// </summary>
        private readonly CbmContainer cbmContainer = DefaultXmlCbmContainer.GetInstance();

        public bool IsServerRunning()
        {
            return true;
        }

        public ValueObject Execute(string CbmId, UserData userdata, ValueObject vo)
        {
            UserData usr = UserData.GetUserData();
            usr.UserCode = userdata.UserCode;
            usr.FactoryCode = userdata.FactoryCode;

            CbmController cbm = cbmContainer.GetCbm(CbmId);

            return DefaultCbmInvoker.Invoke(cbm, vo, userdata, UserDataSpecifiedTransactionContextFactory.GetInstance());

        }
    }
}
