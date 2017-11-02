using ObjectBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adviser.Models.ViewModels
{
    public class SettingViewModels 
    {
        public class ChipsSettingViewModel : BaseViewModel{

            public Customer _Customer { set; get; }

            public int CustomerSN { set; get; }

            public List<Customer> ListCustomer { set; get; }

            public ChipsSettingViewModel()
            {
                ListCustomer = new List<Customer>();
            }

        }

    }
}