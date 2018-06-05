using System;
using System.Text;
using System.Data;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Dao
{
    public class GetMachineMasterMntDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {
            MachineVo inVo = (MachineVo)arg;

            MachineVo outVo = new MachineVo();

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("Select * from m_machine ");
            sqlQuery.Append(" where 1 = 1 ");

            if (inVo.MachineCode != null)
            {
                sqlQuery.Append(" and machine_cd like :machinecd ");
            }

            if (inVo.MachineName != null)
            {
                sqlQuery.Append(" and machine_name like :machinename ");
            }

            sqlQuery.Append(" order by machine_name");

            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();

            if (inVo.MachineCode != null)
            {
                sqlParameter.AddParameterString("machinecd", inVo.MachineCode + "%");
            }

            if (inVo.MachineName != null)
            {
                sqlParameter.AddParameterString("machinename", inVo.MachineName + "%");
            }


            //execute SQL
            IDataReader dataReader = sqlCommandAdapter.ExecuteReader(trxContext, sqlParameter);

            while (dataReader.Read())
            {
                MachineVo currOutVo = new MachineVo
                {
                    MachineId = Convert.ToInt32(dataReader["machine_id"]),
                    MachineCode = dataReader["machine_cd"].ToString(),
                    MachineName = dataReader["machine_name"].ToString(),
                };

                outVo.MachineListVo.Add(currOutVo);
            }

            dataReader.Close();

            return outVo;
        }
    }
}
