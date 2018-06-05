using System;
using System.Text;
using System.Data;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Dao
{
    public class CheckMoldTypeMasterMntDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {
            MoldTypeVo inVo = (MoldTypeVo)arg;

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("Select Count(*) as MoldTypeCount from m_mold_type");
            sqlQuery.Append(" where 1 = 1 ");

            if (inVo.MoldTypeCode != null)
            {
                sqlQuery.Append(" and UPPER(mold_type_cd) = UPPER(:moldtypecd)");
            }
            
            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();

            if (inVo.MoldTypeCode != null)
            {
                sqlParameter.AddParameterString("moldtypecd", inVo.MoldTypeCode);
            }

            //execute SQL
            IDataReader dataReader = sqlCommandAdapter.ExecuteReader(trxContext, sqlParameter);

            MoldTypeVo outVo = new MoldTypeVo();

            while (dataReader.Read())
            {
               outVo.AffectedCount = Convert.ToInt32(dataReader["MoldTypeCount"]);
            }

            dataReader.Close();

            return outVo;
        }
    }
}
