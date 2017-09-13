﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectBase {
    public class Transaction {

        public int SN { set; get;}

        public int CustomerSN { set; get; }

        public TranscationTypes TradeType { set; get; }

        public DateTime TradeTime { set; get; }

        public AuditTypes AuditState { set; get; }

        public int AuditAdviserSN { set; get; }

        public DateTime AuditTime { set; get; }

        public TransactionDetail Detail { set; get; }
    }
}
