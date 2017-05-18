using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectBase {
    public class RewardDAO : CommBase {

        private class SP {
            public const string AddReward = "AddReward";
            public const string PayReward = "PayReward";
        }

        private class SSParameter {
            public const string RewardTime = "RewardTime";
            public const string Money = "Money";
            public const string IsPay = "IsPay";
            public const string PayTime = "PayTime";
            public const string PayCustomerSN = "PayCustomerSN";
        }

        /// <summary>加入新的獎金</summary>
        /// <param name="_Reward"></param>
        /// <returns></returns>
        public int AddReward(Reward _Reward) {
            USP.AddParameter(CommBase.CustomerSN, _Reward.CustomerSN);
            USP.AddParameter(SSParameter.RewardTime, _Reward.RewardTime);
            USP.AddParameter(SSParameter.Money, _Reward.Money);
            return  USP.ExeProcedureReturnKey(SP.AddReward);
        }

        /// <summary>發放獎金</summary>
        /// <param name="_Reward"></param>
        /// <returns></returns>
        public int PayReward(Reward _Reward) {
            USP.AddParameter(CommBase.SN, _Reward.SN);
            USP.AddParameter(SSParameter.PayCustomerSN, _Reward.PayCustomerSN);
            return USP.ExeProcedureReturnKey(SP.PayReward);
        }

    }
}
