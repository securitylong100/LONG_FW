using System;
using System.Text;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Dao
{
    class UpdateCastingMachineFurnaceMasterMntDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {

            CastingMachineFurnaceVo inVo = (CastingMachineFurnaceVo)arg;

            CastingMachineFurnaceVo outVo = new CastingMachineFurnaceVo();

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("Update m_casting_machine_furnace");
            sqlQuery.Append(" set ");
            sqlQuery.Append(" casting_machine_furnace_name = ");
            sqlQuery.Append(" :castfurname, ");
            sqlQuery.Append(" equipment_id = ");
            sqlQuery.Append(" :eqpid,");
            sqlQuery.Append(" factory_cd = ");
            sqlQuery.Append(" :factcode");
            sqlQuery.Append(" where");
            sqlQuery.Append(" casting_machine_furnace_id = ");
            sqlQuery.Append(" :castfurid");

            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();

            sqlParameter.AddParameterInteger("castfurid", inVo.CastingMachineFurnaceId);
            sqlParameter.AddParameterString("castfurname", inVo.CastingMachineFurnaceName);
            sqlParameter.AddParameterInteger("eqpid", inVo.EquipmentId);
            sqlParameter.AddParameterString("factcode", inVo.FactoryCode);

            outVo.AffectedCount = sqlCommandAdapter.ExecuteNonQuery(sqlParameter);

            return outVo;
        }
    }
}
