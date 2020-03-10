using System;

namespace Stock
{
    public class TradeRecord
    {
        public int SN { set; get; }

        public DateTime TradeDate { set; get; }

        public string DueMonth { set; get; }

        public string OP { set; get; }

        public string Contract { set; get; }

        public string Type { set; get; }

        public string Lot { set; get; }

        /// <summary>操作價格</summary>
        public decimal Price { set; get; }

        /// <summary>停損價格</summary>
        public decimal StopPrice { set; get; }

        /// <summary>結算點數</summary>
        public decimal Settlement { set; get; }

        public int Level { set; get; }

        public bool IsPyeongchang { set; get; }

        public DateTime PyeongchangTime { set; get; }

        public bool IsMail { set; get; }

        public int AdviserSN { set; get; }

        public int ChangeTimes { set; get; }
    }
}