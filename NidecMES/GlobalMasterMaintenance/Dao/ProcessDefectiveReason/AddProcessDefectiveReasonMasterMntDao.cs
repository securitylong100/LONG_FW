﻿using System.Text;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Dao
{
    public class AddProcessDefectiveReasonMasterMntDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {
            ProcessDefectiveReasonVo inVo = (ProcessDefectiveReasonVo)arg;

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("Insert into m_process_work_defective_reason");
            sqlQuery.Append(" ( ");
            sqlQuery.Append(" defective_reason_id, ");
            sqlQuery.Append(" process_work_id, ");
            sqlQuery.Append(" registration_user_cd, ");
            sqlQuery.Append(" registration_date_time, ");
            sqlQuery.Append(" factory_cd ");
            sqlQuery.Append(" ) ");
            sqlQuery.Append("VALUES	");
            sqlQuery.Append(" ( ");
            sqlQuery.Append(" :defrid ,");
            sqlQuery.Append(" :prcwid ,");
            sqlQuery.Append(" :regusrid ,");
            sqlQuery.Append(" now() ,");
            sqlQuery.Append(" :factorycode ");
            sqlQuery.Append(" ); ");

            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();
            sqlParameter.AddParameterInteger("defrid", inVo.DefectiveReasonId);
            sqlParameter.AddParameterInteger("prcwid", inVo.ProcessWorkId);
            sqlParameter.AddParameterString("regusrid", inVo.RegistrationUserCode);
            //sqlParameter.AddParameterDateTime("regdt", inVo.RegistrationDateTime);
            sqlParameter.AddParameterString("factorycode", inVo.FactoryCode);

            ProcessDefectiveReasonVo outVo = new ProcessDefectiveReasonVo
            {
                AffectedCount = sqlCommandAdapter.ExecuteNonQuery(sqlParameter)
            };
            return outVo;
        }
    }
}
