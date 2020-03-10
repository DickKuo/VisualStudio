using System.Collections.Generic;
using System;

namespace ObjectBase
{
    public class Transaction
    {
        public int SN { set; get; }

        public int CustomerSN { set; get; }

        public TranscationTypes TradeType { set; get; }

        public DateTime TradeTime { set; get; }

        public AuditTypes AuditState { set; get; }

        public int AuditAdviserSN { set; get; }

        public string TransKey { set; get; }

        public DateTime AuditTime { set; get; }

        public TransactionDetail Detail { set; get; }

        public List<Attachments> AttachmentsList { set; get; }

        public Transaction()
        {
            AttachmentsList = new List<Attachments>();
        }
    }
}