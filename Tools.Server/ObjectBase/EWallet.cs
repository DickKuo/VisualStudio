using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectBase {
    public class EWallet {

        public int SN { set; get; }

        public int CustomerSN { set; get; }

        public decimal Money { set; get; }

        public DateTime AddTime { set; get; }

        public DateTime EditTime { set; get; }

        public string Remark { set; get; }

    }
}
