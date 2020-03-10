namespace ObjectBase
{
    public class EWalletDAO : CommBase
    {
        private class SP
        {
            public const string UpdateEWallet = "UpdateEWallet";
            public const string GetEWalletByCustomerSN = "GetEWalletByCustomerSN";
        }

        private class SSParameter
        {
            public const string SN = "SN";
            public const string Balance = "Balance";
            public const string Pyeongchang = "Pyeongchang";
            public const string Available = "Available";
            public const string EditTime = "EditTime";
            public const string Remark = "Remark";
            public const string CustomerSN = "CustomerSN";
        }

        /// <summary>更新錢包狀態</summary>
        /// <param name="_EWallet"></param>
        /// <returns></returns>
        public int UpdateEWallet(EWallet _EWallet)
        {
            USP.AddParameter(SSParameter.Balance, _EWallet.Balance);
            USP.AddParameter(SSParameter.Pyeongchang, _EWallet.Pyeongchang);
            USP.AddParameter(SSParameter.Available, _EWallet.Available);
            USP.AddParameter(SSParameter.CustomerSN, _EWallet.CustomerSN);
            EWallet Wallet = USP.ExeProcedureGetObject(SP.UpdateEWallet, new EWallet()) as EWallet;
            if (Wallet.SN > 0)
            {
                return Wallet.SN;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>依照客戶取得電子錢包</summary>
        /// <param name="CustomerSN"></param>
        /// <returns></returns>
        public EWallet GetEWalletByCustomerSN(int CustomerSN)
        {
            USP.AddParameter(SSParameter.CustomerSN, CustomerSN);
            EWallet Wallet = USP.ExeProcedureGetObject(SP.GetEWalletByCustomerSN, new EWallet()) as EWallet;
            return Wallet;
        }
    }
}