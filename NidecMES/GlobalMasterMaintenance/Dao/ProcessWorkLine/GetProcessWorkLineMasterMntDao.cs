using System;
using System.Text;
using System.Data;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Dao
{
    public class GetProcessWorkLineMasterMntDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {
            ProcessWorkLineVo inVo = (ProcessWorkLineVo)arg;

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("Select pwm.process_work_id, pwm.line_id,ls.line_name");
            sqlQuery.Append(" from m_processwork_line pwm ");
            sqlQuery.Append(" inner join m_line ls on ls.line_id = pwm.line_id ");
            sqlQuery.Append(" where 1 = 1 ");

            if (inVo.ProcessWorkId > 0)
            {
                sqlQuery.Append(" and pwm.process_work_id = :processworkid ");
            }
            if (inVo.LineName != null)
            {
                sqlQuery.Append(" and line_name like :linename ");
            }

            sqlQuery.Append(" order by line_name ");

            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();

            if (inVo.ProcessWorkId > 0)
            {
                sqlParameter.AddParameterInteger("processworkid", inVo.ProcessWorkId);
            }
            if (inVo.LineName != null)
            {
                sqlParameter.AddParameterString("linename", inVo.LineName + "%");
            }

            //execute SQL
            IDataReader dataReader = sqlCommandAdapter.ExecuteReader(trxContext, sqlParameter);

            ProcessWorkLineVo outVo = new ProcessWorkLineVo();

            while (dataReader.Read())
            {
                ProcessWorkLineVo currOutVo = new ProcessWorkLineVo
                {
                    LineId = Convert.ToInt32(dataReader["line_id"]),
                    LineName = dataReader["line_name"].ToString(),
                };

                outVo.ProcessWorkLineListVo.Add(currOutVo);

            }
            dataReader.Close();

            return outVo;
        } 
    }
}
