using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;

namespace WebRoutingTest.Models.Code
{
    public class BankC : BaseBank
    {
        public BankC()
        {

        }

        public BankC(string Name)
        {
            this._BankName = Name;
        }

        public override TradeInfo Add(TradeInfo Info)
        {
            Info.Money = Info.Money * 3;
            return Info;
        }

        public override TradeInfo Reduce(TradeInfo Info)
        {
            Info.Money = Info.Money * 3;
            return Info;          
        }
    }
}