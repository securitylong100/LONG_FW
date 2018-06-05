using System;
using System.IO;
using System.Data;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using Com.Nidec.Mes.Framework;

namespace Com.Nidec.Mes.Framework_Server
{
    public class DefaultServerApplicationInitializer : ApplicationInitializer
    {
        /// <summary>
        /// instance of this class
        /// </summary>
        private static readonly DefaultServerApplicationInitializer instance = new DefaultServerApplicationInitializer();

        /// <summary>
        /// initialize PopUpMessageController
        /// </summary>
        private readonly PopUpMessageController popUpMessage = new PopUpMessageController();

        /// <summary>
        /// private constructor
        /// </summary>
        private DefaultServerApplicationInitializer()
        {
        }

        /// <summary>
        /// Initialize methods to be called
        /// </summary>
        public void Init()
        {
            //check mandatory application setting values.
            PreInit();

            // Set the unhandled exception mode to force all Windows Forms errors
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

            // Add the event handler for handling non-UI thread exceptions to the event.
            Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(
                            DefaultUnhandledExceptionHandler.GetInstance().HandleException);

        }

        /// <summary>
        /// check mandatory settings
        /// </summary>
        private void PreInit()
        {
            //connectionstring check
            CheckMandatorySettings(ConfigurationDataTypeEnum.CONNECTION_STRING, "fsce00013", Properties.Resources.fsce00013);

            //servertimezone check
            CheckMandatorySettings(ConfigurationDataTypeEnum.SERVER_TIME_ZONE, "fsce00015", Properties.Resources.fsce00015);

            //mandatory check needed
            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.MANDATORY_CHECK, "fsce00014", Properties.Resources.fsce00014);

            //Mandatory settings based on the xml
            CheckMandatorySettingsInXml();

        }

        private void CheckMandatorySettingsInXml()
        {
            if (ServerConfigurationDataTypeEnum.MANDATORY_CHECK.GetValue() == "1")
            {
                DataSet dsMandatoryXmlList = new DataSet();

                dsMandatoryXmlList.ReadXml(ServerConfigurationDataTypeEnum.DEFAULT_MANDATORY_CHECK_XML.GetValue());

                if (dsMandatoryXmlList == null || dsMandatoryXmlList.Tables.Count == 0)
                { return; }

                DataTable dtMandatory = dsMandatoryXmlList.Tables[0];
                if (dtMandatory.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMandatory.Rows)
                    {
                        if (string.Equals(dr["Name"].ToString(), ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_NAME.ToString()))
                        {
                            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_NAME, "fsce00023", Properties.Resources.fsce00023);
                        }
                        else if (string.Equals(dr["Name"].ToString(), ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_LOGON_GROUP.ToString()))
                        {
                            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_LOGON_GROUP, "fsce00024", Properties.Resources.fsce00024);
                        }
                        else if (string.Equals(dr["Name"].ToString(), ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_MESSAGE_SERVER_HOST.ToString()))
                        {
                            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_MESSAGE_SERVER_HOST, "fsce00025", Properties.Resources.fsce00025);
                        }
                        else if (string.Equals(dr["Name"].ToString(), ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_SYSTEM_ID.ToString()))
                        {
                            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_SYSTEM_ID, "fsce00026", Properties.Resources.fsce00026);
                        }
                        else if (string.Equals(dr["Name"].ToString(), ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_SYSTEM_NUMBER.ToString()))
                        {
                            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_SYSTEM_NUMBER, "fsce00027", Properties.Resources.fsce00027);
                        }
                        else if (string.Equals(dr["Name"].ToString(), ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_CLIENT.ToString()))
                        {
                            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_CLIENT, "fsce00030", Properties.Resources.fsce00030);
                        }
                        else if (string.Equals(dr["Name"].ToString(), ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_LANGUAGE.ToString()))
                        {
                            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_LANGUAGE, "fsce00031", Properties.Resources.fsce00031);
                        }
                        else if (string.Equals(dr["Name"].ToString(), ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_POOL_SIZE.ToString()))
                        {
                            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_POOL_SIZE, "fsce00033", Properties.Resources.fsce00033);
                        }
                        else if (string.Equals(dr["Name"].ToString(), ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_PEAK_CONNECTIONS_LIMIT.ToString()))
                        {
                            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_PEAK_CONNECTIONS_LIMIT, "fsce00034", Properties.Resources.fsce00034);
                        }
                        else if (string.Equals(dr["Name"].ToString(), ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_IDLE_TIMEOUT.ToString()))
                        {
                            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_IDLE_TIMEOUT, "fsce00035", Properties.Resources.fsce00035);
                        }
                        else if (string.Equals(dr["Name"].ToString(), ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_IDLE_CHECKTIME.ToString()))
                        {
                            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.DEFAULT_SAP_CONFIG_IDLE_CHECKTIME, "fsce00036", Properties.Resources.fsce00036);
                        }
                        else if (string.Equals(dr["Name"].ToString(), ServerConfigurationDataTypeEnum.DEFAULT_PQM_CONFIG_URL.ToString()))
                        {
                            CheckServerMandatorySettings(ServerConfigurationDataTypeEnum.DEFAULT_PQM_CONFIG_URL, "fsce00038", Properties.Resources.fsce00038);
                        }
                        else
                        {
                            MessageData messageData = new MessageData("fsce00032",  Properties.Resources.fsce00032, dr["Name"].ToString());
                            Framework.SystemException sysEx = new Framework.SystemException(messageData);

                            InitializationSystemExceptionHandler.GetInstance().HandleException(sysEx);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// check mandatory applicationsettings value
        /// </summary>
        private void CheckMandatorySettings(ConfigurationDataTypeEnum settingsdata,
                                                    string messageCode, string message)
        {
            try
            {
                string settingValue = settingsdata.GetValue();

                if (string.IsNullOrWhiteSpace(settingValue))
                {
                    MessageData messageData = new MessageData(messageCode, message, Properties.Resources.fsce00016);
                    Framework.SystemException sysEx = new Framework.SystemException(messageData);

                    InitializationSystemExceptionHandler.GetInstance().HandleException(sysEx);
                }
            }
            catch (Exception ex)
            {
                MessageData messageData = new MessageData(messageCode, message, Properties.Resources.fsce00017);
                Framework.SystemException sysEx = new Framework.SystemException(messageData, ex);

                InitializationSystemExceptionHandler.GetInstance().HandleException(sysEx);

            }
        }

        /// <summary>
        /// check mandatory applicationsettings value
        /// </summary>
        private void CheckServerMandatorySettings(ServerConfigurationDataTypeEnum settingsdata,
                                                    string messageCode, string message)
        {
            try
            {
                string settingValue = settingsdata.GetValue();

                if (string.IsNullOrWhiteSpace(settingValue))
                {
                    MessageData messageData = new MessageData(messageCode, message, Properties.Resources.fsce00016);
                    Framework.SystemException sysEx = new Framework.SystemException(messageData);

                    InitializationSystemExceptionHandler.GetInstance().HandleException(sysEx);
                }
            }
            catch (Exception ex)
            {
                MessageData messageData = new MessageData(messageCode, message, Properties.Resources.fsce00017);
                Framework.SystemException sysEx = new Framework.SystemException(messageData, ex);

                InitializationSystemExceptionHandler.GetInstance().HandleException(sysEx);

            }
        }



        /// <summary>
        /// returns the current instance 
        /// </summary>
        /// <returns></returns>
        public static DefaultServerApplicationInitializer GetInstance()
        {
            return instance;
        }
    }
}
