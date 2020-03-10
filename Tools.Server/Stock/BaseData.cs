namespace Stock
{
    public class BaseData
    {
        protected SQLHelper.UseStoreProcedure USP = new SQLHelper.UseStoreProcedure();

        protected class BaseSParameter
        {
            public const string BeginDate = "BeginDate";
            public const string EndDate = "EndDate";
            public const string SN = "SN";
            public const string DueMonth = "DueMonth";
            public const string DataTimeFormat = "yyyy-MM-dd";
            public const string DateTimeFormat2 = "yyyy-MM-dd HH:mm:ss";
            public const string Htmlnbsp = "&nbsp;";
        }
    }
}