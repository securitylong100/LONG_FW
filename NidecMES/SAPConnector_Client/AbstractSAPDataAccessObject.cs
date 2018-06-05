using System;
using System.Data;
using SAP.Middleware.Connector;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;
using Com.Nidec.Mes.GlobalMasterMaintenance.Cbm;
namespace Com.Nidec.Mes.SAPConnector_Client
{
    public abstract class AbstractSAPDataAccessObject : DataAccessObject
    {

        /// <summary>
        /// initialize CommonLogger
        /// </summary>
        private static readonly CommonLogger logger = CommonLogger.GetInstance(typeof(AbstractSAPDataAccessObject));

        private static RfcConfigParameters configParameters;

        private static RfcDestination rfcDestination;

        /// <summary>
        /// method definition for Execute
        /// </summary>
        /// <param name="trxContext"></param>
        /// <param name="vo"></param>
        /// <returns></returns>
        public abstract ValueObject Execute(TransactionContext trxContext, ValueObject vo);

        //
        /// <summary>
        /// get DefaultNpgCommandAdaptor
        /// </summary>
        /// <param name="trxContext"></param>
        /// <param name="sqlText"></param>
        /// <returns></returns>
        protected SAPCommandAdapter GetSAPCommandAdaptor(TransactionContext trxContext, String sqlText)
        {

            if (configParameters == null)
            {
                configParameters = SetConnectionParameters();
            }

            if (rfcDestination == null)
            {
                rfcDestination = RfcDestinationManager.GetDestination(configParameters);
            }

            SAPCommandAdapter sapCommandAdapter = new DefaultSAPCommandAdaptor(rfcDestination, sqlText);


            return sapCommandAdapter;
        }

        private static RfcDestination getRfcdestination()
        {
            return RfcDestinationManager.GetDestination(configParameters);
        }

        protected Type ConvertNull<Type>(DataRow dr, String columName)
        {
            return (Type)(dr[columName].Equals(null) ? default(Type) : dr[columName]);
        }


        protected bool IsValid(string datetime)
        {
            DateTime outDate;

            if (DateTime.TryParse(datetime, out outDate))
            {
                return true;
            }
            return false;
        }
        private static RfcConfigParameters SetConnectionParameters()
        {
            //RfcCustomDestination dest= getRfcdestination().CreateCustomDestination();

            SapUserVo inVo = new SapUserVo();
            GetSapUserMasterMntCbm sapUserCbm = new GetSapUserMasterMntCbm();

            SapUserVo outVo = null;

            try
            {
                outVo = (SapUserVo)DefaultCbmInvoker.Invoke(sapUserCbm, inVo);
            }
            catch (Framework.ApplicationException ex)
            {
                MessageData messagedata = new MessageData("scce00002", Properties.Resources.scce00002);
                logger.Error(messagedata);

                throw new Framework.ApplicationException(messagedata, ex);
            }

            if (outVo == null)
            {
                MessageData messagedata = new MessageData("scce00001", Properties.Resources.scce00001,UserData.GetUserData().UserCode);
                logger.Error(messagedata);

                throw new Framework.ApplicationException(messagedata);
            }
            RfcConfigParameters rfcParams = new RfcConfigParameters();
            rfcParams.Add(RfcConfigParameters.Name, ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_NAME.GetValue());
            rfcParams.Add(RfcConfigParameters.LogonGroup,  ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_LOGON_GROUP.GetValue());
            rfcParams.Add(RfcConfigParameters.MessageServerHost,  ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_MESSAGE_SERVER_HOST.GetValue());
            rfcParams.Add(RfcConfigParameters.SystemID, ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_SYSTEM_ID.GetValue());
            rfcParams.Add(RfcConfigParameters.SystemNumber, ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_SYSTEM_NUMBER.GetValue());
            rfcParams.Add(RfcConfigParameters.User, outVo.SapUser); 
            rfcParams.Add(RfcConfigParameters.Password, outVo.SapPassWord);
            rfcParams.Add(RfcConfigParameters.Client, ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_CLIENT.GetValue());
            rfcParams.Add(RfcConfigParameters.Language,  ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_LANGUAGE.GetValue());
            rfcParams.Add(RfcConfigParameters.PoolSize, ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_POOL_SIZE.GetValue());
            rfcParams.Add(RfcConfigParameters.PeakConnectionsLimit, ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_PEAK_CONNECTIONS_LIMIT.GetValue());
            rfcParams.Add(RfcConfigParameters.IdleTimeout, ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_IDLE_TIMEOUT.GetValue());
            rfcParams.Add(RfcConfigParameters.IdleCheckTime, ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_IDLE_CHECKTIME.GetValue());

            return rfcParams;
        }
    }
}
