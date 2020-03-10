using System.Collections.Generic;
using System.Data;
using System;

namespace Stock
{
    public class OptionDAO : BaseData
    {
        private class SP
        {
            public const string StoreToHistory = "StoreToHistory";
            public const string GetHistoryOption = "GetHistoryOption";
            public const string GetDueMonthHistory = "GetDueMonthHistory";
        }

        private class SPParameter
        {
            public const string OP = "OP";
            public const string Contract = "Contract";
            public const string DueMonth = "DueMonth";
            public const string BeginDate = "BeginDate";
        }

        /// <summary>儲存歷史資料</summary>
        public int StoreToHistory()
        {
            return USP.ExeProcedureHasResultReturnCode(SP.StoreToHistory);
        }

        /// <summary></summary>
        /// <param name="_Week"></param>
        /// <returns></returns>
        public List<Option> GetHistoryOption(WeekPoint _Week)
        {
            List<Option> Result = new List<Option>();
            USP.AddParameter(SPParameter.OP, _Week.OP);
            USP.AddParameter(SPParameter.Contract, _Week.Contract);
            USP.AddParameter(SPParameter.DueMonth, _Week.DueMonth);
            USP.AddParameter(SPParameter.BeginDate, _Week.TradeDate.ToString("yyyy/MM/dd"));
            Result = USP.ExeProcedureGetObjectList(SP.GetHistoryOption, new Option());
            return Result;
        }

        public DataTable GetDueMonthHistory()
        {
            try
            {
                return USP.ExeProcedureGetDataTable(SP.GetDueMonthHistory);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}