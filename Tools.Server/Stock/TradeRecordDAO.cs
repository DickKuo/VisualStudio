using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Stock {
    public class TradeRecordDAO : BaseData {

        private class SP {
            public const string AddTradeRecord = "AddTradeRecord";
            public const string UpdateTradeRecord = "UpdateTradeRecord";
            public const string GetTradeRecord = "GetTradeRecord";
        }

        private class SPParameter {
            public const string SN = "SN";
            public const string TradeDate = "TradeDate";
            public const string DueMonth = "DueMonth";
            public const string OP = "OP";
            public const string Contract = "Contract";
            public const string Type = "Type";
            public const string Lot = "Lot";
            public const string Price = "Price";
            public const string IsPyeongchang = "IsPyeongchang";
            public const string IsMail = "IsMail";
            public const string StopPrice = "StopPrice";
            public const string Settlement = "Settlement";
            public const string Level = "Level";
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

        /// <summary>更新操作紀錄</summary>
        /// <param name="_TradeRecord"></param>
        public void UpdateTradeRecord(TradeRecord _TradeRecord) {
            USP.AddParameter(SPParameter.SN, _TradeRecord.SN); 
            USP.AddParameter(SPParameter.DueMonth, _TradeRecord.DueMonth);
            USP.AddParameter(SPParameter.OP, _TradeRecord.OP);
            USP.AddParameter(SPParameter.Contract, _TradeRecord.Contract);
            USP.AddParameter(SPParameter.Type, _TradeRecord.Type);
            USP.AddParameter(SPParameter.Lot, _TradeRecord.Lot);
            USP.AddParameter(SPParameter.Price, _TradeRecord.Price);
            USP.AddParameter(SPParameter.IsPyeongchang, _TradeRecord.IsPyeongchang);
            USP.AddParameter(SPParameter.IsMail, _TradeRecord.IsMail);
            USP.AddParameter(SPParameter.StopPrice, _TradeRecord.StopPrice);
            USP.AddParameter(SPParameter.Settlement, _TradeRecord.Settlement);
            USP.AddParameter(SPParameter.Level, _TradeRecord.Level);
            USP.ExeProcedureGetDataTable(SP.UpdateTradeRecord);
        }
        
        /// <summary>取得未平昌的交易紀錄</summary>
        /// <returns></returns>
        public List<TradeRecord> GetTradeRecord() {
            List<TradeRecord> TradeList = new List<TradeRecord>();
            DataTable dt= USP.ExeProcedureGetDataTable(SP.GetTradeRecord);
            if (dt != null && dt.Rows.Count > 0) {
                foreach (DataRow row in dt.Rows) {
                    TradeRecord Record = new TradeRecord();
                    Record.SN = Convert.ToInt32(row[SPParameter.SN]);
                    Record.TradeDate = Convert.ToDateTime(row[SPParameter.TradeDate]);
                    Record.DueMonth = row[SPParameter.DueMonth].ToString();
                    Record.OP = row[SPParameter.OP].ToString();
                    Record.Contract = row[SPParameter.Contract].ToString();
                    Record.Type = row[SPParameter.Type].ToString();
                    Record.Lot = row[SPParameter.Lot].ToString();
                    Record.Price = Convert.ToDecimal(row[SPParameter.Price]);
                    Record.StopPrice = row[SPParameter.StopPrice].ToString() == string.Empty ? 0 : Convert.ToDecimal(row[SPParameter.StopPrice]);
                    Record.Settlement = row[SPParameter.Settlement].ToString() == string.Empty ? 0 : Convert.ToDecimal(row[SPParameter.Settlement]);
                    Record.Level = Convert.ToInt32(row[SPParameter.Level]);
                    Record.IsPyeongchang = Convert.ToBoolean(row[SPParameter.IsPyeongchang]);
                    Record.IsMail = Convert.ToBoolean(row[SPParameter.IsMail]);
                    TradeList.Add(Record);
                }
            }
            return TradeList;
        }

    }
}
