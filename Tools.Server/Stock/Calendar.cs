using System;

namespace Stock
{
    public class Calendar
    {
        /// <summary>PrimayKey</summary>
        public int SN { set; get; }

        /// <summary>每日</summary>
        public DateTime Daily { set; get; }

        /// <summary>是否開盤</summary>
        public bool IsWorkDay { set; get; }

        /// <summary>周選</summary>
        public string Week { set; get; }

        /// <summary>近月1</summary>
        public string NearMonth1 { set; get; }

        /// <summary>近月2</summary>
        public string NearMonth2 { set; get; }

        /// <summary>是否發送Maill</summary>
        public bool IsMaill { set; get; }

        public bool IsSettlement { set; get; }

        /// <summary>備註</summary>
        public string Remark { set; get; }
    }
}