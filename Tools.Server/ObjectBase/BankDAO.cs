﻿using System.Collections.Generic;
using System;

namespace ObjectBase
{
    public class BankDAO : CommBase
    {
        private class SP
        {
            public const string AddBank = "AddBank";
            public const string UpdateBankBySN = "UpdateBankBySN";
            public const string GetBankByCustomerSN = "GetBankByCustomerSN";
            public const string GetBankBySN = "GetBankBySN";
        }

        private class SSParamter
        {
            public const string CustomerSN = "CustomerSN";
            public const string BankCode = "BankCode";
            public const string BankAccount = "BankAccount";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string Seq = "Seq";
            public const string BankName = "BankName";
        }

        /// <summary>新增Bank</summary>
        /// <param name="_Bank"></param>
        /// <returns></returns>
        public int AddBank(Bank _Bank)
        {
            try
            {
                USP.AddParameter(SSParamter.CustomerSN, _Bank.CustomerSN);
                USP.AddParameter(SSParamter.BankName, _Bank.BankName);
                USP.AddParameter(SSParamter.BankCode, _Bank.BankCode);
                USP.AddParameter(SSParamter.BankAccount, _Bank.BankAccount);
                USP.AddParameter(SSParamter.FirstName, _Bank.FirstName);
                USP.AddParameter(SSParamter.LastName, _Bank.LastName);
                USP.AddParameter(SSParamter.Seq, _Bank.Seq);
                USP.AddParameter(CommBase.Remark, _Bank.Remark == null ? string.Empty : _Bank.Remark);
                return USP.ExeProcedureHasResultReturnCode(SP.AddBank);
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
                return -1;
            }
        }

        /// <summary>更新Bank資料</summary>
        /// <param name="_Bank"></param>
        /// <returns></returns>
        public int UpdateBankBySN(Bank _Bank)
        {
            try
            {
                USP.AddParameter(CommBase.SN, _Bank.SN);
                USP.AddParameter(SSParamter.BankName, _Bank.BankName);
                USP.AddParameter(SSParamter.BankCode, _Bank.BankCode);
                USP.AddParameter(SSParamter.BankAccount, _Bank.BankAccount);
                USP.AddParameter(SSParamter.FirstName, _Bank.FirstName);
                USP.AddParameter(SSParamter.LastName, _Bank.LastName);
                USP.AddParameter(SSParamter.Seq, _Bank.Seq);
                USP.AddParameter(CommBase.Remark, _Bank.Remark == null ? string.Empty : _Bank.Remark);
                USP.AddParameter(CommBase.SParamter_IsEnable, _Bank.IsEnable);
                return USP.ExeProcedureHasResultReturnCode(SP.UpdateBankBySN);
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
                return -1;
            }
        }

        /// <summary>取得客戶銀行帳號</summary>
        /// <param name="CustomerSN"></param>
        /// <returns></returns>
        public List<Bank> GetBankByCustomerSN(int CustomerSN)
        {
            try
            {
                USP.AddParameter(SSParamter.CustomerSN, CustomerSN);
                List<Bank> BankList = USP.ExeProcedureGetObjectList(SP.GetBankByCustomerSN, new Bank());
                return BankList;
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
                return null;
            }
        }

        public Bank GetBankBySN(int SN)
        {
            try
            {
                USP.AddParameter(CommBase.SN, SN);
                Bank Result = USP.ExeProcedureGetObject(SP.GetBankBySN, new Bank());
                return Result;
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
                return null;
            }
        }
    }
}