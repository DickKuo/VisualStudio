using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Code.DAO
{
    public class TradeInfo
    {

        public enum OperationCode { 
            Add=0,
            Reduce=1
        }

        public enum ResultCode { 
             Sucess=0,
             Error=1
        }
        

        public int  BankNo { set; get; }

        public string  EmpNo { set; get; }

        public decimal Money { set; get; }
        

        public ResultCode ApiResultCode { set; get; }



        public string ApiTradeResult { set; get; }


        public OperationCode Operation { set; get; }

    }
}