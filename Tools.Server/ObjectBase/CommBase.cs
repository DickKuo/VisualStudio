﻿namespace ObjectBase
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
        protected const string SParamter_IsEnable = "IsEnable";
        protected const string SParamter_Seq = "Seq";
        protected const string SParamter_BeginDate = "BeginDate";
        protected const string SParamter_EndDate = "EndDate";
        protected const string SParamter_Page = "Page";
        protected const string SParamter_PageCount = "PageCount";
        protected const string SParamter_PageSize = "PageSize";
    }
    public enum SQLExecResultCode
    {
        Success = 99,
        Fail = -1
    }

    public enum GenderType
    {
        Male = 1,
        Fmale = 2
    }

    public enum TradeType
    {
        Deposit = 1,
        Withdrawal = 2
    }
}