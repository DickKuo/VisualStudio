using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock {
    public class Open_Interest {

        public int SN { set; get; }

        public DateTime TradeDate { set; get; }

        public int PutVolume { set; get; }

        public int CallVolume { set; get; }

        public decimal Ratios { set; get; }

        public int PutOpenInterest { set; get; }

        public int CallOpenInterest { set; get; }
        
        public decimal OpenInterestRatios { set; get; }

    }
}
