using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;

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
           sb.AppendLine("namespace Dcms.HR.DataEntities {");
           sb.AppendLine("    using System;");
           sb.AppendLine("    using System.ComponentModel;");
           sb.AppendLine("    using System.Runtime.Serialization;");
           sb.AppendLine("    using System.Security.Permissions;");
           sb.AppendLine("    using System.Xml.Serialization;");
           sb.AppendLine("    using Dcms.Common.Torridity;");
           // using Dcms.Common.Torridity.Properties;        
           sb.AppendLine("    [Serializable()]");
           sb.AppendLine(string.Format(@"    public class {0}Collection : DataEntityList<{0}> ", ClassName));
           sb.AppendLine("{");
           sb.AppendLine(string.Format(@"        public {0}Collection() ", ClassName));
           sb.AppendLine("{");
           sb.AppendLine("    }");
           sb.AppendLine(string.Format(@"        public {0}Collection(object pOwner) : ", ClassName));
           sb.AppendLine("");
           sb.AppendLine("        base(pOwner) {}");
           sb.AppendLine("    }");
           sb.AppendLine("}"); 
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
           CommTool.Files.WritFile(Content.ToString(), SaveFile,false);
       }


       /// <summary>
       /// 20150120 add by Dick for 加入dev #13
       /// </summary>
       /// <param name="EntityName"></param>
       /// <param name="DetailEnitiyName"></param>
       /// <returns></returns>
       public string CreateNewDev(string EntityName,string DetailEnitiyName)
       {
           StringBuilder result = new StringBuilder();
           result.AppendLine("            //");
           result.AppendLine(string.Format("            //dev", DetailEnitiyName));
           result.AppendLine("            //");
           result.AppendLine(string.Format("            this.dev{0}.DataSource = this.{1}BindingSource;",DetailEnitiyName ,EntityName ));
           result.AppendLine(string.Format("            this.dev{0}.SourceType = typeof(Dcms.HR.DataEntities.{1});", DetailEnitiyName, DetailEnitiyName));
           return result.ToString();
       }
       
       /// <summary>
       /// 20150121 add by Dick for 產生明細瀏覽畫面控件 #13
       /// </summary>
       /// <param name="DetatilEntityName"></param>
       /// <param name="Grid"></param>
       /// <returns></returns>
       public string CreateDeclare(string DetatilEntityName, DataGridView Grid)
       {        
           StringBuilder result = new StringBuilder();
           result.AppendLine(string.Format("        private TabPage tpg{0};", DetatilEntityName));
           result.AppendLine(string.Format("        private DataEntityListView dev{0};", DetatilEntityName));
           result.AppendLine(string.Format("        private DcmsEditGrid dcms{0};", DetatilEntityName));
           result.AppendLine(string.Format("        private DevExpress.XtraGrid.Views.Grid.GridView grid{0};", DetatilEntityName));
           DataTable dt = Grid.DataSource as DataTable;
           foreach (DataRow dr in dt.Rows)
           {
               if (!string.IsNullOrEmpty(dr[4].ToString()))
               {
                   result.AppendLine(string.Format("        private DevExpress.XtraGrid.Columns.GridColumn grid{0};", dr[0].ToString()));
               }
           }
           return result.ToString();
       }


       /// <summary>
       /// 20150121 add by Dick for 產生明細瀏覽畫面控件 #13
       /// </summary>
       /// <param name="DetatilEntityName"></param>
       /// <param name="Grid"></param>
       /// <returns></returns>
       public string CreateInit(string DetatilEntityName, DataGridView Grid)
       {
           StringBuilder result = new StringBuilder();
           result.AppendLine(string.Format("            this.tpg{0} = new System.Windows.Forms.TabPage();", DetatilEntityName));
           result.AppendLine(string.Format("            this.dev{0} = new Dcms.Common.UI.DataEntityListView();", DetatilEntityName));
           result.AppendLine(string.Format("            this.dcms{0} = new Dcms.Common.UI.DcmsEditGrid();", DetatilEntityName));
           result.AppendLine(string.Format("            this.grv{0} = new DevExpress.XtraGrid.Views.Grid.GridView();", DetatilEntityName));
           DataTable dt = Grid.DataSource as DataTable;
           foreach (DataRow dr in dt.Rows)
           {
               if (!string.IsNullOrEmpty(dr[4].ToString()))
               {
                   result.AppendLine(string.Format("            this.grid{0} = new DevExpress.XtraGrid.Columns.GridColumn();", dr[0].ToString()));
               }
           }
           return result.ToString();
       }

       /// <summary>
       /// 20141226 add by Dick 雙檔新增明細的瀏覽畫面
       /// </summary>
       /// <param name="pSettingPath"></param>
       /// <param name="pEntityName"></param>
       /// <param name="pEntityDetailName"></param>
       public void CreateDetailEntityBrowse(string pSettingPath,string pEntityName,string pEntityDetailName,StringBuilder dev)
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
           CommTool.Files.WritFile(Content.ToString(), SaveFile,false);
       }

       /// <summary>
       /// 20150106 modified by Dick for 明細的編輯畫面
       /// </summary>
       /// <param name="pSettingPath"></param>
       /// <param name="pEntityName"></param>
       /// <param name="pEntityDetailName"></param>
       public void CreateDetailEditView(string pSettingPath, string pEntityName, string pEntityDetailName)
       {
           DirectoryInfo DirInfo = new DirectoryInfo(pSettingPath);
           string SourcePath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "SampleFile\\DetailEditView.txt";
           StringBuilder Content = new StringBuilder();
           string SubName = pEntityDetailName.Substring(0, 1).ToLower();
           SubName += pEntityDetailName.Substring(1,pEntityDetailName.Length-1);
           using (StreamReader sr = new StreamReader(SourcePath, Encoding.Default))
           {
               string line = string.Empty;
               while ((line = sr.ReadLine()) != null)
               {
                   line = line.Replace("XTestDate", DateTime.Now.ToString("yyyyMMdd"));
                   line = line.Replace("XTestDetail", pEntityDetailName);
                   line = line.Replace("XTest", pEntityName);
                   line = line.Replace("xTestDetail", SubName);
                   Content.AppendLine(line);
               }
           }
           string SaveFile = DirInfo.Parent.FullName + Path.DirectorySeparatorChar + "DigiWin.HR.CustomUI" + Path.DirectorySeparatorChar + pEntityDetailName+"UI" + ".cs";
           CommTool.Files.WritFile(Content.ToString(), SaveFile,false);
       }


    }
}
