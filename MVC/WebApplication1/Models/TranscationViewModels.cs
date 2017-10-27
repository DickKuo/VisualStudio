using ObjectBase;
using System;
using System.Collections.Generic;

namespace WebApplication1.Models {
    public class TranscationViewModels {

        public class TranscationViewModel {
            public int MaxPage { set; get; }

            public DateTime BeginTime { set; get; }

            public DateTime EndTime { set; get; }

            public List<Transaction>  TransactionList { set; get; }

            public int Page { set; get; }

            public int Range { set; get; }

            public decimal Total { set; get; }

            public string TradeType { set; get; }

            public string Money { set; get; }

            public string AuditState { set; get; }

            public TranscationViewModel() {
                TransactionList = new List<Transaction>();
                Page = 1;
                Range = 10;
            }
        }
    }
}