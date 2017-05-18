
namespace ObjectBase
{
    public class CommBase
    {
        protected SQLHelper.UseStoreProcedure USP = new SQLHelper.UseStoreProcedure();
        protected const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        protected const string SN = "SN";
        protected const string Remark = "Remark";
        protected const string OutResult = "OutResult";
        protected const string SQLSuccessMessage = "Store Procedure Execute Sucess";
        protected const string CustomerSN = "CustomerSN";
    }
    public enum SQLExecResultCode {
        Success = 99,
        Fail = -1
    }

    public enum GenderType {
        Male = 1,
        Fmale = 2
    }

    public enum TradeType { 
       Deposit=1,
       Withdrawal=2
    }

}
