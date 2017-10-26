using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Code {
    public class DepositRequest : BaseRequest {

        public string Status { set; get; }

        public HttpPostedFileBase MoneyOrder1 { set; get; }

        public HttpPostedFileBase MoneyOrder2 { set; get; }

        public HttpPostedFileBase MoneyOrder3 { set; get; }
    }
}