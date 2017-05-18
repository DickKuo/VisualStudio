using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectBase {

    public class EWalletDAO : CommBase {

        private class SP {
            public const string ReCalculateEWallet = "ReCalculateEWallet";
            public const string AddEWalletTrade = "AddEWalletTrade";
        }

        private class SSParameter {
            public const string Money = "Money";
            public const string TradeType = "TradeType";
        }

        /// <summary>重新計算電子錢包</summary>
        /// <param name="CustomerSN"></param>
        /// <returns></returns>
        public decimal ReCalculateEWallet(int CustomerSN) {
            USP.AddParameter(CommBase.CustomerSN, CustomerSN);
            return Convert.ToDecimal(USP.ExeProcedureSingleResult(SP.ReCalculateEWallet));
        }

        /// <summary>新增交易</summary>
        /// <param name="_EWalletTrade"></param>
        /// <returns></returns>
        public int AddEWalletTrade(EWalletTrade _EWalletTrade) {
            USP.AddParameter(CommBase.CustomerSN, _EWalletTrade.CustomerSN);
            USP.AddParameter(SSParameter.Money, _EWalletTrade.Money);
            USP.AddParameter(SSParameter.TradeType, _EWalletTrade.TradeType);
            return USP.ExeProcedureReturnKey(SP.AddEWalletTrade); 
        }
         
    }
}
