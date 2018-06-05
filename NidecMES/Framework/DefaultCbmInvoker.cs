using System;
using System.Data;
using Npgsql;

namespace Com.Nidec.Mes.Framework
{
    public class DefaultCbmInvoker
    {

        /// <summary>
        /// get the instance of CommonLogger
        /// </summary>
        private static readonly CommonLogger logger = CommonLogger.GetInstance(typeof(DefaultCbmInvoker));

        private static readonly TransactionContextFactory defaultTrxFactory = new DefaultTransactionContextFactory();

        /// <summary>
        /// executing transaction 
        /// </summary>
        /// <param name="cbm"></param>
        /// <param name="vo"></param>
        /// <returns></returns>
        public static ValueObject Invoke(CbmController cbm, ValueObject vo)
        {

            string connectionString = ConfigurationDataTypeEnum.CONNECTION_STRING.GetValue(); // Properties.Settings.Default.ConnectionString;

            return Invoke(cbm, vo, connectionString);

        }

        /// <summary>
        /// invoking transaction for execution
        /// </summary>
        /// <param name="cbm"></param>
        /// <param name="vo"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static ValueObject Invoke(CbmController cbm, ValueObject vo, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                MessageData messageData = new MessageData("ffce00019", Properties.Resources.ffce00019);
                logger.Error(messageData, new NullReferenceException());
                throw new SystemException(messageData, new NullReferenceException());
            }   
            return Invoke(cbm, vo, null, defaultTrxFactory, connectionString);
        }

        /// <summary>
        /// invoking and executing transaction using userdata and transaxtioncontextfacctory
        /// </summary>
        /// <param name="cbm"></param>
        /// <param name="vo"></param>
        /// <param name="userData"></param>
        /// <param name="trxFactory"></param>
        /// <returns></returns>
        public static ValueObject Invoke(CbmController cbm, ValueObject vo, UserData userData, TransactionContextFactory trxFactory)
        {

            string connectionString = ConfigurationDataTypeEnum.CONNECTION_STRING.GetValue(); // Properties.Settings.Default.ConnectionString;

            return Invoke(cbm, vo, userData, trxFactory, connectionString);

        }


        /// <summary>
        /// executing transaction 
        /// </summary>
        /// <param name="cbm"></param>
        /// <param name="vo"></param>
        /// <returns></returns>
        internal static ValueObject Invoke(CbmController cbm, ValueObject vo, UserData userData, TransactionContextFactory trxFactory, string connectionString)
        {

            if (cbm == null)
            {

                MessageData messageData = new MessageData("ffce00001", Properties.Resources.ffce00001, System.Reflection.MethodBase.GetCurrentMethod().Name);
                logger.Error(messageData, new NullReferenceException());
                throw new SystemException(messageData, new NullReferenceException());

            }

            if (trxFactory == null)
            {
                //please write code here
                defaultTrxFactory.GetTransactionContext(userData);
                trxFactory = defaultTrxFactory;
            }


            IDbTransaction dbTransaction = null;
            IDbConnection connection = null;
            //Get TransactionContext
            TransactionContext trxContext = trxFactory.GetTransactionContext(userData);

            try
            {

                connection = new NpgsqlConnection(connectionString);

                trxContext.DbConnection = connection;
                trxContext.DbConnection.Open();

                dbTransaction = trxContext.DbConnection.BeginTransaction();

                //Get DB Processing Time
                CbmController GetDBProcessingTimeCbm = trxFactory.GetDBProcessingTimeCbm();

                TimeVo time = (TimeVo)GetDBProcessingTimeCbm.Execute(trxContext, vo);

                trxContext.ProcessingDBDateTime = time.CurrentDateTime;

                //Start transaction
                ValueObject returnedVo = cbm.Execute(trxContext, vo);



                //commit
                dbTransaction.Commit();

                return returnedVo;

            }
            catch (ApplicationException appEx)
            {


                //rollback
                if (dbTransaction != null) { dbTransaction.Rollback(); }

                MessageData messageData = new MessageData("ffce00003", Properties.Resources.ffce00003, appEx.Message);
                logger.Error(messageData, appEx);
                throw appEx;

            }
            catch (SystemException sysEx)
            {
                //rollback
                if (dbTransaction != null) { dbTransaction.Rollback(); }

                MessageData messageData = new MessageData("ffce00003", Properties.Resources.ffce00003, sysEx.Message);
                logger.Error(messageData, sysEx);

                throw sysEx;
            }
            catch (Exception Ex)
            {

                //rollback
                if (dbTransaction != null) { dbTransaction.Rollback(); }

                MessageData messageData = new MessageData("ffce00003", Properties.Resources.ffce00003, Ex.Message);
                logger.Error(messageData, Ex);

                throw new SystemException(messageData);
            }
            finally
            {

                try
                {
                    if (connection != null) connection.Close();
                }
                catch (Exception ex)
                {
                    //need to be implemented
                    MessageData messageData = new MessageData("ffce00003", Properties.Resources.ffce00003, ex.Message);
                    logger.Error(messageData, ex);

                    throw new SystemException(messageData);
                }
            }
        }
    }
}
