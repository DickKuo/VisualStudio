using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectBase {
    public class Attachments {

        public int SN { set; get; }

        public string AttName { set; get; }

        /// <summary>主檔案ID</summary>
        public int TargetID { set; get; }

        public int AttType { set; get; }
        
        public static string[] GetTableTypeColumn()
        {
            return new string[] {  "AttName", "AttType" };
        }
    }

    public enum AttTypes {

        Deposit = 0 ,

        Withdrawal = 1
    }
}
