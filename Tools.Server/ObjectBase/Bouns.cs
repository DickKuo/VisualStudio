using System;

namespace ObjectBase
{
    public class Bouns
    {
        public int SN { set; get; }

        public decimal Draw { set; get; }

        public DateTime TradeDate { set; get; }

        public BounsTypes BounsType { set; get; }

        public int AdviserSN { set; get; }

        public int CustomerSN { set; get; }
    }
}