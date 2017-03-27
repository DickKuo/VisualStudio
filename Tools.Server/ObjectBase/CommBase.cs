
namespace ObjectBase
{
    public class CommBase
    {
        protected SQLHelper.UseStoreProcedure USP = new SQLHelper.UseStoreProcedure();
        public const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        public const string SN = "SN";
        public const string Remark = "Remark";
        public const string OutResult = "OutResult";
        public const string SQLSuccessMessage = "Store Procedure Execute Sucess";
    }
    public enum SQLExecResultCode {
        Success = 99,
        Fail = -1
    }

    public enum GenderType {
        Male = 1,
        Fmale = 2
    }
}
