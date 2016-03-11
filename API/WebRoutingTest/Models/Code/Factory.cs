using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;

namespace WebRoutingTest.Models.Code
{
    public class Factory
    {

    
        private class Names
        {
            public const string Bank1 = "Bank1";
            public const string Bank2 = "Bank2";
            public const string Bank3 = "Bank3";
        }


        /// <summary>取得銀行 </summary>
        /// <param name="bank"></param>
        /// <returns></returns>
        public BaseBank GetBank( TradeInfo Info)
        {
            BaseBank BankResult = null;
            switch (Info.BankNo)
            {
                case 0:
                    BankResult = new BankA(Names.Bank1);
                    break;
                case 1:
                    BankResult = new BankB(Names.Bank2);
                    break;
                case 2:
                    BankResult = new BankC(Names.Bank3);
                    break;
            }
            return BankResult;
        }
    }
}