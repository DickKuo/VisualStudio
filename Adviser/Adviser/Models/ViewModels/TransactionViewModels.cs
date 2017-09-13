using ObjectBase;
using System.Collections.Generic;

namespace Adviser.Models.ViewModels {
    public class TransactionViewModels  {

        public class TransactionViewModel : BaseViewModel {             

            public int TransSN { set; get; }

            public TranscationTypes TransType { set; get; }

            public AuditTypes Audit { set; get; }

            public List<TransInfo> ListTransInfo { set; get; }

            public Transaction Trans { set; get; }

            public Customer _Customer { set; get; }

        }
    }
}