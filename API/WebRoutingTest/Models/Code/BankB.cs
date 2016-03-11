using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;

namespace WebRoutingTest.Models.Code
{
    public class BankB : BaseBank
    {
        public BankB()
        {

        }

        public BankB(string Name)
        {
            this._BankName = Name;
        }

        public override TradeInfo Add(TradeInfo Info)
        {
            Info.Money = Info.Money * 2;
            return Info;
        }

        public override TradeInfo Reduce(TradeInfo Info)
        {
            Info.Money = Info.Money * 2;
            return Info;          
        }
    }
}