using System;
using System.Text;
using System.Data;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Dao
{
    public class GetLineMasterMntDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {
            LineVo inVo = (LineVo)arg;

            LineVo outVo = new LineVo();

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("Select * from m_line ");
            sqlQuery.Append(" where 1 = 1 ");

            if (inVo.LineCode != null)
            {
                sqlQuery.Append(" and line_cd like :linecd ");
            }

            if (inVo.LineName != null)
            {
                sqlQuery.Append(" and line_name like :linename ");
            }

            sqlQuery.Append(" order by line_name");

            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();

            if (inVo.LineCode != null)
            {
                sqlParameter.AddParameterString("linecd", inVo.LineCode + "%");
            }

            if (inVo.LineName != null)
            {
                sqlParameter.AddParameterString("linename", inVo.LineName + "%");
            }

            //execute SQL
            IDataReader dataReader = sqlCommandAdapter.ExecuteReader(trxContext, sqlParameter);

            while (dataReader.Read())
            {
                LineVo currOutVo = new LineVo
                {
                    LineId = Convert.ToInt32(dataReader["line_id"]),
                    LineCode = dataReader["line_cd"].ToString(),
                    LineName = dataReader["line_name"].ToString(),
                };

                outVo.LineListVo.Add(currOutVo);
            }

            dataReader.Close();

            return outVo;
        }
    }
}
