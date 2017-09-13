using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ObjectBase {
    public class TranscationDAO : CommBase {

        public class SP {
            public const string AddTranscation = "AddTranscation";
            public const string GetTop10TransactionByCustomerSN = "GetTop10TransactionByCustomerSN";
            public const string GetDetailByTransactionSN = "GetDetailByTransactionSN";
            public const string GetTranscationPagesByDueDay = "GetTranscationPagesByDueDay";
            public const string GetTransactionsNotAuditByTradeType = "GetTransactionsNotAuditByTradeType";
            public const string GetTranscationBySN = "GetTranscationBySN";
            public const string AuditTranscation = "AuditTranscation";
            public const string GetTransactionByCustomerSNPages = "GetTransactionByCustomerSNPages";
        }

        public class SParameter {
            public const string SN = "SN";
            public const string CustomerSN = "CustomerSN";
            public const string TradeType = "TradeType";
            public const string Draw = "Draw";
            public const string TradeTime = "TradeTime";
            public const string AuditState = "AuditState";
            public const string AuditCustomerSN = "AuditCustomerSN";
            public const string AuditTime = "AuditTime";
            public const string TransactionSN = "TransactionSN";
            public const string BankName = "BankName";
            public const string BankAccount = "BankAccount";
            public const string BranchName = "BranchName";
            public const string Remark = "Remark";
            public const string BeginTime = "BeginTime";
            public const string EndTime = "EndTime";
            public const string Range = "Range";
            public const string Page = "Page";
            public const string AuditAdviserSN = "AuditAdviserSN";
        }

        /// <summary>新增交易單</summary>
        /// <param name="_Transaction"></param>
        /// <returns></returns>
        public int AddTranscation(Transaction _Transaction) {            
            try {
                USP.AddParameter(SParameter.CustomerSN, _Transaction.CustomerSN);
                USP.AddParameter(SParameter.TradeType, _Transaction.TradeType);
                USP.AddParameter(SParameter.BankName, _Transaction.Detail.BankName);
                USP.AddParameter(SParameter.BankAccount, _Transaction.Detail.BankAccount);
                USP.AddParameter(SParameter.BranchName, _Transaction.Detail.BranchName);
                USP.AddParameter(SParameter.Draw, _Transaction.Detail.Draw);
                USP.AddParameter(SParameter.Remark, _Transaction.Detail.Remark); 
                Transaction _Result = USP.ExeProcedureGetObject(SP.AddTranscation, new Transaction()) as Transaction;
                return _Result.SN;
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return 0;
            }
        }

        /// <summary>取得交易單</summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public Transaction GetTranscationBySN(int SN) {
            try {
                USP.AddParameter(SParameter.TransactionSN, SN);
                Transaction _Result = USP.ExeProcedureGetObject(SP.GetTranscationBySN, new Transaction()) as Transaction;
                _Result.Detail = GetDetailByTransactionSN(SN);
                return _Result;
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return new Transaction();
            }
        }

        /// <summary>依照交易單取得明細</summary>
        /// <param name="TransactionSN"></param>
        /// <returns></returns>
        public TransactionDetail GetDetailByTransactionSN(int TransactionSN) {          
            try {
                USP.AddParameter(SParameter.TransactionSN, TransactionSN);
                TransactionDetail _Result = USP.ExeProcedureGetObject(SP.GetDetailByTransactionSN, new TransactionDetail()) as TransactionDetail;
                return _Result;
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return new TransactionDetail();
            }
        }

        /// <summary>取得時間區間的交易資料</summary>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="CustomerSN"></param>
        /// <returns></returns>
        public List<Transaction> GetTop10TransactionByCustomerSN(string BeginTime, string EndTime, int CustomerSN) {           
            try {
                USP.AddParameter(SParameter.CustomerSN, CustomerSN);
                USP.AddParameter(SParameter.BeginTime, BeginTime);
                USP.AddParameter(SParameter.EndTime, EndTime);
                List<Transaction> ListTransaction = USP.ExeProcedureGetObjectList(SP.GetTop10TransactionByCustomerSN, new Transaction()) as List<Transaction>;
                TranscationDAO DAO =new TranscationDAO();
                foreach (Transaction Tran in ListTransaction)
                {
                    TransactionDetail Detail = DAO.GetDetailByTransactionSN(Tran.SN);
                    Tran.Detail = Detail;
                }
                return ListTransaction;
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return new List<Transaction>();
            } 
        }


        /// <summary>取得客戶指定頁的交易單</summary>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="CustomerSN"></param>
        /// <param name="Page"></param>
        /// <param name="Range"></param>
        /// <returns></returns>
        public List<Transaction> GetTransactionByCustomerSNPages(string BeginTime, string EndTime, int CustomerSN, int Page, int Range) {
            try {
                USP.AddParameter(SParameter.CustomerSN, CustomerSN);
                USP.AddParameter(SParameter.BeginTime, BeginTime);
                USP.AddParameter(SParameter.EndTime, EndTime);
                USP.AddParameter(SParameter.Page, Page);
                USP.AddParameter(SParameter.Range, Range);
                List<Transaction> ListTransaction = USP.ExeProcedureGetObjectList(SP.GetTransactionByCustomerSNPages, new Transaction()) as List<Transaction>;
                TranscationDAO DAO = new TranscationDAO();
                foreach (Transaction Tran in ListTransaction) {
                    TransactionDetail Detail = DAO.GetDetailByTransactionSN(Tran.SN);
                    Tran.Detail = Detail;
                }
                return ListTransaction;
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return new List<Transaction>();
            } 
        }



        /// <summary>取得指定交易類別的交易單</summary>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Range"></param>
        /// <returns></returns>
        public List<TransInfo> GetTransactionsNotAuditByTradeType(string BeginTime, string EndTime, int Range, int Page, TranscationTypes TradeType) {
            try {
                USP.AddParameter(SParameter.BeginTime, BeginTime);
                USP.AddParameter(SParameter.EndTime, EndTime);
                USP.AddParameter(SParameter.Range, Range);
                USP.AddParameter(SParameter.TradeType, TradeType);
                USP.AddParameter(SParameter.Page, Page);
                List<TransInfo> ListTransaction = USP.ExeProcedureGetObjectList(SP.GetTransactionsNotAuditByTradeType, new TransInfo()) as List<TransInfo>;
                return ListTransaction;
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return new List<TransInfo>();
            } 
        }

        /// <summary>取得交易單</summary>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Range"></param>
        /// <returns></returns>
        public List<Transaction> GetTransactions(string BeginTime, string EndTime, int Range, int Page) {
            try {
                USP.AddParameter(SParameter.BeginTime, BeginTime);
                USP.AddParameter(SParameter.EndTime, EndTime);
                USP.AddParameter(SParameter.Range, Range);
                USP.AddParameter(SParameter.Page, Page);
                List<Transaction> ListTransaction = USP.ExeProcedureGetObjectList(SP.GetTop10TransactionByCustomerSN, new Transaction()) as List<Transaction>;
                TranscationDAO DAO = new TranscationDAO();
                foreach (Transaction Tran in ListTransaction) {
                    TransactionDetail Detail = DAO.GetDetailByTransactionSN(Tran.SN);
                    Tran.Detail = Detail;
                }
                return ListTransaction;
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return new List<Transaction>();
            }
        }

        /// <summary>取得時間區間的交易資料頁數</summary>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="CustomerSN"></param>
        /// <returns></returns>
        public int GetTranscationPagesByDueDay(string BeginTime, string EndTime, int CustomerSN,int Range) {
            try {
                USP.AddParameter(SParameter.CustomerSN, CustomerSN);
                USP.AddParameter(SParameter.BeginTime, BeginTime);
                USP.AddParameter(SParameter.EndTime, EndTime);
                USP.AddParameter(SParameter.Range, Range);
                DataTable dt =  USP.ExeProcedureGetDataTable(SP.GetTranscationPagesByDueDay);
                int Result = 0;
                if (dt != null && dt.Rows.Count > 0) {
                    Result = Convert.ToInt32(dt.Rows[0][0]);
                }
                return Result;
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return 0;
            }
        }

        /// <summary>審核交易單</summary>
        /// <param name="TransactionSN"></param>
        /// <param name="Audit"></param>
        /// <param name="AuditAdviserSN"></param>
        /// <returns></returns>
        public int AuditTranscation(int TransactionSN, AuditTypes Audit, int AuditAdviserSN) {
            try {
                USP.AddParameter(SParameter.TransactionSN, TransactionSN);
                USP.AddParameter(SParameter.AuditState, Audit);
                USP.AddParameter(SParameter.AuditAdviserSN, AuditAdviserSN);
                DataTable dt = USP.ExeProcedureGetDataTable(SP.AuditTranscation);
                int Result = 0;
                if (dt != null && dt.Rows.Count > 0) {
                    Result = Convert.ToInt32(dt.Rows[0][0]);
                }
                return Result;
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return 0;
            }
        }

    }
}
