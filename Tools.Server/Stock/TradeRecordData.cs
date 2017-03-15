using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock {
    public class TradeRecordData : BaseData {

        private class SP {
            public const string AddTradeRecord = "AddTradeRecord";

        }

        private class SPParameter {
            public const string TradeDate = "TradeDate";
            public const string DueMonth = "DueMonth";
            public const string OP = "OP";
            public const string Contract = "Contract";
            public const string Type = "Type";
            public const string Lot = "Lot";
            public const string Price = "Price";
            public const string IsPyeongchang = "IsPyeongchang";
            public const string IsMail = "IsMail";
        }

        /// <summary>新增操作紀錄</summary>
        /// <param name="_TradeRecord"></param>
        /// <returns></returns>
        public int AddTradeRecord(TradeRecord _TradeRecord) {
            USP.AddParameter(SPParameter.TradeDate, _TradeRecord.TradeDate);
            USP.AddParameter(SPParameter.DueMonth, _TradeRecord.DueMonth);
            USP.AddParameter(SPParameter.OP, _TradeRecord.OP);
            USP.AddParameter(SPParameter.Contract, _TradeRecord.Contract);
            USP.AddParameter(SPParameter.Type, _TradeRecord.Type);
            USP.AddParameter(SPParameter.Lot, _TradeRecord.Lot);
            USP.AddParameter(SPParameter.Price, _TradeRecord.Price); 
            TradeRecord Result= USP.ExeProcedureGetObject(SP.AddTradeRecord,new TradeRecord());
            return Result.SN;
        }

    }
}
