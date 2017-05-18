using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectBase {
    public class EWalletTrade {

        public int SN { set; get;}

        public int CustomerSN { set; get; }

        public int TradeType { set; get; }

        public decimal Money { set; get; }

        public DateTime TradeTime { set; get; }

        public bool IsAudite { set; get; }

        public int  AuditeCustomerSN { set; get; }

        public string Remark { set; get; }
    }
}
