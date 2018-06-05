using Com.Nidec.Mes.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Reflection;
using System;
using System.Linq;
using System.Configuration;

namespace Com.Nidec.Mes.Framework_Server
{
    public class DefaultXmlCbmContainer : CbmContainer
    {

        /// <summary>
        /// get the instance of CommonLogger
        /// </summary>
        private static readonly CbmContainer INSTANCE = new DefaultXmlCbmContainer();

        /// <summary>
        /// get the initialized flag
        /// </summary>
        private bool initializedFlg = false;

        /// <summary>
        /// store cbm instance
        /// </summary>
        private Dictionary<string, CbmController> dicAssemblyTypeObj = new Dictionary<string, CbmController>();

        /// <summary>
        /// get the instance of CommonLogger
        /// </summary>
        private static readonly CommonLogger logger = CommonLogger.GetInstance(typeof(DefaultXmlCbmContainer));


        private string assemblyname;


        private bool isWebService;

        /// <summary>
        /// 
        /// </summary>
        private DefaultXmlCbmContainer() { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static CbmContainer GetInstance()
        {
            return INSTANCE;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init(string assemblyname, bool isWebService = false)
        {
            lock (INSTANCE)
            {
                this.assemblyname = assemblyname;

                this.isWebService = isWebService;

                // load each XML and Cached to the map objecct cbmTableListForEachAssmebly
                LoadCbmXmlAndCreateCbmInstance();

                initializedFlg = true;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="cbmcontrollerId"></param>
        /// <returns></returns>
        public CbmController GetCbm(string cbmcontrollerId)
        {
            lock (INSTANCE)
            {
                if (!initializedFlg)
                {

                    Init(this.assemblyname);

                }

                try
                {
                    CbmController cbmController = (CbmController)dicAssemblyTypeObj.First(c => c.Key == cbmcontrollerId).Value;
                    return cbmController;
                }
                catch (ArgumentNullException exception)
                {
                    MessageData messageData = new MessageData("fsci00002", Properties.Resources.fsci00002, new string[] { cbmcontrollerId, exception.Message });
                    logger.Error(messageData, exception);

                    throw new Framework.ApplicationException(messageData, exception);
                }
                catch (InvalidOperationException exception)
                {
                    MessageData messageData = new MessageData("fsci00002", Properties.Resources.fsci00002, new string[] { cbmcontrollerId, exception.Message });
                    logger.Error(messageData, exception);

                    throw new Framework.ApplicationException(messageData, exception);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadCbmXmlAndCreateCbmInstance()
        {

            string filePath = AppDomain.CurrentDomain.BaseDirectory + ServerConfigurationDataTypeEnum.DEFAULT_CBM_LIST_XML.GetValue();

            if (!System.IO.File.Exists(filePath))
            {
                //logging
                return;
            }

            DataSet dsCbm = new DataSet();

            //Read Xml file from Exection Path
            dsCbm.ReadXml(filePath);

            Assembly assembly = LoadAssembly(this.assemblyname);

            if (assembly == null)
            {
                //log
                return;

            }


            DataTable dtCbm = dsCbm.Tables[0];

            if (dtCbm.Rows.Count == 0)
            {
                //logging
                return;
            }

            foreach (DataRow dr in dtCbm.Rows)
            {
                CbmController cbmController = CbmInstance(assembly, dr["Name"].ToString());

                if (cbmController != null)
                {
                    dicAssemblyTypeObj.Add(dr["ID"].ToString(), cbmController);

                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cbmAssblyName"></param>
        /// <returns></returns>
        private Assembly LoadAssembly(string cbmAssblyName)
        {
            try
            {
                string assemblyFilePath = AppDomain.CurrentDomain.BaseDirectory + cbmAssblyName;
                if (isWebService)
                {
                    assemblyFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\bin\\" + cbmAssblyName;
                }
                Assembly assembly = Assembly.LoadFile(assemblyFilePath);

                return assembly;
            }
            catch (ArgumentNullException exception)
            {
                MessageData messageData = new MessageData("fsce00001", Properties.Resources.fsce00001, new string[] { cbmAssblyName, exception.Message });
                logger.Error(messageData, exception);

                throw new Framework.ApplicationException(messageData, exception);
            }
            catch (System.IO.FileLoadException exception)
            {
                MessageData messageData = new MessageData("fsce00001", Properties.Resources.fsce00001, new string[] { cbmAssblyName, exception.Message });
                logger.Error(messageData, exception);

                throw new Framework.ApplicationException(messageData, exception);
            }
            catch (System.IO.FileNotFoundException exception)
            {
                MessageData messageData = new MessageData("fsce00001", Properties.Resources.fsce00001, new string[] { cbmAssblyName, exception.Message });
                logger.Error(messageData, exception);

                throw new Framework.ApplicationException(messageData, exception);
            }
            catch (BadImageFormatException exception)
            {
                MessageData messageData = new MessageData("fsce00001", Properties.Resources.fsce00001, new string[] { cbmAssblyName, exception.Message });
                logger.Error(messageData, exception);

                throw new Framework.ApplicationException(messageData, exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="cbmName"></param>
        /// <returns></returns>
        private CbmController CbmInstance(Assembly assembly, string cbmName)
        {
            try
            {
                CbmController cbmController = (CbmController)assembly.CreateInstance(cbmName);

                return cbmController;
            }
            catch (ArgumentException exception)
            {
                MessageData messageData = new MessageData("fsce00002", Properties.Resources.fsce00002, new string[] { cbmName, exception.Message });
                logger.Error(messageData, exception);

                throw new Framework.ApplicationException(messageData, exception);
            }
            catch (MissingMethodException exception)
            {
                MessageData messageData = new MessageData("fsce00002", Properties.Resources.fsce00002, new string[] { cbmName, exception.Message });
                logger.Error(messageData, exception);

                throw new Framework.ApplicationException(messageData, exception);
            }
            catch (System.IO.FileLoadException exception)
            {
                MessageData messageData = new MessageData("fsce00002", Properties.Resources.fsce00002, new string[] { cbmName, exception.Message });
                logger.Error(messageData, exception);

                throw new Framework.ApplicationException(messageData, exception);
            }
            catch (System.IO.FileNotFoundException exception)
            {
                MessageData messageData = new MessageData("fsce00002", Properties.Resources.fsce00002, new string[] { cbmName, exception.Message });
                logger.Error(messageData, exception);

                throw new Framework.ApplicationException(messageData, exception);
            }
            catch (BadImageFormatException exception)
            {
                MessageData messageData = new MessageData("fsce00002", Properties.Resources.fsce00002, new string[] { cbmName, exception.Message });
                logger.Error(messageData, exception);

                throw new Framework.ApplicationException(messageData, exception);
            }
        }
    }
}
