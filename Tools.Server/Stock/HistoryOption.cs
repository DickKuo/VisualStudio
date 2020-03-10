using System;

namespace Stock
{
    public class HistoryOption
    {
        /// <summary>交易日期</summary>
        public DateTime TradeDate { set; get; }

        /// <summary>契約</summary>
        public string Contract { set; get; }

        /// <summary>到期月份(週別)</summary>
        public string DueMonth { set; get; }

        /// <summary>履約價</summary>
        public string Price { set; get; }

        /// <summary>買賣權</summary>
        public string Option { set; get; }

        /// <summary>開盤價</summary>
        public decimal Opening_Price { set; get; }

        /// <summary>最高價</summary>
        public decimal Highest { set; get; }

        /// <summary>最低價</summary>
        public decimal Lowest { set; get; }

        /// <summary>收盤價</summary>
        public decimal Closing { set; get; }

        /// <summary>成交量</summary>
        public int Volume { set; get; }

        /// <summary>結算價</summary>
        public decimal Settlement { set; get; }

        /// <summary>未沖銷契約數</summary>
        public int NumberOfContracts { set; get; }

        /// <summary>最後最佳買價</summary>
        public decimal GreatBuy { set; get; }

        /// <summary>最後最佳賣價</summary>
        public decimal GreatSell { set; get; }

        /// <summary>歷史最高價</summary>
        public decimal History_Highest { set; get; }

        /// <summary>歷史最低價</summary>
        public decimal History_Lowest { set; get; }

        /// <summary>是否因訊息面暫停交易</summary>
        public string Remark { set; get; }
    }
}