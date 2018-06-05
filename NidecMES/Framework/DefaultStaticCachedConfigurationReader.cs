using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using System.Collections;

namespace Com.Nidec.Mes.Framework
{
    internal class DefaultStaticCachedConfigurationReader : ConfigurationReader
    {

        private string SECTION_GROUP_NAME = "userSettings";

        private Hashtable configValueMap = new Hashtable();

        /// <summary>
        /// get the instance of CommonLogger
        /// </summary>
        private static readonly CommonLogger logger = CommonLogger.GetInstance(typeof(DefaultStaticCachedConfigurationReader));

        /// <summary>
        /// Synchronized method
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public string GetValue(string keyName)
        {
            lock (this)
            {
                return GetValueImple(keyName);
            }

        }

        /// <summary>
        /// need to execute from Synchronized method
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private string GetValueImple(string keyName)
        {

            if (configValueMap.ContainsKey(keyName))
            {
                return (string)configValueMap[keyName];
            }

            Configuration config = null;
            try
            {
                Assembly ass = Assembly.GetEntryAssembly();
                if (ass != null)
                {
                    config = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
                }
                else
                {
                   config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                    SECTION_GROUP_NAME = "applicationSettings";
                }
                               
            }
            catch (ConfigurationErrorsException Ex)
            {
                //implement system exception
                MessageData messageData = new MessageData("ffce00003", Properties.Resources.ffce00003, Ex.Message);
                logger.Error(messageData, Ex);

                throw new SystemException(messageData);
            }

            if (!config.HasFile)
            {
                //implement system exception
                MessageData messageData = new MessageData("ffce00004", Properties.Resources.ffce00004);
                logger.Error(messageData);

                throw new SystemException(messageData);
            }

            ConfigurationSectionGroup sectionGroup = config.SectionGroups[SECTION_GROUP_NAME];

            if (sectionGroup == null)
            {
                //implement system exception
                MessageData messageData = new MessageData("ffce00005", Properties.Resources.ffce00005);
                logger.Error(messageData);

                throw new SystemException(messageData);
            }

            string value = null;

            foreach (ConfigurationSection configurationSection in sectionGroup.Sections)
            {

                ClientSettingsSection section = null;
                try
                {
                    section = (ClientSettingsSection)ConfigurationManager
                                            .GetSection(configurationSection.SectionInformation.SectionName);
                }
                catch (ConfigurationErrorsException ex)
                {
                    //implement system exception
                    MessageData messageData = new MessageData("ffce00006", Properties.Resources.ffce00006, ex.Message);
                    logger.Error(messageData,ex);

                    throw new SystemException(messageData);
                }

                SettingElement element = section.Settings.Get(keyName);
                if (element == null)
                {
                    //implement system exception
                    MessageData messageData = new MessageData("ffce00007", Properties.Resources.ffce00007, keyName);
                    logger.Error(messageData);

                    throw new SystemException(messageData);
                }

                value = element.Value.ValueXml.InnerText;
                break;
            }

            //set to cache
            configValueMap.Add(keyName, value);

            return value;

        }

    }
}
