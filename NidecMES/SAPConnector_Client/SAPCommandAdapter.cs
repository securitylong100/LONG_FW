using System;
using System.Data;
using SAP.Middleware.Connector;
using Com.Nidec.Mes.Framework;
namespace Com.Nidec.Mes.SAPConnector_Client
{
   public interface SAPCommandAdapter
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trxContext"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        SAPFunction Execute(TransactionContext trxContext, SAPParameterList parameterList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trxContext"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        SAPFunction ExecuteTable(TransactionContext trxContext, SAPParameterList parameterList);

        /// <summary>
        /// method definition for CreateParameterList
        /// </summary>
        /// <returns></returns>
        SAPParameterList CreateParameterList();


    }
}
