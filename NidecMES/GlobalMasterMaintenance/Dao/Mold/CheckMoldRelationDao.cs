using System;
using System.Text;
using System.Data;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Dao
{
    public class CheckMoldRelationDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {
            MoldVo inVo = (MoldVo)arg;

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("select count(cv.cavity_cd) as CavityCount from m_mold mo");
            sqlQuery.Append(" inner join m_cavity cv on cv.mold_id = mo.mold_id");
            sqlQuery.Append(" where 1 = 1 ");

            if (inVo.MoldCode != null)
            {
                sqlQuery.Append(" and UPPER(mold_cd) = UPPER(:moldcd) ");
            }
                        
            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();

            if (inVo.MoldCode != null)
            {
                sqlParameter.AddParameterString("moldcd", inVo.MoldCode);
            }

            //execute SQL
            IDataReader dataReader = sqlCommandAdapter.ExecuteReader(trxContext, sqlParameter);

            MoldVo outVo = new MoldVo();

            while (dataReader.Read())
            {
              outVo.AffectedCount = Convert.ToInt32("0" + dataReader["CavityCount"]);
            }
            dataReader.Close();

            return outVo;
        }
    }
}
