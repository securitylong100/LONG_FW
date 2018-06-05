using System;
using System.Text;
using System.Data;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Dao
{
    public class GetLocalSupplierCavityMasterMntDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {
            LocalSupplierCavityVo inVo = (LocalSupplierCavityVo)arg;

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("Select lsc.cavity_id, lsc.cavity_cd, lsc.cavity_name, ls.local_supplier_id, ls.local_supplier_cd, ls.local_supplier_name ");
            sqlQuery.Append("from m_local_supplier_cavity lsc ");
            sqlQuery.Append("inner join m_local_supplier ls on ls.local_supplier_id = lsc.local_supplier_id ");
            sqlQuery.Append(" where 1 = 1 ");

            if (inVo.LocalSupplierCavityCode != null)
            {
                sqlQuery.Append(" and lsc.cavity_cd like :cavitycd ");
            }

            if (inVo.LocalSupplierCavityName != null)
            {
                sqlQuery.Append(" and lsc.cavity_name like :cavityname ");
            }

            if (inVo.LocalSupplierId != 0)
            {
                sqlQuery.Append(" and ls.local_supplier_id = :localsupplierid ");
            }

            sqlQuery.Append(" order by lsc.cavity_cd");

            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();

            if (inVo.LocalSupplierCavityCode != null)
            {
                sqlParameter.AddParameterString("cavitycd", inVo.LocalSupplierCavityCode + "%");
            }

            if (inVo.LocalSupplierCavityName != null)
            {
                sqlParameter.AddParameterString("cavityname", inVo.LocalSupplierCavityName + "%");
            }
            if (inVo.LocalSupplierId != 0)
            {
                sqlParameter.AddParameterInteger("localsupplierid", inVo.LocalSupplierId);
            }

            //execute SQL
            IDataReader dataReader = sqlCommandAdapter.ExecuteReader(trxContext, sqlParameter);

            LocalSupplierCavityVo outVo = new LocalSupplierCavityVo();

            while (dataReader.Read())
            {
                LocalSupplierCavityVo currOutVo = new LocalSupplierCavityVo
                {
                    LocalSupplierCavityId = Convert.ToInt32(dataReader["cavity_id"]),
                    LocalSupplierCavityCode = dataReader["cavity_cd"].ToString(),
                    LocalSupplierCavityName = dataReader["cavity_name"].ToString(),
                    LocalSupplierId = Convert.ToInt32(dataReader["local_supplier_id"]),
                    LocalSupplierName = dataReader["local_supplier_name"].ToString()
                };
                outVo.LocalSupplierCavityListVo.Add(currOutVo);
            }

            dataReader.Close();

            return outVo;
        } 
    }
}
