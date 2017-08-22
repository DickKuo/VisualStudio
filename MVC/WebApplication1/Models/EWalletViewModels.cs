using ObjectBase;
using System.Collections.Generic;

namespace WebApplication1.Models {
    public class EWalletViewModels {

        public class EWalletViewModel {

            public EWallet _EWallet { set; get; }

            public Customer _Customer { set; get; }

            public List<Transaction> TransList { set; get; }

            public string JsonString { set; get; }
        }

    }
}