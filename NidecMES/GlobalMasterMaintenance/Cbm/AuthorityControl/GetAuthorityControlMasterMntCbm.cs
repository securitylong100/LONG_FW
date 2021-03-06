﻿using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Dao;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance.Cbm
{
    public class GetAuthorityControlMasterMntCbm : CbmController
    {
        private readonly DataAccessObject getAuthorityControlDao = new GetAuthorityControlMasterMntDao();

        public ValueObject Execute(TransactionContext trxContext, ValueObject vo)
        {

            return getAuthorityControlDao.Execute(trxContext, vo);

        }
    }
}
