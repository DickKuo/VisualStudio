
using System;
using System.Collections.Generic;
namespace ObjectBase {
    public class BankDAO :CommBase{

        private class SP {
            public const string AddBank = "AddBank";
            public const string UpdateBankBySN = "UpdateBankBySN";
            public const string GetBankByCustomerSN = "GetBankByCustomerSN";
        }

        private class SSParamter {
            public const string CustomerSN = "CustomerSN";
            public const string BankCode = "BankCode";
            public const string BankAccount = "BankAccount";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string Seq = "Seq";
        }

        /// <summary>新增Bank</summary>
        /// <param name="_Bank"></param>
        /// <returns></returns>
        public int AddBank(Bank _Bank) {
            try {
                USP.AddParameter(SSParamter.CustomerSN, _Bank.CustomerSN);
                USP.AddParameter(SSParamter.BankCode, _Bank.BankCode);
                USP.AddParameter(SSParamter.BankAccount, _Bank.BankAccount);
                USP.AddParameter(SSParamter.FirstName, _Bank.FirstName);
                USP.AddParameter(SSParamter.LastName, _Bank.LastName);
                USP.AddParameter(SSParamter.Seq, _Bank.Seq);
                USP.AddParameter(CommBase.Remark, _Bank.Remark == null ? string.Empty : _Bank.Remark);
                return USP.ExeProcedureHasResultReturnCode(SP.AddBank);                         
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return -1;
            }
        }

        /// <summary>更新Bank資料</summary>
        /// <param name="_Bank"></param>
        /// <returns></returns>
        public int UpdateBankBySN(Bank _Bank) {
            try {
                USP.AddParameter(CommBase.SN,_Bank.SN); 
                USP.AddParameter(SSParamter.BankCode, _Bank.BankCode);
                USP.AddParameter(SSParamter.BankAccount, _Bank.BankAccount);
                USP.AddParameter(SSParamter.FirstName, _Bank.FirstName);
                USP.AddParameter(SSParamter.LastName, _Bank.LastName);
                USP.AddParameter(SSParamter.Seq, _Bank.Seq);
                USP.AddParameter(CommBase.Remark, _Bank.Remark == null ? string.Empty : _Bank.Remark);
                string Result = USP.ExeProcedureHasResult(SP.UpdateBankBySN);
                if (Result == CommBase.SQLSuccessMessage) {
                    return 99;
                }
                else {
                    return -1;
                }      
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return -1;
            }
        }

        /// <summary>取得客戶銀行帳號</summary>
        /// <param name="CustomerSN"></param>
        /// <returns></returns>
        public List<Bank> GetBankByCustomerSN(int CustomerSN) {
            try {
                USP.AddParameter(SSParamter.CustomerSN, CustomerSN);
                List<Bank> BankList = USP.ExeProcedureGetObjectList(SP.GetBankByCustomerSN, new Bank());
                return BankList;
            }
            catch (Exception ex) {                
                CommTool.ToolLog.Log(ex);
                return null;
            }
        }

    }
}
