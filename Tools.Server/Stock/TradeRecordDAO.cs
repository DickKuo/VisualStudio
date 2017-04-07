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
            public const string GetDueDateSettlement="GetDueDateSettlement";
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
            public const string PyeongchangTime = "PyeongchangTime";
        }

        /// <summary>新增操作紀錄</summary>
        /// <param name="_TradeRecord"></param>
        /// <returns></returns>
        public int AddTradeRecord(TradeRecord _TradeRecord) {
            USP.AddParameter(SPParameter.TradeDate, _TradeRecord.TradeDate);
            USP.AddParameter(SPParameter.DueMonth, _TradeRecord.DueMonth.ToUpper());
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
            USP.AddParameter(SPParameter.DueMonth, _TradeRecord.DueMonth.ToUpper());
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
            USP.AddParameter(SPParameter.PyeongchangTime, _TradeRecord.PyeongchangTime);
            USP.ExeProcedureGetDataTable(SP.UpdateTradeRecord);
        }
        
        /// <summary>取得未平昌的交易紀錄</summary>
        /// <returns></returns>
        public List<TradeRecord> GetTradeRecord() {
            List<TradeRecord> TradeList = new List<TradeRecord>();
            TradeList = USP.ExeProcedureGetObjectList(SP.GetTradeRecord,new TradeRecord()); 
            return TradeList;
        }

        /// <summary>取得時間區間內的結算金額</summary>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public decimal GetDueDateSettlement(string BeginDate,string EndDate) {
            USP.AddParameter(BaseData.BaseSParameter.BeginDate,BeginDate);
            USP.AddParameter(BaseData.BaseSParameter.EndDate, EndDate);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetDueDateSettlement);
            if (dt != null && dt.Rows.Count > 0) {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            else {
                return 0;
            }
        }
        
        /// <summary>計算時間區間的獎勵</summary>
        /// <returns></returns>
        public decimal CalculateReward(string BeginDate, string EndDate) {
            decimal RecordSettlement = GetDueDateSettlement(BeginDate, EndDate);
            int BasePrice = 50;
            if (RecordSettlement >= 500 && RecordSettlement <= 999) {
                return BasePrice * RecordSettlement * 0.06m;
            }
            else if (RecordSettlement >= 1000 && RecordSettlement <= 1999) {
                return BasePrice * RecordSettlement * 0.09m;
            }
            else if (RecordSettlement >= 2000 && RecordSettlement <= 3999) {
                return BasePrice * RecordSettlement * 0.12m;
            }
            else if (RecordSettlement >= 4000 && RecordSettlement <= 6999) {
                return BasePrice * RecordSettlement * 0.15m;
            }
            else if (RecordSettlement >= 7000 && RecordSettlement <= 9999) {
                return BasePrice * RecordSettlement * 0.18m;
            }
            else if (RecordSettlement >= 10000) {
                return BasePrice * RecordSettlement * 0.21m;
            }
            else {
                return 0;
            }
        }
        
    }
}
