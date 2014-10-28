using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateXML {
    //20141002 add by Dick 一鍵生成多檔功能
    public class OneClickMultiFile 
    {


       public string Export { get; set; }

       public string Parent { get; set; }

      

       public OneClickMultiFile(string Parent,string Export) {
           Export = Export;
           Parent = Parent;       
       }


        /// <summary>
        /// 20141028 add by Dick
        /// 建立Collection.cs 
        /// </summary>
        /// <param name="ClassName">明細的名稱</param>
        /// <returns></returns>
       public string CreateCollection(string ClassName)
       {
           StringBuilder sb = new StringBuilder();
        sb.Append("namespace Dcms.HR.DataEntities {\r\n");
        sb.Append("    using System;\r\n");
        sb.Append("    using System.ComponentModel;\r\n");
        sb.Append("    using System.Runtime.Serialization;\r\n");
        sb.Append("    using System.Security.Permissions;\r\n");
        sb.Append("    using System.Xml.Serialization;\r\n");
        sb.Append("    using Dcms.Common.Torridity;\r\n");
                // using Dcms.Common.Torridity.Properties;        
        sb.Append("    [Serializable()]\r\n");
        sb.Append(string.Format(@"    public class {0}Collection : DataEntityList<{0}> ", ClassName));
        sb.Append("{\r\n");
        sb.Append(string.Format(@"        public {0}Collection() ", ClassName));
        sb.Append("{\r\n");
        sb.Append("    }\r\n");

        sb.Append(string.Format(@"        public {0}Collection(object pOwner) : ", ClassName));
        sb.Append("\r\n");
        sb.Append("        base(pOwner) {}\r\n");
        sb.Append("    }\r\n");
        sb.Append("}\r\n"); 
           return sb.ToString();
       }
    }
}
