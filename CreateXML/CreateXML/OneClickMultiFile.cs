using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

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


       /// <summary>
       ///  20141226 add by Dick for 雙檔主表EditView
       /// </summary>
       /// <param name="pEntityName"></param>
       /// <param name="dt"></param>
       /// <param name="Mode"></param>
       public void CreateEntityHasDetail(string pSettingPath,string pEntityName,string pEntityDetailName, DataGridView dt, int Mode)
       {          
           DirectoryInfo DirInfo = new DirectoryInfo(pSettingPath);
           string SourcePath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "SampleFile\\HasDetail.txt";
           StringBuilder Content = new StringBuilder();
           using (StreamReader sr = new StreamReader(SourcePath,Encoding.Default))
           {
               string line = string.Empty;
               while ((line = sr.ReadLine()) != null) {
                   line = line.Replace("XTestDate",DateTime.Now.ToString("yyyyMMdd"));
                   line = line.Replace("XTestDetail", pEntityDetailName);
                   line = line.Replace("XTest", pEntityName);                   
                   Content.AppendLine(line);
               }
           }
           string SaveFile = DirInfo.Parent.FullName + Path.DirectorySeparatorChar + "DigiWin.HR.CustomUI" + Path.DirectorySeparatorChar + pEntityName + ".cs";
           FileTool.Files.WritFile(Content, SaveFile);
       }



       /// <summary>
       /// 20141226 add by Dick 雙檔新增明細的瀏覽畫面
       /// </summary>
       /// <param name="pSettingPath"></param>
       /// <param name="pEntityName"></param>
       /// <param name="pEntityDetailName"></param>
       public void CreateDetailEntityBrowse(string pSettingPath,string pEntityName,string pEntityDetailName)
       {
           DirectoryInfo DirInfo = new DirectoryInfo(pSettingPath);
           string SourcePath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "SampleFile\\Detail.txt";
           StringBuilder Content = new StringBuilder();
           using (StreamReader sr = new StreamReader(SourcePath, Encoding.Default))
           {
               string line = string.Empty;
               while ((line = sr.ReadLine()) != null)
               {
                   line = line.Replace("XTestDate", DateTime.Now.ToString("yyyyMMdd"));
                   line = line.Replace("XTestDetail", pEntityDetailName);
                   line = line.Replace("XTest", pEntityName);
                   Content.AppendLine(line);
               }
           }
           string SaveFile = DirInfo.Parent.FullName + Path.DirectorySeparatorChar + "DigiWin.HR.CustomUI" + Path.DirectorySeparatorChar + pEntityDetailName + ".cs";
           FileTool.Files.WritFile(Content, SaveFile);
       }


       public void CreateDetailEditView(string pSettingPath, string pEntityName, string pEntityDetailName)
       {
           DirectoryInfo DirInfo = new DirectoryInfo(pSettingPath);
           string SourcePath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "SampleFile\\DetailEditView.txt";
           StringBuilder Content = new StringBuilder();
           using (StreamReader sr = new StreamReader(SourcePath, Encoding.Default))
           {
               string line = string.Empty;
               while ((line = sr.ReadLine()) != null)
               {
                   line = line.Replace("XTestDate", DateTime.Now.ToString("yyyyMMdd"));
                   line = line.Replace("XTestDetail", pEntityDetailName);
                   line = line.Replace("XTest", pEntityName);
                   Content.AppendLine(line);
               }
           }
           string SaveFile = DirInfo.Parent.FullName + Path.DirectorySeparatorChar + "DigiWin.HR.CustomUI" + Path.DirectorySeparatorChar + pEntityDetailName+"UI" + ".cs";
           FileTool.Files.WritFile(Content, SaveFile);
       }


    }
}
