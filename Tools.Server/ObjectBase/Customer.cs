using System;

namespace ObjectBase
{
    public class Customer
    {
        /// <summary>Primary</summary>        
        public int SN { set; get; }

        /// <summary>帳號</summary>
        public string Account { set; get; }

        /// <summary>密碼</summary>
        public string PassWord { set; get; }

        /// <summary>狀態</summary>
        public int State { set; get; }

        /// <summary>是否審核</summary>
        public AuditTypes Audit { set; get; }

        /// <summary>帳密錯誤次數</summary>
        public int LockCount { set; get; }

        /// <summary>是否鎖住</summary>
        public int IsLock { set; get; }

        /// <summary></summary>
        public Member Member { set; get; }

        /// <summary>權限資訊</summary>
        public Role Role { set; get; }

        /// <summary>備註</summary>
        public string Remark { set; get; }

        /// <summary>最小下單組數</summary>
        public int MinimunLotLimit { set; get; }

        public DateTime AddTime { set; get; }

        public bool IsEnable { set; get; }

        public int HelperSN { set; get; }

        public decimal CommissionRate { set; get; }

        public decimal Chips { set; get; }
    }
}