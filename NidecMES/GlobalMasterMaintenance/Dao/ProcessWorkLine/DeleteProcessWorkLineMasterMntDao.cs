﻿using System.Text;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Dao
{
    public class DeleteProcessWorkLineMasterMntDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {
            ProcessWorkLineVo inVo = (ProcessWorkLineVo)arg;

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("Delete from m_processwork_line");
            sqlQuery.Append(" where ");
            sqlQuery.Append(" process_work_id = :processworkid");           

            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();
            sqlParameter.AddParameterInteger("processworkid", inVo.ProcessWorkId);

            ProcessWorkLineVo outVo = new ProcessWorkLineVo { AffectedCount = sqlCommandAdapter.ExecuteNonQuery(sqlParameter)};

            return outVo;
        }
    }
}
