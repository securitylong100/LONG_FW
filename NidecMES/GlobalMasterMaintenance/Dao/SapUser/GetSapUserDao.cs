using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;
using System.Data;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GTRS_MasterMaintenance.Dao
{
    class GetSapUserDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {
            SapUserVo inVo = (SapUserVo)arg;

            StringBuilder sqlQuery = new StringBuilder();

            //create SQL
            sqlQuery.Append(" select ");
            sqlQuery.Append(" mes_user_cd,");
            sqlQuery.Append(" sap_user,");
            sqlQuery.Append(" sap_password,");            
            sqlQuery.Append(" registration_user_cd,");
            sqlQuery.Append(" registration_date_time,");
            sqlQuery.Append(" factory_cd");
            sqlQuery.Append(" from m_sap_user");
            sqlQuery.Append(" where 1 = 1 ");

            if (inVo.SapUserId > 0)
            {
                sqlQuery.Append(" and sap_user_id = :sapuserid;");
            }
            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();

            if (inVo.SapUserId > 0)
            {
                sqlParameter.AddParameterInteger("sapuserid", inVo.SapUserId);
            }
            //execute SQL
            IDataReader dataReader = sqlCommandAdapter.ExecuteReader(trxContext, sqlParameter);

            SapUserVo outVo = new SapUserVo();
            
            while (dataReader.Read())
            {
                SapUserVo currVo = new SapUserVo();
                currVo.SapUserId = ConvertDBNull<int>(dataReader, "sapuserid");

                outVo.SapUserListVo.Add(currVo);
            }

            dataReader.Close();

            return outVo;
        }
    }
}
