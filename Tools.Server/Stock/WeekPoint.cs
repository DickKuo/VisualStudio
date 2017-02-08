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
        public string StopPirce { set; get; }
    }
}
