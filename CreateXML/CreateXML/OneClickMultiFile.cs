using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateXML {
    //20141002 add by Dick 一鍵生成多檔功能
   public class OneClickMultiFile {


       public string Export { get; set; }

       public string Parent { get; set; }

       public OneClickMultiFile(string Parent,string Export) {
           Export = Export;
           Parent = Parent;       
       }

    }
}
