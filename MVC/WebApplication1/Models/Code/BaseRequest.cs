using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Code {
    public class BaseRequest {

        public string Status { set; get; }

        public string Account { set; get; }
        
        public decimal Draw { set; get; }

        public string BankAccount { set; get; }

        public string BranchName { set; get; }

        public string BankName { set; get; }

        public string Remark { set; get; }
    }
}