using System;

namespace ObjectBase
{
    public class Reward
    {
        public int SN { set; get; }

        public int CustomerSN { set; get; }

        public DateTime RewardTime { set; get; }

        public decimal Money { set; get; }

        public bool IsPay { set; get; }

        public DateTime PayTime { set; get; }

        public int PayCustomerSN { set; get; }

        public string Remark { set; get; }
    }
}