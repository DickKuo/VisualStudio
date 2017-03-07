using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock {
    public class WeekPoint {

        /// <summary>Primary</summary>
        public int SN { set; get; }

        /// <summary>交易日期</summary>
        public DateTime TradeDate { set; get; }

        /// <summary>操作方向</summary>
        public string OP { set; get; }

        /// <summary>契約</summary>
        public string Contract { set; get; }

        /// <summary>價格</summary>
        public string Price { set; get; }

        /// <summary>交易量</summary>
        public string Volume { set; get; }

        /// <summary>停損價格</summary>
        public string StopPrice { set; get; }

        /// <summary>周</summary>
        public string DueMonth { set; get; }

        /// <summary>買方停損價</summary>
        public string BuyStopPrice { set; get; }

        /// <summary>結算價格</summary>
        public string ClosePrice { set; get; }
    }
}
