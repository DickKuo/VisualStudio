using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock {
    public class TradeRecord {

        public int SN { set; get; }

        public DateTime TradeDate { set; get; }

        public string DueMonth { set; get; }

        public string OP { set; get; }

        public string Contract { set; get; }

        public string Type { set; get; }

        public string Lot { set; get; }

        public string Price { set; get; }

        public bool IsPyeongchang { set; get; }

        public bool IsMail { set; get; }
    }
}
