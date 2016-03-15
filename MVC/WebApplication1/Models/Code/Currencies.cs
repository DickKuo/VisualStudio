using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Code
{
    public class Currencies
    {

        public enum Currency
        {
            USD = 0,//美元
            JPY = 1,//日元
            CNY = 2,//人民幣
            TWD = 3,//新台幣
            HKD = 4,//港元
            EUR = 5,//歐元
            THB = 6,//泰銖
            KRW = 7,//韓元
            GBP = 8,//英鎊
            SGD = 9//新加坡元
        }
        
    }
}