using System;
using System.Text;
using System.Data;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Dao
{
    public class GetEquipmentMasterMntDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {
            EquipmentVo inVo = (EquipmentVo)arg;

            EquipmentVo outVo = new EquipmentVo();
          
            StringBuilder sqlQuery = new StringBuilder();

            //create SQL
            sqlQuery.Append("Select * from m_equipment ");
            sqlQuery.Append(" where 1 = 1 ");

            if (inVo.EquipmentCode != null)
            {
                sqlQuery.Append(" and equipment_cd like :equipmentcode ");
            }

            if (inVo.EquipmentName != null)
            {
                sqlQuery.Append(" and equipment_name  like :equipmentname ");
            }

            if (inVo.InstrationDate != null && inVo.InstrationDate != DateTime.MinValue)
            {
                sqlQuery.Append(" and instration_dt = :instrationdt ");
            }

            if (inVo.AssetNo != null)
            {
                sqlQuery.Append(" and asset_no  like :assetno ");
            }

            if (inVo.Manufacturer != null)
            {
                sqlQuery.Append(" and manufacturer  like :manufacturer ");
            }

            if (inVo.ModelCode != null)
            {
                sqlQuery.Append(" and model_cd  like :modelcd ");
            }

            if (inVo.ModelName != null)
            {
                sqlQuery.Append(" and model_name  like :modelname ");
            }

            sqlQuery.Append(" order by equipment_cd ");

            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();

           
            if (inVo.EquipmentCode != null)
            {
                sqlParameter.AddParameterString("equipmentcode", inVo.EquipmentCode + "%");
            }

            if (inVo.EquipmentName != null)
            {
                sqlParameter.AddParameterString("equipmentname", inVo.EquipmentName + "%");
            }

            if (inVo.InstrationDate != null)
            {
                sqlParameter.AddParameterDateTime("instrationdt", inVo.InstrationDate);
            }

            if (inVo.AssetNo != null)
            {
                sqlParameter.AddParameterString("assetno", inVo.AssetNo + "%");
            }

            if (inVo.Manufacturer != null)
            {
                sqlParameter.AddParameterString("manufacturer", inVo.Manufacturer + "%");
            }

            if (inVo.ModelCode != null)
            {
                sqlParameter.AddParameterString("modelcd", inVo.ModelCode + "%");
            }

            if (inVo.ModelName != null)
            {
                sqlParameter.AddParameterString("modelname", inVo.ModelName + "%");
            }
            //execute SQL
            IDataReader dataReader = sqlCommandAdapter.ExecuteReader(trxContext, sqlParameter);

            while (dataReader.Read())

            {
                EquipmentVo currOutVo = new EquipmentVo
                {
                    EquipmentId = Convert.ToInt32(dataReader["equipment_id"]),
                    EquipmentCode = dataReader["equipment_cd"].ToString(),
                    EquipmentName = dataReader["equipment_name"].ToString(),
                    InstrationDate = Convert.ToDateTime(dataReader["instration_dt"]), //.ToString("yyyy-MM-dd"),
                    AssetNo = dataReader["asset_no"].ToString(),
                    Manufacturer = dataReader["manufacturer"].ToString(),
                    ModelName = dataReader["model_name"].ToString(),
                    ModelCode = dataReader["model_cd"].ToString()
                };

                outVo.EquipmentListVo.Add(currOutVo);
            }

            dataReader.Close();

            return outVo;
        }
    }
}
