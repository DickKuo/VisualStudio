
namespace ObjectBase {
    public class Customer {

        /// <summary>Primary</summary>
        public int SN { set; get; }

        public string Account { set; get; }

        public string PassWord { set; get; }

        public int State { set; get; }

        public bool IsAudite { set; get; }

        public int LockCount { set; get; }

        public int IsLock { set; get; }

        public Member Member { set; get; }

        public string Remark { set; get; }
    }
}
