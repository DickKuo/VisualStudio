using ObjectBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adviser.Models.ViewModels {
    public class CustomerViewModels : BaseViewModel {

        public class CustomerViewModel {
            
            public int CustomerSN { set; get; } 

            public Customer _Customer { set; get; }

            public List<Customer> ListCustomer { set; get; }
        }
    }
}