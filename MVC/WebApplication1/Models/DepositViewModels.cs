using ObjectBase;

namespace WebApplication1.Models {
    public class DepositViewModels {

        public class DepositViewModel :BaseViewModel {

            public Customer _Customer { set; get; }

            public Transaction _Transaction { set; get; }
        }
    }
}