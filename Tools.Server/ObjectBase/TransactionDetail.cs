namespace ObjectBase
{
    public class TransactionDetail
    {
        public int SN { set; get; }

        public int TransactionSN { set; get; }

        public string BankName { set; get; }

        public string BankAccount { set; get; }

        /// <summary>分行號碼</summary>
        public string BankCode { set; get; }

        public decimal Draw { set; get; }

        public decimal Commission { set; get; }

        public decimal Fee { set; get; }

        public string Remark { set; get; }
    }
}