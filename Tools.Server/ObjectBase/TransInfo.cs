using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectBase {
    public  class TransInfo {

        public int TranSN { set; get; }

        public int CustomerSN { set; get; }

        public string Account { set; get; }

        public string NickName { set; get; }

        public TranscationTypes TradeType { set; get; }

        public DateTime TradeTime { set; get; }

        public AuditTypes AuditState { set; get; }

        public decimal Draw { set; get; }

        public int AuditeAdviserSN { set; get; }

        public string AdviserName { set; get; }

        public DateTime AuditeTime { set; get; }

    }
}
