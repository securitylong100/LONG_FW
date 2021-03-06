﻿using System.Text;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Dao
{
    public class UpdateLocationMasterMntDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {
            LocationVo inVo = (LocationVo)arg;

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("Update m_location");
            sqlQuery.Append(" Set ");
            sqlQuery.Append(" location_name = :locationname, ");
            sqlQuery.Append(" building_id = :buildingid ");
            sqlQuery.Append(" Where	");
            sqlQuery.Append(" location_id = :locationid ;");

            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();
            sqlParameter.AddParameterInteger("locationid", inVo.LocationId);
            sqlParameter.AddParameterString("locationname", inVo.LocationName);
            sqlParameter.AddParameterInteger("buildingid", inVo.BuildingId);

            LocationVo outVo = new LocationVo {AffectedCount = sqlCommandAdapter.ExecuteNonQuery(sqlParameter)};

            return outVo;
        }

  


    }
}
