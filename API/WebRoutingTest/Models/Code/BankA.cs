using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;

namespace WebRoutingTest.Models.Code
{
    public class BankA : BaseBank
    {
        public BankA()
        {

        }

        public BankA(string Name)
        {
            this._BankName = Name;
        }


        public override TradeInfo Add(TradeInfo Info)
        {
            Info.Money = Info.Money;           
            return Info;
        }

        public override TradeInfo Reduce(TradeInfo Info)
        {
            Info.Money = Info.Money; 
            return Info;
        }


    }
}