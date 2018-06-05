using System.Text;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Dao
{
    public class AddDefectiveReasonMasterMntDao : AbstractDataAccessObject
    {
        public override ValueObject Execute(TransactionContext trxContext, ValueObject arg)
        {
            DefectiveReasonVo inVo = (DefectiveReasonVo)arg;

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("Insert into m_defective_reason");
            sqlQuery.Append(" ( ");
            sqlQuery.Append(" defective_reason_cd, ");
            sqlQuery.Append(" defective_reason_name, ");
            sqlQuery.Append(" defective_category_id, ");
            sqlQuery.Append(" display_order, ");
            sqlQuery.Append(" registration_user_cd, ");
            sqlQuery.Append(" registration_date_time, ");
            sqlQuery.Append(" factory_cd ");
            sqlQuery.Append(" ) ");
            sqlQuery.Append("VALUES	");
            sqlQuery.Append(" ( ");
            sqlQuery.Append(" :defectivereasoncd ,");
            sqlQuery.Append(" :defectivereasonname ,");
            sqlQuery.Append(" :defectivecategoryid ,");
            sqlQuery.Append(" :displayorder,");
            sqlQuery.Append(" :registrationusercode ,");
            sqlQuery.Append(" now() ,");
            sqlQuery.Append(" :factorycode ");
            sqlQuery.Append(" ); ");

            //create command
            DbCommandAdaptor sqlCommandAdapter = base.GetDbCommandAdaptor(trxContext, sqlQuery.ToString());

            //create parameter
            DbParameterList sqlParameter = sqlCommandAdapter.CreateParameterList();
            sqlParameter.AddParameterString("defectivereasoncd", inVo.DefectiveReasonCode);
            sqlParameter.AddParameterString("defectivereasonname", inVo.DefectiveReasonName);
            sqlParameter.AddParameterInteger("defectivecategoryid", inVo.DefectiveCategoryId);
            sqlParameter.AddParameterInteger("displayorder", inVo.DisplayOrder);
            sqlParameter.AddParameterString("registrationusercode", inVo.RegistrationUserCode);
            sqlParameter.AddParameterString("factorycode", inVo.FactoryCode);

            DefectiveReasonVo outVo = new DefectiveReasonVo
            {
                AffectedCount = sqlCommandAdapter.ExecuteNonQuery(sqlParameter)
            };

            return outVo;
        }
    }
}
