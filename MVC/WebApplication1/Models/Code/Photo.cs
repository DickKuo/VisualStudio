using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication1.Code
{
    public class Photo
    {

        public string ImageNo { set; get; }

        public string Remark { set; get; }

        /// <summary> 附檔名 </summary>
        public string AttachedFileName { set; get; }

        public int ImageOrder { set; get; }

  
    }
}