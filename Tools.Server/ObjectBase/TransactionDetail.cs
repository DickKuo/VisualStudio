using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectBase {
    public  class TransactionDetail {

        public int SN { set; get; }

        public int TransactionSN { set; get; }

        public string BankName { set; get; }

        public string BankAccount { set; get; }

        public string BranchName { set; get; }

        public decimal Draw { set; get; }

        public decimal Fee { set; get; }

        public string Remark { set; get; }
    }
}
