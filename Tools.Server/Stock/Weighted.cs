using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock {
    public class Weighted {

        public int SN { set; get; }

        /// <summary>日期</summary>
        public DateTime TradeDate { set; get; }
        
        /// <summary>現貨</summary>
        public decimal Price { set; get; }

        /// <summary>期貨</summary>
        public decimal Futures { set; get; }

        /// <summary>開盤指數</summary>
        public decimal OpenPrice { set; get; }

        /// <summary>最高指數</summary>
        public decimal HighestPrice { set; get; }

        /// <summary>最低指數</summary>
        public decimal LowestPrice { set; get; }

        /// <summary>收盤指數</summary>
        public decimal ClosingPrice { set; get; }

        /// <summary>成交量</summary>
        public string Volume { set; get; }

        /// <summary>漲跌</summary>
        public decimal Change { set; get; }

        /// <summary>備註</summary>
        public string Remark { set; get; }

        /// <summary>時間戳章</summary>
        public string TradeTimestamp { set; get; }

    }
}