using System;
using System.Collections.Generic;
using System.Data;
using SAP.Middleware.Connector;
using Com.Nidec.Mes.Framework;
namespace Com.Nidec.Mes.SAPConnector_Client
{
    internal class DefaultSAPCommandAdaptor : SAPCommandAdapter
    {

        /// <summary>
        /// store IRfcFunction
        /// </summary>
        private readonly IRfcFunction rfcFunction;

        /// <summary>
        /// store RfcDestination
        /// </summary>
        private readonly RfcDestination rfcDestination;

        /// <summary>
        /// store RfcRepository
        /// </summary>
        private readonly RfcRepository rfcRepository;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="rfcDestination"></param>
        /// <param name="functionName"></param>
        internal DefaultSAPCommandAdaptor(RfcDestination rfcDestination, string functionName)
        {//

            this.rfcDestination = rfcDestination;

            //this.rfcRepository = rfcDestination.Repository;

            this.rfcFunction = rfcDestination.Repository.CreateFunction(functionName);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="irfcFunction"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        private IRfcFunction SetCommandParameter(IRfcFunction irfcFunction, SAPParameterList parameterList)
        {
            foreach (SAPParameter parameter in parameterList.Parameters)
            {
                if (parameter.Value == null) continue;

                if (parameter.Value.GetType() == typeof(DefaultSAPParameterList))
                {
                    IRfcTable irfcTable = rfcFunction.GetTable(parameter.Name);


                    SAPParameterList tableParameterList = (SAPParameterList)parameter.Value;
                    if (tableParameterList.ParameterLists.Length > 0)
                    {
                        foreach (SAPParameterList tableParameterLists in tableParameterList.ParameterLists)
                        {
                            irfcTable.Insert();

                            foreach (SAPParameter tableParameter in tableParameterLists.Parameters)
                            {
                                irfcTable.CurrentRow.SetValue(tableParameter.Name, tableParameter.Value);
                            }
                        }
                    }
                    else
                    {
                        irfcTable.Insert();
                        foreach (SAPParameter tableParameter in tableParameterList.Parameters)
                        {
                            irfcTable.SetValue(tableParameter.Name, tableParameter.Value);
                        }
                    }


                }
                else
                {
                    irfcFunction.SetValue(parameter.Name, parameter.Value);
                }

            }


            return irfcFunction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="irfcFunction"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        private IRfcFunction SetCommandParameterTable(IRfcFunction irfcFunction, SAPParameterList parameterList)
        {
            foreach (SAPParameter parameter in parameterList.Parameters)
            {
                IRfcTable irfcTable = rfcFunction.GetTable(parameter.Name);
                irfcTable.Insert();

                SAPParameterList tableParameterList = (SAPParameterList)parameter.Value;
                foreach (SAPParameter tableParameter in tableParameterList.Parameters)
                {
                    irfcTable.SetValue(tableParameter.Name, tableParameter.Value);
                }

            }

            return irfcFunction;
        }

        /// <summary>
        /// execute with ExecuteReader
        /// </summary>
        /// <param name="trxContext"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        public SAPFunction Execute(TransactionContext trxContext, SAPParameterList parameterList)
        {
            SetCommandParameter(rfcFunction, parameterList);

            DefaultSAPFunction sapFunction = new DefaultSAPFunction();

            sapFunction.Invoke(this.rfcDestination, rfcFunction);

            return sapFunction;

        }

        /// <summary>
        /// execute with ExecuteReader
        /// </summary>
        /// <param name="trxContext"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        public SAPFunction ExecuteTable(TransactionContext trxContext, SAPParameterList parameterList)
        {
            SetCommandParameterTable(rfcFunction, parameterList);

            DefaultSAPFunction sapFunction = new DefaultSAPFunction();

            sapFunction.Invoke(this.rfcDestination, rfcFunction);

            return sapFunction;

        }

        /// <summary>
        /// create parameterlist object
        /// </summary>
        /// <returns></returns>
        public SAPParameterList CreateParameterList()
        {
            return new DefaultSAPParameterList();

        }

    }
}
