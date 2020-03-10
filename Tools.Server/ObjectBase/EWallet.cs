using System;

namespace ObjectBase
{
    public class EWallet
    {
        public int SN { set; get; }

        public int CustomerSN { set; get; }

        public decimal Balance { set; get; }

        public decimal Pyeongchang { set; get; }

        public decimal Available { set; get; }

        public DateTime AddTime { set; get; }

        public DateTime EditTime { set; get; }

        public decimal TranscationDraw { set; get; }

        public string Remark { set; get; }
    }
}