using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Nidec.Mes.Framework;
using System.Web.Services.Protocols;
namespace Com.Nidec.Mes.Framework_Server
{
    public class DefaultWebServiceCbmInvokerProxy
    {

        /// <summary>
        /// cbmcontainer instance
        /// </summary>
        private static readonly CbmContainer cbmContainer = DefaultXmlCbmContainer.GetInstance();

        /// <summary>
        /// Execute cbm with userdataspecifiedtransaction
        /// </summary>
        /// <param name="CbmId"></param>
        /// <param name="userdata"></param>
        /// <param name="vo"></param>
        /// <returns></returns>
        public static ValueObject Execute(string CbmId, UserData userdata, ValueObject vo)
        {
            if (userdata != null)
            {
                UserData usr = UserData.GetUserData();
                usr.UserCode = userdata.UserCode;
                usr.FactoryCode = userdata.FactoryCode;
            }
            CbmController cbm;

            ///get cbm instance
            try
            {
                cbm = cbmContainer.GetCbm(CbmId);
            }
            catch (Framework.ApplicationException ex)
            {
                throw new SoapException(string.Empty, SoapException.ServerFaultCode, "ApplicationException", ex);
            }
            catch (Framework.SystemException ex)
            {
                throw new SoapException(string.Empty, SoapException.ServerFaultCode, "SystemException", ex);
            }
            catch (Exception ex)
            {
                throw new SoapException(string.Empty, SoapException.ServerFaultCode, "SystemException", ex);
            }

            ///execute cbm
            try
            {
                return DefaultCbmInvoker.Invoke(cbm, vo, userdata, UserDataSpecifiedTransactionContextFactory.GetInstance());
            }
            catch (Framework.ApplicationException ex)
            {
                throw new SoapException(string.Empty, SoapException.ServerFaultCode, "ApplicationException", ex);
            }
            catch (Framework.SystemException ex)
            {
                throw new SoapException(string.Empty, SoapException.ServerFaultCode, "SystemException", ex);
            }
            catch (Exception ex)
            {
                throw new SoapException(string.Empty, SoapException.ServerFaultCode, "SystemException", ex);
            }
        }

    }
}
