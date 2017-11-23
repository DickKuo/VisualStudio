using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectBase {

    public class AdviserEWalletDAO : CommBase {

        private class SP {
            public const string GetAdviserEWalletByAdviserSN = "GetAdviserEWalletByAdviserSN";
        }

        private class SParamter {
            public const string AdviserSN = "AdviserSN";
        }

        /// <summary>取得電子錢包</summary>
        /// <param name="AdviserSN"></param>
        /// <returns></returns>
        public AdviserEWallet GetAdviserEWalletByAdviserSN(int AdviserSN) {
            USP.AddParameter(SParamter.AdviserSN, AdviserSN);
            AdviserEWallet Wallet = USP.ExeProcedureGetObject(SP.GetAdviserEWalletByAdviserSN,new AdviserEWallet());
            return Wallet;
        }
    }
}
