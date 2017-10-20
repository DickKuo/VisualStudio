using ObjectBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models {
    public class BankViewModels {


        public class BankViewModel : BaseViewModel {

            public BankViewModel() {
                ListBank = new List<Bank>();
            }

            public int BankSN { set; get; }

            public Bank _Bank { set; get; }

            public List<Bank> ListBank { set; get; }

        }
    
    }
}