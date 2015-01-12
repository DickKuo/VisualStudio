using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Data;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Resources;

namespace CreateXML {
   public class OneClick {

        public string Expory { get; set; }

        public string Parent { get; set; }

        

        public OneClick(string parent, string export) {
            Expory = export;
            Parent = parent;
        }

        /// <summary>
        /// 20140819 add by Dick 儲存CS檔
        /// </summary>
        /// <param name="pDirectory"></param>
        /// <param name="pSunDirectory"></param>
        /// <param name="pFileName"></param>
        /// <param name="pContext"></param>
        public void CSFileSave(string pDirectory, string pSubDirectory, string pFileName, string pContext) {
            //string pPath = Parent + Path.DirectorySeparatorChar + pDirectory + Path.DirectorySeparatorChar + pSubDirectory;
            //string FullFileName = pPath + Path.DirectorySeparatorChar + pFileName + ".cs";
            string reuslt =string.Empty;
            GetDirectory(ref reuslt, Parent + Path.DirectorySeparatorChar + pDirectory, pSubDirectory);
            string FullFileName = reuslt + Path.DirectorySeparatorChar + pFileName + ".cs";
            using(StreamWriter Sw = new StreamWriter(FullFileName, false)) {
                Sw.Write(pContext);
            }
        }
        public void  GetDirectory(ref string result,string Parent ,string det)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Parent);
            foreach (DirectoryInfo dr in dirInfo.GetDirectories())
            {
                    if (!dr.Name.Equals(det))
                    {
                        GetDirectory(ref result, dr.FullName, det);
                    }
                    else
                    {
                        result= dr.FullName;
                    }
                }
        }

        /// <summary>
        /// 產生QueryView
        /// </summary>
        /// <param name="pFileName"></param>
        /// <param name="pContext"></param>
        /// <param name="GridView"></param>
        public void CreateQueryView(string pFileName, string pContext, System.Windows.Forms.DataGridView GridView) {
            string pPath = Parent + Path.DirectorySeparatorChar + "DigiWin.HR.CustomBusinessImplement" + Path.DirectorySeparatorChar + "Configuration" + Path.DirectorySeparatorChar + "Query" + Path.DirectorySeparatorChar + "Case";
            string FullFileName = pPath + Path.DirectorySeparatorChar + pFileName + ".xml";
            string EntityName = pFileName;
            pContext = NewQueryView();
            StreamWriter Sw = new StreamWriter(FullFileName, false);
            Sw.Write(pContext);
            Sw.Close();
            //SubQueryView(GridView, FullFileName, EntityName, "Browse", "Browse");
        }

        /// <summary>
        /// 建立QueryViewXmL結構
        /// 20141004 modified by Dick for 修改成只建立基本結構。
        /// </summary>
        /// <returns></returns>
        private string NewQueryView() {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<QueryConfiguration  Version=\"5.1.3.1\" IsCase=\"1\">");
            sb.Append(" <QueryViewXML>");
            //sb.Append("   <QueryView>");
            //sb.Append("      <QueryViewId></QueryViewId>");
            //sb.Append("      <Name>Browse</Name>");
            //sb.Append("      <RefToTypeKey></RefToTypeKey>");
            //sb.Append("      <IsSystem>true</IsSystem>");
            //sb.Append("      <QueryViewColumns></QueryViewColumns>");
            //sb.Append("   </QueryView>");
            sb.Append(" </QueryViewXML>");
            sb.Append(" <QueryProjectXML>");
            //sb.Append("   <QueryProject>");
            //sb.Append("   <Text>Browse</Text>");
            //sb.Append("   <QueryProjectId></QueryProjectId>");
            //sb.Append("   <ProjectType>2</ProjectType>");
            //sb.Append("   <Name>Browse</Name>");
            //sb.Append("   <RefToTypeKey></RefToTypeKey>");
            //sb.Append("   <QueryViewId></QueryViewId>");
            //sb.Append("   <IsSystem>true</IsSystem>");
            //sb.Append("   </QueryProject>");
            sb.Append(" </QueryProjectXML>");
            sb.Append(" <QueryCollectXML>");
            //sb.Append(" <QueryCollect>");
            //sb.Append(" <QueryCollectId></QueryCollectId>");
            //sb.Append(" <Name>Browse</Name>");
            //sb.Append(" <RefToTypeKey></RefToTypeKey>");
            //sb.Append(" <IsSystem>true</IsSystem>");
            //sb.Append(" <QueryCollectItems>");
            //sb.Append(" <QueryCollectItem>");
            //sb.Append(" <QueryCollectItemId> </QueryCollectItemId>");
            //sb.Append(" <QueryProjectId> </QueryProjectId>");
            //sb.Append(" <QueryName>Browse</QueryName>");
            //sb.Append(" <Text>Browse</Text>");
            //sb.Append(" <RefToTypeKey> </RefToTypeKey>");
            //sb.Append(" <OrderNumber>0</OrderNumber>");
            //sb.Append(" </QueryCollectItem>");
            //sb.Append(" </QueryCollectItems>");
            //sb.Append(" </QueryCollect>");
            sb.Append(" </QueryCollectXML>");
            sb.Append("</QueryConfiguration>");
            return sb.ToString();
        }


#region 20141004 modified by Dick for 保留原先寫法，重構此功能
        //private void SubQueryView(System.Windows.Forms.DataGridView GridView, string FullFileName, string EntityName,string Type) {
        //    XmlDocument doc = Tools.XmlTool.LoadXml(FullFileName);
        //    XmlNode root = doc.SelectSingleNode("QueryConfiguration");
        //    XmlNode QueryViewXML = root.SelectSingleNode("QueryViewXML/QueryView/QueryViewId");
        //    QueryViewXML.InnerXml = EntityName + "_" + Type;
        //    XmlNode RefToTypeKey = root.SelectSingleNode("QueryViewXML/QueryView/RefToTypeKey");
        //    RefToTypeKey.InnerXml = EntityName;
        //    XmlNode QueryViewColumns = root.SelectSingleNode("QueryViewXML/QueryView/QueryViewColumns");
        //    int count = 1;
        //    bool IsRemarkExit = false;
        //    QueryViewColumns.AppendChild(QueryViewColumnSpecil(doc, EntityName, EntityName + "Id", 0, false));//加入Id
        //    foreach(DataGridViewRow dr in GridView.Rows) {
        //        if(dr.Cells["Order"].Value != null) {
        //            if(dr.Cells["Order"].Value.ToString() == "-1") {
        //                IsRemarkExit = true;
        //            }
        //            else if(!dr.Cells["Order"].Value.ToString().Equals(string.Empty)) {
        //                QueryViewColumns.AppendChild(QueryViewColumn(dr, doc, EntityName));
        //                count++;
        //            }
        //        }
        //    }
        //    if(IsRemarkExit) {
        //        QueryViewColumns.AppendChild(QueryViewColumnSpecil(doc, EntityName, "Remark", count, true));
        //    }
        //    XmlNode QueryProjectId = root.SelectSingleNode("QueryProjectXML/QueryProject/QueryProjectId");
        //    QueryProjectId.InnerXml = EntityName + "_" + Type;
        //    XmlNode QueryProjectViewId = root.SelectSingleNode("QueryProjectXML/QueryProject/QueryViewId");
        //    QueryProjectViewId.InnerXml = EntityName + "_" + Type;
        //    XmlNode QueryProjectRefToTypeKey = root.SelectSingleNode("QueryProjectXML/QueryProject/RefToTypeKey");
        //    QueryProjectRefToTypeKey.InnerXml = EntityName;
        //    XmlNode QueryCollectId = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectId");
        //    QueryCollectId.InnerXml = EntityName + "_" + Type;
        //    XmlNode QueryCollectRefToTypeKey = root.SelectSingleNode("QueryCollectXML/QueryCollect/RefToTypeKey");
        //    QueryCollectRefToTypeKey.InnerXml = EntityName;
        //    XmlNode QueryCollectItemId = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectItems/QueryCollectItem/QueryCollectItemId");
        //    QueryCollectItemId.InnerXml = EntityName + "_" + Type + "_QueryCollectItem" + Guid.NewGuid().ToString();
        //    XmlNode QueryCollectQueryProjectId = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectItems/QueryCollectItem/QueryProjectId");
        //    QueryCollectQueryProjectId.InnerXml = EntityName + "_" + Type;
        //    XmlNode QueryCollectQueryRefToTypeKey = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectItems/QueryCollectItem/RefToTypeKey");
        //    QueryCollectQueryRefToTypeKey.InnerXml = EntityName;
        //    XmlNode TextTextQueryCollectQueryText = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectItems/QueryCollectItem/Text");
        //    TextTextQueryCollectQueryText.InnerXml = EntityName + "_" + Type;
        //    doc.Save(FullFileName);
        //}
#endregion

        /// <summary>
        /// 20141004 modified by Dick 重構此寫法。
        /// </summary>
        /// <param name="GridView"></param>
        /// <param name="FullFileName"></param>
        /// <param name="EntityName"></param>
        /// <param name="Type"></param>
        private void SubQueryView(System.Windows.Forms.DataGridView GridView, string FullFileName, string EntityName,QueryViewCondition Condition) {
            string PageName = Condition.BrowseName;
            string Type = Condition.Type;
            XmlDocument doc = FileTool.XmlFile.LoadXml(FullFileName);
            XmlNode root = doc.SelectSingleNode("QueryConfiguration");
            XmlNode QueryViewXML = root.SelectSingleNode("QueryViewXML");
            XmlElement QueryView = doc.CreateElement("QueryView");
            XmlElement QueryViewId = doc.CreateElement("QueryViewId");
            QueryViewId.InnerXml = EntityName + "_" + PageName;
            XmlElement Name = doc.CreateElement("Name");
            #region 20141224 modified by Dick for #10
            //Name.InnerXml = Type;
            Name.InnerXml = QueryViewId.InnerXml;
            #endregion
            QueryViewXML.AppendChild(QueryView);
            QueryView.AppendChild(QueryViewId);
            QueryView.AppendChild(Name);
            XmlElement RefToTypeKey = doc.CreateElement("RefToTypeKey");
            RefToTypeKey.InnerXml = EntityName;
            QueryView.AppendChild(RefToTypeKey);
            //XmlNode QueryViewColumns = root.SelectSingleNode("QueryViewXML/QueryView/QueryViewColumns");
            XmlElement QueryViewColumns = doc.CreateElement("QueryViewColumns");
            XmlElement ViewIsSystem = doc.CreateElement("IsSystem");
            ViewIsSystem.InnerXml = "true";
            QueryView.AppendChild(ViewIsSystem);

            int count = 1;
            bool IsRemarkExit = false;
            QueryViewColumns.AppendChild(QueryViewColumnSpecil(doc, EntityName, EntityName + "Id", 0, false));//加入Id
            foreach(DataGridViewRow dr in GridView.Rows) {
                if(dr.Cells["Order"].Value != null) {
                    if(dr.Cells["Order"].Value.ToString() == "-1") {
                        IsRemarkExit = true;
                    }
                    else if(!dr.Cells["Order"].Value.ToString().Equals(string.Empty)) {
                        QueryViewColumns.AppendChild(QueryViewColumn(dr, doc, EntityName));
                        count++;
                    }
                }
            }
            if(IsRemarkExit) {
                QueryViewColumns.AppendChild(QueryViewColumnSpecil(doc, EntityName, "Remark", count, true));
            }
            QueryView.AppendChild(QueryViewColumns);
            XmlNode  QueryProjectXML  = root.SelectSingleNode("QueryProjectXML");
            XmlElement QueryProject = doc.CreateElement("QueryProject");
            QueryProjectXML.AppendChild(QueryProject);
            XmlElement QueryProjectId = doc.CreateElement("QueryProjectId");
            //XmlNode QueryProjectId = root.SelectSingleNode("QueryProjectXML/QueryProject/QueryProjectId");
            QueryProjectId.InnerXml = EntityName + "_" + PageName;
            QueryProject.AppendChild(QueryProjectId);            
            //XmlNode QueryProjectViewId = root.SelectSingleNode("QueryProjectXML/QueryProject/QueryViewId");
            //QueryProjectViewId.InnerXml = EntityName + "_" + Type;
            XmlElement QueryProjectViewId = doc.CreateElement("QueryViewId");
            QueryProjectViewId.InnerXml = QueryViewId.InnerXml;
            QueryProject.AppendChild(QueryProjectViewId);
            //XmlNode QueryProjectRefToTypeKey = root.SelectSingleNode("QueryProjectXML/QueryProject/RefToTypeKey");
            XmlElement QueryProjectRefToTypeKey = doc.CreateElement("RefToTypeKey");
            QueryProjectRefToTypeKey.InnerXml = EntityName;
            QueryProject.AppendChild(QueryProjectRefToTypeKey);
            XmlElement ProjectName = doc.CreateElement("Name");
            ProjectName.InnerXml = PageName;
            QueryProject.AppendChild(ProjectName);
            XmlElement ProjectType = doc.CreateElement("ProjectType");
            ProjectType.InnerXml = Type == "Browse" ? "2" : "1";
            QueryProject.AppendChild(ProjectType);
            XmlElement ProjectIsSystem = doc.CreateElement("IsSystem");
            ProjectIsSystem.InnerXml = "true";
            QueryProject.AppendChild(ProjectIsSystem);

            #region 20141006 add by Dick for ProjectCondition            
            StringBuilder sb =new StringBuilder();
            List<Condition> li = Condition.ConditionList;
            foreach(Condition cond in li )
            {
                sb.Append(string.Format("\r\n &lt;QueryItem ItemType=\"Dcms.Common.UI.QueryItem\" ID=\"{0}\" ParentID=\"{1}\" ParameterName=\"{2}\" DbType=\"{3}\" FieldName=\"{4}\" Value1=\"{5}\" Value2=\"{6}\" Symbol=\"{7}\" /&gt; "
                ,cond.ID,cond.ParentID,cond.ParameterName,cond.Type,cond.Field,cond.Conditon1,cond.Conditon2,cond.sybel));
            }
            if(sb.Length > 0) {
                sb.Append("\r\n");
                XmlElement ConditionXML = doc.CreateElement("ConditionXML");
                ConditionXML.InnerXml = sb.ToString();
                QueryProject.AppendChild(ConditionXML);
            }
            #endregion



            XmlElement ProjectText = doc.CreateElement("Text");
            ProjectText.InnerXml = PageName;
            QueryProject.AppendChild(ProjectText);

            XmlNode QueryCollectXML = root.SelectSingleNode("QueryCollectXML");
            QueryCollectXML = CreateCollectByType(EntityName,Type, QueryCollectXML,doc);
            XmlNodeList QueryCollects = QueryCollectXML.SelectNodes("QueryCollect");
            //XmlNode QueryCollectId = null;
            XmlNode QueryCollect = null;
            foreach(XmlNode node in QueryCollects)
            {
                XmlNode NodeName = node.SelectSingleNode("Name");
                if(NodeName.InnerXml.Equals(Type)) {
                    QueryCollect = node;
                }
            }



            XmlNode QueryCollectItems = QueryCollect.SelectSingleNode("QueryCollectItems");
            int Order = QueryCollectItems.ChildNodes.Count;
            XmlElement QueryCollectItem = doc.CreateElement("QueryCollectItem");
            QueryCollectItems.AppendChild(QueryCollectItem);
            XmlElement QueryCollectItemId = doc.CreateElement("QueryCollectItemId");
            QueryCollectItemId.InnerXml = EntityName + "_" + PageName + "_QueryCollectItem" + Guid.NewGuid().ToString();
            QueryCollectItem.AppendChild(QueryCollectItemId);
            //XmlNode QueryCollectItemId = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectItems/QueryCollectItem/QueryCollectItemId");
            //QueryCollectItemId.InnerXml = EntityName + "_" + Type + "_QueryCollectItem" + Guid.NewGuid().ToString();
            XmlElement QueryCollectQueryProjectId = doc.CreateElement("QueryProjectId");
            QueryCollectQueryProjectId.InnerXml = EntityName + "_" + PageName;
            QueryCollectItem.AppendChild(QueryCollectQueryProjectId);
            //XmlNode QueryCollectQueryProjectId = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectItems/QueryCollectItem/QueryProjectId");
            //QueryCollectQueryProjectId.InnerXml = EntityName + "_" + Type;
            XmlElement QueryCollectQueryName = doc.CreateElement("QueryName");
            QueryCollectQueryName.InnerXml = PageName;
            QueryCollectItem.AppendChild(QueryCollectQueryName);
            XmlElement QueryCollectText = doc.CreateElement("Text");
            QueryCollectText.InnerXml = EntityName + "_" + PageName;
            QueryCollectItem.AppendChild(QueryCollectText);
            XmlElement QueryCollectQueryRefToTypeKey = doc.CreateElement("RefToTypeKey");
            QueryCollectQueryRefToTypeKey.InnerXml = EntityName;
            QueryCollectItem.AppendChild(QueryCollectQueryRefToTypeKey);

            XmlElement QueryCollectOrderNumber = doc.CreateElement("OrderNumber");
            QueryCollectOrderNumber.InnerXml = Order.ToString();
            QueryCollectItem.AppendChild(QueryCollectOrderNumber);

            //XmlNode QueryCollectQueryRefToTypeKey = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectItems/QueryCollectItem/RefToTypeKey");
            //QueryCollectQueryRefToTypeKey.InnerXml = EntityName;
            //XmlNode TextTextQueryCollectQueryText = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectItems/QueryCollectItem/Text");
            //TextTextQueryCollectQueryText.InnerXml = EntityName + "_" + Type;
            doc.Save(FullFileName);
        }



        /// <summary>
        /// 20141004 add by Dick for 建立Collect 類別 Browse || Select 
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="Type"></param>
        /// <param name="root"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        private XmlNode CreateCollectByType(string Entity,string Type,XmlNode root,XmlDocument doc ) {
            XmlNodeList Names = root.SelectNodes("QueryCollect/Name");
            bool IsBrowse = false;           
            foreach(XmlNode node in Names)
            {
                if(node.InnerXml.Equals(Type)) {
                    IsBrowse = true;
                }               
            }          
            if(!IsBrowse) {
                    XmlElement QueryCollect = doc.CreateElement("QueryCollect");
                    root.AppendChild(QueryCollect);
                    XmlElement QueryCollectId = doc.CreateElement("QueryCollectId");
                    QueryCollectId.InnerXml = Entity + "_" + Type;
                    QueryCollect.AppendChild(QueryCollectId);
                    XmlElement name = doc.CreateElement("Name");
                    name.InnerXml = Type;
                    QueryCollect.AppendChild(name);
                    XmlElement RefToTypeKey = doc.CreateElement("RefToTypeKey");
                    RefToTypeKey.InnerXml = Entity;
                    QueryCollect.AppendChild(RefToTypeKey);
                    XmlElement QueryCollectItems = doc.CreateElement("QueryCollectItems");
                    QueryCollect.AppendChild(QueryCollectItems);
                    XmlElement IsSystem = doc.CreateElement("IsSystem");
                    IsSystem.InnerXml = "true";
                    QueryCollect.AppendChild(IsSystem);
                }
                     
                return root;
        }


        /// <summary>
        /// 20141003 add by Dick 多頁籤
        /// </summary>
        /// <param name="pFileName"></param>
        /// <param name="pContext"></param>
        /// <param name="GridView"></param>
        public void CreateQueryView(string pFileName, string pContext, List<DataGridView> GridViewList, Dictionary<string, QueryViewCondition> DicQueryView) {
            string pPath = Parent + Path.DirectorySeparatorChar + "DigiWin.HR.CustomBusinessImplement" + Path.DirectorySeparatorChar + "Configuration" + Path.DirectorySeparatorChar + "Query" + Path.DirectorySeparatorChar + "Case";
            string FullFileName = pPath + Path.DirectorySeparatorChar + pFileName + ".xml";
            string EntityName = pFileName;
            if(File.Exists(FullFileName)) {
                foreach(DataGridView GirdView in GridViewList)
                {
                    if(DicQueryView.ContainsKey(GirdView.Name)) {
                        QueryViewCondition QueryView = DicQueryView[GirdView.Name];
                        SubQueryView(GirdView, FullFileName, EntityName, QueryView);
                    }
                }
            }
        }


        /// <summary>
        /// 20140815 建立單檔UI
        /// 20150109 Modifide by Dick for 加入模組ProviderName #14
        /// </summary>
        /// <param name="pEntityName"></param>
        public void CreateentityNoDetailBrowseEditViewV5(string pEntityName, DataGridView dt,int Mode,string pSourceFile,string ModuleProviderName) {        
           
            #region 加入控件插入
            List<SubUIControl> li = AddControlerInit(dt, pEntityName);
            StringBuilder SBParameter = new StringBuilder();
            StringBuilder SBNewControl = new StringBuilder();
            StringBuilder SBLayout = new StringBuilder();
            StringBuilder SBContext = new StringBuilder();
            StringBuilder SBAdd = new StringBuilder();
            bool HasRemark = false;           
            //起始座標
            int x = 30;
            int y = 0;

            ///排列邏輯還在想
            int count = 0;
            foreach(SubUIControl control in li.OrderBy(o => o.Order)) {

                #region  20140905 add by Dick 針對備註另外處理
                if(control.Order == -1) {
                    HasRemark = true;
                    SBParameter.AppendLine(control.Declare);

                    SBNewControl.AppendLine(control.NewControl);

                    SBLayout.AppendLine(control.Layout);

                    SBContext.AppendLine(control.Context);
                
                    continue;
                }
                #endregion

                #region 20140905 add by Dick 對控件座標進行做調整
                if(count % Mode == 0) {
                    x = 30;
                    if(Mode == 2) {
                        y += 30;
                    }
                    if(Mode == 3) {
                        y += 60;
                    }
                    if(Mode == 4) {
                        y += 80;
                    }
                }
                #endregion

                #region  20140904 add by Dick 加入控件
                SBParameter.AppendLine(control.Declare);
              
                SBParameter.AppendLine(control.LabelDeclare);
              
                    SBNewControl.AppendLine(control.NewControl);
                    
                    SBNewControl.AppendLine(control.LabelNewControl);
              
                    SBLayout.AppendLine(control.Layout);             
                    if(!string.IsNullOrEmpty(control.LabelContext)) {
                        SBContext.AppendLine(control.LabelContext.Replace("$X", x.ToString()).Replace("$Y", y.ToString()));                    
                    }
                    SBContext.AppendLine(control.Context.Replace("$X", (x + 85).ToString()).Replace("$Y", y.ToString()));
                             
                    if(Mode == 2) {
                        x += 350;
                    }
                    if(Mode==3) {
                        x += 100;
                    }
                    SBAdd.AppendLine(control.LabelAdd);
                    SBAdd.AppendLine(control.GroupAdd);                  
                    x += 130;
                #endregion
                    count++;
            }

            #endregion
            StringBuilder sb  =new StringBuilder();
            sb = AnalysisUI(pEntityName, pSourceFile, SBParameter, SBNewControl, SBLayout, SBContext, SBAdd, HasRemark, ModuleProviderName);
            string SaveFile = Parent + Path.DirectorySeparatorChar + "DigiWin.HR.CustomUI" + Path.DirectorySeparatorChar + pEntityName + ".cs";
            FileTool.Files.WritFile(sb, SaveFile);
           
        }

       /// <summary>
       /// 20141229 add by Dick for 實體UI分析功能。
       /// 20150109 modified by Dick for 加入實體模組的ProviderName
       /// </summary>
       /// <param name="pEntityName"></param>
       /// <param name="pSourceFile"></param>
       /// <param name="line"></param>
       /// <param name="SBParameter"></param>
       /// <param name="SBNewControl"></param>
       /// <param name="SBLayout"></param>
       /// <param name="SBContext"></param>
       /// <param name="SBAdd"></param>
       /// <param name="HasRemark"></param>
       /// <param name="AddGroupBox"></param>
       /// <returns></returns>
        private static StringBuilder AnalysisUI(string pEntityName, string pSourceFile, StringBuilder SBParameter, StringBuilder SBNewControl, StringBuilder SBLayout, StringBuilder SBContext, StringBuilder SBAdd, bool HasRemark,string ModuleProviderName)
        {
            string line = string.Empty;
            string AddGroupBox = string.Empty;
            string SourcePath = System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "SampleFile\\" + pSourceFile;
            StreamReader sr = new StreamReader(SourcePath,Encoding.Default);
            StringBuilder sb = new StringBuilder();
            #region 每行撈取分析
            while ((line = sr.ReadLine()) != null)
            {
                if (line.IndexOf("this.entityEditerView1 = new Dcms.HR.UI.EntityEditerView();") != -1)
                {
                    sb.Append("\r\n");
                    string temp = "            ((Label)(this.Controls.Find(\"labDoc\",true)[0])).Text = ((Label)(this.Controls.Find(\"labDoc\",true)[0])).Text  + \"*\";";
                    sb.AppendLine(temp);                  
                }
                if (line.IndexOf("XTestDate") != -1)
                {
                    line = line.Replace("XTestDate", DateTime.Now.ToString("yyyy/MM/dd"));
                }
                if (line.IndexOf("//<description>description</description>") != -1)
                {
                    line = line.Replace("//<description>description</description>", "//<description>UI編輯畫面</description>");
                }
                if (line.IndexOf(" public class EntityEditerView : HREditerView {") != -1)
                {
                    line = line.Replace("Entity", pEntityName);
                }
                if (line.IndexOf("EntityEditerView()") != -1)
                {
                    line = line.Replace("Entity", pEntityName);
                }
                if (line.IndexOf("this.entityBindingSource") != -1)
                {
                    line = line.Replace("entity", pEntityName.ToLower());
                }
                if (line.IndexOf("private BindingSource entityBindingSource;") != -1)
                {
                    line = line.Replace("entity", pEntityName.ToLower());
                }
                if (line.IndexOf("Dcms.HR.DataEntities.Entity") != -1)
                {
                    line = line.Replace("Entity", pEntityName);
                }
                if (line.IndexOf("return Factory.GetService<IEntityService>();") != -1)
                {
                    line = line.Replace("IEntityService", "I" + pEntityName.Remove(0, 1) + "ServiceX");
                }
                if (line.IndexOf("browseWindow.Name = GetBrowseWindowName();") != -1)
                {
                    sb.Append("\r\n");
                    sb.AppendLine("            browseWindow.UsingExtraText = true;");
                }
                if (line.IndexOf("Resources.EntityDisplayName;") != -1)
                {
                    line = line.Replace("Resources.Entity", "ResourcesForCase." + pEntityName);
                }
                if (line.IndexOf("entity") != -1)
                {
                    line = line.Replace("entity", pEntityName.ToLower());
                }
                if (line.IndexOf("Entity") != -1)
                {
                    line = line.Replace("Entity", pEntityName);
                }
                if (line.IndexOf("//ResourceExtend") != -1)
                {
                    line = line.Replace("//ResourceExtend", "System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(" + pEntityName + "EditerView));");
                }
                #region 20150109 add by Dick for 修改模組Provider Mame #14
                if (line.IndexOf("ModuleNameProvider") != -1)
                {
                    line = line.Replace("ModuleNameProvider", ModuleProviderName);
                }
                #endregion              

                if (SBParameter.Length > 0)
                {
                    if (line.IndexOf("//ParameterExtend") != -1)
                    {
                        line = line.Replace("//ParameterExtend", "");
                        sb.Append(SBParameter);
                    }
                }
                if (SBNewControl.Length > 0)
                {
                    if (line.IndexOf("//NewControlExtend") != -1)
                    {
                        line = line.Replace("//NewControlExtend", "");
                        sb.Append(SBNewControl);                    
                    }
                }
                if (SBLayout.Length > 0)
                {
                    if (line.IndexOf("//LayoutExtend") != -1)
                    {
                        line = line.Replace("//LayoutExtend", "");
                        sb.Append(SBLayout);
                    }
                }
                if (SBContext.Length > 0)
                {
                    if (line.IndexOf("//ContextExtend") != -1)
                    {
                        line = line.Replace("//ContextExtend", "");
                        sb.Append(SBContext);
                    }
                }
                if (SBAdd.Length > 0)
                {
                    if (line.IndexOf("//groupBoxExtend") != -1)
                    {
                        line = line.Replace("//groupBoxExtend", "");
                        sb.Append(SBAdd);
                    }
                }
                if (HasRemark)
                {
                    if (line.IndexOf("this.GeneralTabPage.Controls.Add(this.groupBox1);") != -1)
                    {

                        AddGroupBox = "\r\n            this.GeneralTabPage.Controls.Add(this.groupBox2);";
                        AddGroupBox += "\r\n" + line;
                        line = AddGroupBox;
                    }
                }
                sb.AppendLine(line);
            }
            #endregion
            sr.Close();          
            return sb;
        }

       

        /// <summary>
        /// 20140818 add by Dick 生出控件出來，讓後面可以加入控件
        /// </summary>
        /// <param name="dt"></param>
        public List<SubUIControl> AddControlerInit(System.Windows.Forms.DataGridView dv, string EntityName) {
            List<SubUIControl> li = new List<SubUIControl>();
            foreach(DataGridViewRow dr in dv.Rows) {
                if(dr.Cells["UIOrder"] != null) {
                    if(!string.IsNullOrEmpty(dr.Cells["UIOrder"].ToString())) {
                        if(dr.Cells["UIOrder"].Value != null) {
                            int order =0;
                            if(int.TryParse(dr.Cells["UIOrder"].Value.ToString(), out order)) {
                                if(!string.IsNullOrEmpty(dr.Cells["Type"].Value.ToString())) {
                                    SubUIControl control = null;
                                    switch(dr.Cells["Type"].Value.ToString().ToLower()) {
                                        case"ntext":
                                            if(order == -1) {
                                                control = new SubUIControl();
                                                control.Order = -1;
                                                control.Name = dr.Cells["Parameter"].Value.ToString();                                               
                                                control.Declare = "\r\n            private GroupBox groupBox2;";                                                
                                                control.NewControl = "\r\n            this.groupBox2 = new System.Windows.Forms.GroupBox();";
                                                control.NewControl += "\r\n            this." + control.Name + "DcmsMemoEdit = new Dcms.Common.UI.DcmsMemoEdit();";
                                                control.Context += "            //";
                                                control.Context += "\r\n            // groupBox2";
                                                control.Context += "\r\n            // ";
                                                control.Context += "\r\n            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)";
                                                control.Context += "\r\n            | System.Windows.Forms.AnchorStyles.Left)";
                                                control.Context += "\r\n            | System.Windows.Forms.AnchorStyles.Right)));";
                                                control.Context += "\r\n            resources.ApplyResources(this.groupBox2, \"groupBox2\");";
                                                control.Context += "\r\n            this.groupBox2.Controls.Add(this." + control.Name + "DcmsMemoEdit);";
                                                control.Context += "\r\n            this.errorProvider.SetError(this.groupBox2, resources.GetString(\"groupBox2.Error\"));";
                                                control.Context += "\r\n            this.errorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject(\"groupBox2.IconAlignment\"))));";
                                                control.Context += "\r\n            this.errorProvider.SetIconPadding(this.groupBox2, ((int)(resources.GetObject(\"groupBox2.IconPadding\"))));";
                                                control.Context += "\r\n            this.groupBox2.Name = \"groupBox2\";";
                                                control.Context += "\r\n            this.groupBox2.Location = new System.Drawing.Point(10, 261);";
                                                control.Context += "\r\n            this.groupBox2.Size = new System.Drawing.Size(803, 226);";
                                                control.Context += "\r\n            this.groupBox2.TabStop = false;";
                                                control.Context += "\r\n            this.groupBox2.Text = \"Remark\";";                                                
                                                //control = ControlsSetting(EntityName, dr, control, "DcmsMemoEdit");
                                                control.Declare += "\r\n            private DcmsMemoEdit " + control.Name + "DcmsMemoEdit;";
                                                control.Layout = "\r\n            this.groupBox2.SuspendLayout();";
                                                control.Layout = "\r\n            ((System.ComponentModel.ISupportInitialize)(this." + control.Name + "DcmsMemoEdit.Properties)).BeginInit();";
                                                control.Context += "\r\n            //";
                                                control.Context += "\r\n            // " + control.Name + "DcmsMemoEdit";
                                                control.Context += "\r\n            // ";
                                                control.Context += "\r\n            this." + control.Name + "DcmsMemoEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)";
                                                control.Context += "\r\n            | System.Windows.Forms.AnchorStyles.Left)";
                                                control.Context += "\r\n            | System.Windows.Forms.AnchorStyles.Right)));";
                                                control.Context += "\r\n            this." + control.Name + "DcmsMemoEdit.DataBindings.Add(new System.Windows.Forms.Binding(\"Text\", this."+EntityName.ToLower()+"BindingSource, \"" + control.Name + "\", true));";
                                                control.Context += "\r\n            this." + control.Name + "DcmsMemoEdit.Location = new System.Drawing.Point(6, 21);";
                                                control.Context += "\r\n            this." + control.Name + "DcmsMemoEdit.Size = new System.Drawing.Size(180, 70);";
                                                control.Context += "\r\n             this." + control.Name + "DcmsMemoEdit.Properties.AccessibleDescription = resources.GetString(\"" + control.Name + "DcmsMemoEdit.Properties.AccessibleDescription\");";
                                                control.Context += "\r\n             this." + control.Name + "DcmsMemoEdit.Properties.AccessibleName = resources.GetString(\"" + control.Name + "DcmsMemoEdit.Properties.AccessibleName\");";
                                                control.Context += "\r\n             this." + control.Name + "DcmsMemoEdit.Properties.NullValuePrompt = resources.GetString(\"" + control.Name + "DcmsMemoEdit.Properties.NullValuePrompt\");";
                                                control.Context += "\r\n             this." + control.Name + "DcmsMemoEdit.Properties.NullValuePromptShowForEmptyValue = ((bool)(resources.GetObject(\"" + control.Name + "DcmsMemoEdit.Properties.NullValuePromptShowForEmptyValue\")));";
                                            }
                                            break;
                                        case "int":                                            
                                            break;
                                        case"bool":
                                            control = ControlsSetting(EntityName, dr, control, "DcmsCheckEdit");
                                            control.Context += "\r\n             this.dcmsCheckEdit" + control.Name + ".Properties.Caption =\"" + control.Name + "\";";
                                            control.Layout = "\r\n            ((System.ComponentModel.ISupportInitialize)(this.dcmsCheckEdit" + control.Name + ".Properties)).EndInit();\r\n";
                                            break;
                                        case "decimal":
                                            control = ControlsSetting(EntityName, dr, control, "DcmsCalcEdit");
                                            control.Context += "\r\n            this." + control.Name + "DcmsCalcEdit.DataBindings.Add(new System.Windows.Forms.Binding(\"Value\", this." + EntityName.ToLower() + "BindingSource, \"" + control.Name + "\", true));";
                                            control.Context += "\r\n            this." + control.Name + "DcmsCalcEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {";
                                            control.Context += "\r\n            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject(\"" + control.Name + "DcmsCalcEdit.Properties.Buttons\"))))});";
                                            control.Context += "\r\n            this." + control.Name + "DcmsCalcEdit.Properties.Mask.UseMaskAsDisplayFormat = ((bool)(resources.GetObject(\"" + control.Name + "DcmsCalcEdit.Properties.Mask.UseMaskAsDisplayFormat\")));";			
                                            control.Layout = "             ((System.ComponentModel.ISupportInitialize)(this." + control.Name + "DcmsCalcEdit.Properties)).EndInit();\r\n";
                                            break;
                                        case "guid":
                                            if(!string.IsNullOrEmpty(dr.Cells["ReferenceProperty"].Value.ToString())) {
                                                control = ControlsSetting(EntityName, dr, control, "HRSelectControl");
                                                control.Context += "            this.hRSelectControlDepartmentId.TypeKey =\"" + dr.Cells["ReferenceProperty"].Value.ToString() + "\";";
                                            }
                                            break;
                                        case "string":
                                            if(!string.IsNullOrEmpty(dr.Cells["ReferenceProperty"].Value.ToString())) {
                                                if(dr.Cells["ReferenceProperty"].Value.ToString().ToLower().Equals("codeinfo")) {
                                                    control = ControlsSetting(EntityName, dr, control, "HRPickList");
                                                    control.Context += "\r\n            this." + control.Name + "HRPickList.AutoDisplayText = true;";
                                                    control.Context += "             this." + control.Name + "HRPickList.BackColor = System.Drawing.SystemColors.Control;\r\n";
                                                    control.Context += "            this." + control.Name + "HRPickList.ControlDataSource = null;";
                                                }
                                                else {
                                                    control = ControlsSetting(EntityName, dr, control, "HRSelectControl");
                                                    control.Context += "            this.hRSelectControl" + control.Name + ".TypeKey =\"" + dr.Cells["ReferenceProperty"].Value.ToString() + "\";";
                                                }
                                            }
                                            else {
                                                control = ControlsSetting(EntityName, dr, control, "DcmsTextEdit");
                                                control.Context += "            this.dcmsTextEdit" + control.Name + " .DataBindings.Add(new System.Windows.Forms.Binding(\"Text\", this." + EntityName.ToLower() + "BindingSource, \"" + control.Name + "\", true));";
                                                control.Context += "            this.dcmsTextEdit" + control.Name + ".Properties.MaxLength = 200;";
                                            }
                                            break;
                                        case "datetime":
                                            control = ControlsSetting(EntityName, dr, control, "DcmsDateEdit");
                                            control.Layout = "            ((System.ComponentModel.ISupportInitialize)(this.dcmsDateEdit" + control.Name + ".Properties.VistaTimeProperties)).BeginInit();\r\n";
                                            control.Context += "            this.dcmsDateEdit" + control.Name + ".DataBindings.Add(new System.Windows.Forms.Binding(\"EditValue\", this." + EntityName.ToLower() + "BindingSource, \"" + control.Name + "\", true));\r\n";
                                            control.Context += "            this.dcmsDateEdit" + control.Name + ".Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {";
                                            control.Context += "\r\n            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject(\"dcmsDateEdit" + control.Name + ".Properties.Buttons\"))))});\r\n";
                                            control.Context += "            this.dcmsDateEdit" + control.Name + ".Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {";
                                            control.Context += "\r\n            new DevExpress.XtraEditors.Controls.EditorButton()});\r\n";
                                            break;
                                    }
                                    if(control != null) {
                                        if(dr.Cells["Type"].Value.ToString().ToLower() != "bool" & control.Order!=-1) {
                                            //加入Label
                                            string temp = control.Name.ToLower().IndexOf("x") != -1 ? control.Name.Substring(1, control.Name.Length-1) : control.Name;
                                            control.LabelName = control.Name + "Label";
                                            control.LabelDeclare += "\r\n        System.Windows.Forms.Label  " + control.Name + "Label1;";
                                            control.LabelNewControl += "\r\n            " + control.Name + "Label1 = new System.Windows.Forms.Label();";
                                            control.LabelAdd = "\r\n            this.groupBox1.Controls.Add(" + control.Name + "Label1);";
                                            control.LabelContext += "\r\n            //";
                                            control.LabelContext += "\r\n            // " + control.LabelName;
                                            control.LabelContext += "\r\n            // ";
                                            control.LabelContext += "\r\n            " + control.LabelName + ".AutoSize = true;";
                                            control.LabelContext += "\r\n            " + control.LabelName + ".Name = \"" + control.LabelName + "\";";
                                            control.LabelContext += "\r\n            " + control.LabelName + ".Size = new System.Drawing.Size(29, 12);";
                                            control.LabelContext += "\r\n            " + control.LabelName + ".TabIndex = 2;";
                                            control.LabelContext += "\r\n            " + control.LabelName + ".Text = \"" + control.Name + ":\";";
                                            control.LabelContext += "\r\n            " + control.LabelName + ".TextAlign = System.Drawing.ContentAlignment.MiddleRight;\r\n";
                                            control.LabelContext += "\r\n            " + control.LabelName + ".Location = new System.Drawing.Point($X, $Y);";
                                            if(dr.Cells["Necessary"].Value != null) {
                                                if(Convert.ToBoolean(dr.Cells["Necessary"].Value)) {
                                                    control.LabelContext += "\r\n            " + control.LabelName + ".ForeColor = System.Drawing.Color.Red;";
                                                }
                                            }                                           
                                        }
                                        li.Add(control);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return li;
        }




        /// <summary>
        /// 20140904 動態件利控件
        /// </summary>
        /// <param name="EntityName"></param>
        /// <param name="dr"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        private static SubUIControl ControlsSetting(string EntityName, DataGridViewRow dr, SubUIControl control,string ControlType) {
            string temp = ControlType.Substring(0, 1).ToLower() + ControlType.Substring(1, ControlType.Length - 1);
            control = new SubUIControl();
            control.Name = dr.Cells["Parameter"].Value.ToString();
            control.ControlFullName = " this." + temp + control.Name;
            control.Order = Convert.ToInt32(dr.Cells["UIOrder"].Value);
            control.Declare = "        private " + ControlType + " "+temp + control.Name + ";";
            if(ControlType.Equals("HRSelectControl")) { ///SelectControl 命名空間不一樣
                control.NewControl = "            this." + temp + control.Name + " = new " + ControlType + "();\r\n";
            } 
            else {
                if (ControlType == "HRPickList")
                {
                    control.NewControl = "            this." + temp + control.Name + " = new " + ControlType + "();\r\n";
                }
                else
                {
                    control.NewControl = "            this." + temp + control.Name + " = new Dcms.Common.UI." + ControlType + "();\r\n";
                }
                control.Layout += "            ((System.ComponentModel.ISupportInitialize)(this." + temp + control.Name + ".Properties)).BeginInit();";            
            }         
            control.GroupAdd = "\r\n            this.groupBox1.Controls.Add(this."+temp + control.Name + ");";
            control.Context = "             //\r\n";
            control.Context += "            // "+temp + control.Name + "\r\n";
            control.Context += "            //\r\n";
            control.Context += "            resources.ApplyResources(this."+temp + control.Name + ", \""+temp+"" + control.Name + "\");\r\n";
            control.Context += "            this."+temp + control.Name + ".Name = \""+temp + control.Name + "\";\r\n";
            control.Context += "            this."+temp + control.Name + ".Size = new System.Drawing.Size(130, 26);\r\n";
            control.Context += "            this."+temp + control.Name + ".Location = new System.Drawing.Point( $X, $Y);\r\n ";
            return control;
        }


        /// <summary>
        /// 20140815 add by Dick for DisPlayName 加入多語系
        ///  20141223 modified by Dick for 修改會重複加入多語系問題 #4 
        /// </summary>
        /// <param name="pString"></param>
        /// <param name="FileName"></param>
        public void AddResourceRow(string pDirectory, string pString, string FileName, bool IsAppend)
        {
            string SaveFile = Parent + Path.DirectorySeparatorChar + pDirectory + Path.DirectorySeparatorChar + "Properties" + Path.DirectorySeparatorChar + FileName + ".resx";          
            string[] spl = pString.Split('\t');
            if(spl.Length > 2) {
                AddResource(SaveFile, spl);
                if(IsAppend) {
                    string DesignerFile = Parent + Path.DirectorySeparatorChar + pDirectory + Path.DirectorySeparatorChar + "Properties" + Path.DirectorySeparatorChar + FileName + ".Designer.cs";
                    List<string> ExistList = this.GetResoutList(DesignerFile); //#4
                    StringBuilder sb = new StringBuilder();
                    using(StreamReader reader = new StreamReader(DesignerFile)) {
                        string line = string.Empty;
                        while((line = reader.ReadLine()) != null) {
                            sb.Append(line);
                            sb.Append("\r\n");
                            if(line.IndexOf("resourceCulture = value;") != -1)
                            {
                                #region 20141223  已存在的就跳過 #4
                                if (ExistList.Contains(spl[0]))
                                {
                                    continue;
                                }
                                #endregion
                               
                                sb.Append(reader.ReadLine());
                                sb.Append("\r\n");
                                sb.Append(reader.ReadLine());
                                sb.Append("\r\n");
                                sb.Append(" ");
                                sb.Append("\r\n");
                                sb.Append("        /// <summary>\r\n");
                                sb.Append(string.Format("        ///   查詢類似 {0} 的當地語系化字串。\r\n", spl[2]));
                                sb.Append("        /// <summary>\r\n");
                                sb.Append(string.Format("        public static string {0} ", spl[0]));
                                sb.Append("{\r\n");
                                sb.Append("            get {\r\n");
                                sb.Append(string.Format("                return ResourceManager.GetString(\"{0}\", resourceCulture);\r\n", spl[0]));
                                sb.Append("            }\r\n");
                                sb.Append("        }\r\n");
                                sb.Append(" ");
                                sb.Append("\r\n");
                            }
                        }
                    }
                    //存檔
                    if(sb.Length > 0) {
                        using(StreamWriter sw = new StreamWriter(DesignerFile)) {
                            sw.Write(sb.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 20140816 add by Dick for 加入resx
        /// </summary>
        /// <param name="SaveFile"></param>
        /// <param name="spl"></param>
        public static void AddResource(string SaveFile, string[] spl) {
            XmlDocument doc = FileTool.XmlFile.LoadXml(SaveFile);
            XmlNode root = doc.SelectSingleNode("root");
            XmlElement element = doc.CreateElement("data");
            element.SetAttribute("name", spl[0]);
            element.SetAttribute("xml:space", "preserve");
            XmlElement value = doc.CreateElement("value");
            if(spl[1].Length > 0) {
                if(spl[1].Substring(0, 1) == "X") {
                    value.InnerText = spl[1].Remove(0, 1);
                }
                else {
                    value.InnerText = spl[1];
                }
            }
            else {
                value.InnerText = spl[1];
            }
            element.AppendChild(value);
            XmlElement comment = doc.CreateElement("comment");
            comment.InnerText = spl[2];
            element.AppendChild(comment);
            root.AppendChild(element);
            doc.Save(SaveFile);
        }

        /// <summary>
        /// QueryView 的多語系         
        /// </summary>
        public void AddQueryView(DataTable dt, string pDirectory, string[] pContext, string FileName, bool IsAppend) {
            string SaveFile = Parent + Path.DirectorySeparatorChar + pDirectory + Path.DirectorySeparatorChar + "Properties" + Path.DirectorySeparatorChar + FileName + ".resx";          
            List<string> QueryList = new List<string>();
            foreach(DataRow dr in dt.Rows) {
                if(!string.IsNullOrEmpty(dr["Order"].ToString())) {
                    QueryList.Add(dr["Parameter"].ToString());
                }
            }
            foreach(string line in pContext) {
                foreach(string str in QueryList) {
                    if(line.IndexOf(str) != -1) {
                        AddResourceRow(pDirectory, line, FileName, IsAppend);
                    }
                }
            }
        }
        
       /// <summary>
       /// 20141223 add by Dick for 修改會重複加入多語系問題 #4   
       /// </summary>
       /// <param name="pSaveFile"></param>
       /// <returns></returns>
        public List<string> GetResoutList(string pSaveFile)
        {
            List<string> result = new List<string>();
            using (StreamReader reader = new StreamReader(pSaveFile))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.IndexOf("public static string") != -1)
                    {
                        string temp = line.Replace("public static string", "").Replace("{","").Trim();
                        if (!result.Contains(temp))
                        {
                            result.Add(temp);
                        }
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 20140815 add by Dick for 加入掛節點
        /// 20150112 modified by Dick for 修正重複加入節點問題 #25
        /// </summary>
        /// <param name="pEntityName"></param>
        public void RegisterEntity(string pEntityName) {
            string FullFileName = Parent + Path.DirectorySeparatorChar + "DigiWin.HR.CustomServerApplication" + Path.DirectorySeparatorChar + "EntityTypeRegisterForCase.config";
            XmlDocument doc = FileTool.XmlFile.LoadXml(FullFileName);
            XmlNode root = doc.SelectSingleNode("EntityTypeRegister/DataEntity");
            if(root != null) {
                string basestr = "Dcms.HR.DataEntities." + pEntityName;
                bool IsExist = false;
                foreach(XmlNode node in root.ChildNodes) {
                    if(node.InnerText.Equals(basestr)) {
                        IsExist = true;
                    }
                }
                if(!IsExist) {
                    XmlElement element = doc.CreateElement("TypeName");
                    element.InnerText = basestr;
                    root.AppendChild(element);
                    doc.Save(FullFileName);
                }
            }
            else {
                XmlNode Data = doc.SelectSingleNode("EntityTypeRegister");
                if(Data != null)
                {
                    #region 20150112 modified by Dick for 修正重複加入節點問題 #25
                    XmlElement DataEntity = doc.CreateElement("DataEntity");
                    string basestr = "Dcms.HR.DataEntities." + pEntityName;                  
                    DataEntity.SetAttribute("dllFile", "DigiWin.HR.CaseBusiness.dll");
                    XmlElement element = doc.CreateElement("TypeName");
                    element.InnerText = basestr;
                    DataEntity.AppendChild(element);
                    Data.AppendChild(DataEntity);
                    doc.Save(FullFileName);
                    //XmlNode TypeName = doc.SelectSingleNode("EntityTypeRegister");
                    //if (TypeName == null)
                    //{
                    //    string basestr = "Dcms.HR.DataEntities." + pEntityName;
                    //    XmlElement DataEntity = doc.CreateElement("DataEntity");
                    //    DataEntity.SetAttribute("dllFile", "DigiWin.HR.CaseBusiness.dll");
                    //    XmlElement element = doc.CreateElement("TypeName");
                    //    element.InnerText = basestr;
                    //    DataEntity.AppendChild(element);
                    //    Data.AppendChild(DataEntity);
                    //    doc.Save(FullFileName);
                    //}
                    //else
                    //{
                    //    bool IsExsist = false;
                    //    string NewNodeName ="Dcms.HR.DataEntities." + pEntityName;
                    //    foreach (XmlNode node in TypeName.ChildNodes)
                    //    {
                    //        if (node.Name.Equals("TypeName"))
                    //        {
                    //            if (node.InnerText.Equals(NewNodeName))
                    //            {
                    //                IsExsist = true;
                    //            }
                    //        }
                    //    }
                    //    if (!IsExsist)
                    //    { //不存在則加入新節點
                    //        XmlElement element = doc.CreateElement("TypeName");
                    //        element.InnerText = NewNodeName;
                    //        TypeName.AppendChild(element);
                    //        doc.Save(FullFileName);
                    //    }
                    //}
                    #endregion                   
                }
            }
        }

        
        private XmlNode QueryViewColumn(DataGridViewRow dr, XmlDocument doc, string Entity) {
            XmlElement element = doc.CreateElement("QueryViewColumn");
            XmlElement OrderNumber = doc.CreateElement("OrderNumber");
            if(dr.Cells["Order"].Value != null) {
                OrderNumber.InnerXml = dr.Cells["Order"].Value.ToString();
            }
            element.AppendChild(OrderNumber);
            XmlElement Name = doc.CreateElement("Name");
            if(dr.Cells["Parameter"].Value != null) {
                if(dr.Cells["ReferenceProperty"].Value != null) {
                    if(dr.Cells["ReferenceProperty"].Value.ToString().Equals("CodeInfo")) {
                        Name.InnerXml = dr.Cells["Parameter"].Value.ToString() + ".ScName";
                    }
                    else if(!dr.Cells["ReferenceProperty"].Value.ToString().Equals(string.Empty)) {
                        {
                            if(dr.Cells["Parameter"].Value.ToString().Equals("EmployeeId")) {
                                ///員工Id修改成CnName
                                Name.InnerXml = dr.Cells["Parameter"].Value.ToString() + ".CnName";
                            }
                            else {
                                Name.InnerXml = dr.Cells["Parameter"].Value.ToString() + ".Name";
                            }
                        }
                    }
                    else {
                        Name.InnerXml = dr.Cells["Parameter"].Value.ToString();
                    }
                }
                else {
                    Name.InnerXml = dr.Cells["Parameter"].Value.ToString();
                }
            }
            element.AppendChild(Name);
            XmlElement DisplayName = doc.CreateElement("DisplayName");
            if(dr.Cells["Parameter"].Value != null) {
                DisplayName.InnerXml = Entity + "_" + dr.Cells["Parameter"].Value.ToString();
            }
            element.AppendChild(DisplayName);
            XmlElement visible = doc.CreateElement("Visible");
            visible.InnerXml = "true";
            element.AppendChild(visible);
            XmlElement Width = doc.CreateElement("Width");
            Width.InnerXml = "75";
            element.AppendChild(Width);
            XmlElement IsBrowable = doc.CreateElement("IsBrowable");
            IsBrowable.InnerXml = "true";
            element.AppendChild(IsBrowable);
            XmlElement Description = doc.CreateElement("Description");
            Description.InnerXml = dr.Cells["Describe"].Value.ToString();
            element.AppendChild(Description);
            return element;
        }


        private XmlNode QueryViewColumnSpecil(XmlDocument doc, string Entity, string pName, int count, bool Visible) {
            XmlElement element = doc.CreateElement("QueryViewColumn");
            XmlElement OrderNumber = doc.CreateElement("OrderNumber");
            OrderNumber.InnerXml = count.ToString();
            element.AppendChild(OrderNumber);
            XmlElement Name = doc.CreateElement("Name");
            Name.InnerXml = pName;
            element.AppendChild(Name);
            XmlElement DisplayName = doc.CreateElement("DisplayName");
            DisplayName.InnerXml = Entity + "_" + pName;
            element.AppendChild(DisplayName);
            XmlElement visible = doc.CreateElement("Visible");
            visible.InnerXml = Visible.ToString().ToLower();
            element.AppendChild(visible);
            XmlElement Width = doc.CreateElement("Width");
            Width.InnerXml = "75";
            element.AppendChild(Width);
            XmlElement IsBrowable = doc.CreateElement("IsBrowable");
            IsBrowable.InnerXml = Visible.ToString().ToLower();
            element.AppendChild(IsBrowable);
            XmlElement Description = doc.CreateElement("Description");
            Description.InnerXml = pName;
            element.AppendChild(Description);
            return element;
        }


        /// <summary>
        /// 20140828  add by Dick for 加入自定義欄位多語系
        /// </summary>
        /// <param name="GridTable"></param>
        /// <param name="pFileName"></param>
        public void AppendDataEntityDisplayInfo(DataTable GridTable, string pFileName) {
            string pPath = Parent + Path.DirectorySeparatorChar + "DigiWin.HR.CustomBusinessImplement" + Path.DirectorySeparatorChar + "Configuration" + Path.DirectorySeparatorChar + "DataEntityDisplay";
            string FullFileName = pPath + Path.DirectorySeparatorChar + "DataEntityDisplayInfoForCase.xml";
            XmlDocument doc = FileTool.XmlFile.LoadXml(FullFileName);
            XmlNode root = doc.SelectSingleNode("Root");
            bool IsExistNode = false;
            XmlElement DataEntity = doc.CreateElement("DataEntity");
            foreach(XmlNode node in root.ChildNodes) {
                if(node.Attributes != null) {
                    if(node.Attributes["TypeKey"].Value.ToString().Equals("pFileName")) {
                        IsExistNode = true;
                    }
                }
            }
            if(!IsExistNode) {
                DataEntity.SetAttribute("TypeKey", pFileName);
                DataEntity.SetAttribute("name", pFileName);
            }
            root.AppendChild(DataEntity);
            doc.Save(FullFileName);
        }


       /// <summary>
        /// 20141225 add by Dick for 寫入Permission檔案#6
       /// </summary>
       /// <param name="pPermissionPath"></param>
       /// <param name="pModuleName"></param>
       /// <param name="pSubModuleName"></param>
       /// <param name="pModules"></param>
       /// <param name="pEnToCHT"></param>
       /// <param name="pCHTToEn"></param>
        public void WritePermissionFile(string pPermissionPath,string pEntityName,string pModuleName,string pSubModuleName,string ActionString,Dictionary<string, List<string>> pModules, Dictionary<string, string> pEnToCHT ,Dictionary<string, string> pCHTToEn)
        {
            string ModuleName =string.Empty;
            string SubName =string.Empty;
            if (pCHTToEn.ContainsKey(pModuleName))
            {
                string temp = pCHTToEn[pModuleName];
                if (pModules.ContainsKey(temp))
                {
                    ModuleName = temp;
                }
                else
                {
                    //20150112 add by Dick for 考勤錯誤 #24
                    if (temp.Equals("WorkTimeManagement"))
                    {
                        temp = "WorkTimeManagemen";
                    }
                    if (pModules.ContainsKey(temp+"Module"))
                     {
                         ModuleName = temp + "Module";
                     }
                }                
            }
            if (pCHTToEn.ContainsKey(pSubModuleName))
            {
                SubName = pCHTToEn[pSubModuleName];
            }
            if (string.IsNullOrEmpty(ModuleName))
            {
                throw new Exception("寫入Permission失敗");
            }

            CheckModule(pPermissionPath, ModuleName, SubName);
            CheckSubModule(pPermissionPath, ModuleName, SubName);
            XmlDocument doc = FileTool.XmlFile.LoadXml(pPermissionPath);
            AppendModule(pEntityName, ModuleName, doc);
            AppendAction(pEntityName, doc);
            AppendBusinessObject(pEntityName, ActionString, doc);
            doc.Save(pPermissionPath);
        }

       /// <summary>
        /// 20141225 add by Dick for 加入BusinessObject #6
       /// </summary>
       /// <param name="pEntityName"></param>
       /// <param name="ActionString"></param>
       /// <param name="doc"></param>
        private void AppendBusinessObject(string pEntityName,string ActionString, XmlDocument doc)
        {
            XmlNode node = doc.ChildNodes[1].ChildNodes[2];
            bool IsExist = false;
            foreach(XmlNode Child in node.ChildNodes)
            {
                if (Child.Name.Equals("BusinessObject"))
                {
                    if (Child.Attributes[0].Value.Equals(pEntityName))
                    {
                        IsExist = true;
                    }
                }
            }
            if (!IsExist)
            {
                XmlElement BusinessObject = doc.CreateElement("BusinessObject");
                BusinessObject.SetAttribute("Name", pEntityName);
                BusinessObject.SetAttribute("ProgramName", pEntityName + "Browse");
                XmlElement ExtendCodes = doc.CreateElement("ExtendCodes");
                XmlElement ExtendCode = doc.CreateElement("ExtendCode");
                ExtendCode.SetAttribute("Name", "DEFAULT");
                ExtendCode.SetAttribute("ActionSetting", ActionString);
                ExtendCodes.AppendChild(ExtendCode);
                BusinessObject.AppendChild(ExtendCodes);
                node.AppendChild(BusinessObject);
            }
        }


       /// <summary>
        /// 20141225 add by Dick for 加入動作包 #6
        /// 20150109 modified by Dicl for 避免重複加入節點問題 #18
       /// </summary>
       /// <param name="pEntityName"></param>
       /// <param name="doc"></param>
        private void AppendAction(string pEntityName,XmlDocument doc)
        {
            XmlNode node = doc.ChildNodes[1].ChildNodes[0];
            bool IsExist = false;
            foreach(XmlNode Child in node.ChildNodes)
            {
                if (Child.Name.Equals("ActionSetting"))
                {
                    if (Child.Attributes[0].Value.Equals(pEntityName + "Actions"))
                    {
                        IsExist = true;
                    }
                }            
            }
            if (!IsExist)
            {
                XmlElement ActionSetting = doc.CreateElement("ActionSetting");
                ActionSetting.SetAttribute("Name", pEntityName + "Actions");
                XmlElement Actions = doc.CreateElement("Actions");
                ActionSetting.AppendChild(Actions);
                XmlElement Action1 = doc.CreateElement("Action");
                Action1.SetAttribute("Name", "Read");
                XmlElement Action2 = doc.CreateElement("Action");
                Action2.SetAttribute("Name", "Create");
                XmlElement Action3 = doc.CreateElement("Action");
                Action3.SetAttribute("Name", "Write");
                XmlElement Action4 = doc.CreateElement("Action");
                Action4.SetAttribute("Name", "Delete");
                Actions.AppendChild(Action1);
                Actions.AppendChild(Action2);
                Actions.AppendChild(Action3);
                Actions.AppendChild(Action4);
                ActionSetting.AppendChild(Actions);
                node.AppendChild(ActionSetting);
            }
        }

       /// <summary>
        /// 20141225 add by Dick for 加入Permission Browse #6
       /// </summary>
       /// <param name="pEntityName"></param>
       /// <param name="ModuleName"></param>
       /// <param name="doc"></param>
        private void AppendModule( string pEntityName, string ModuleName, XmlDocument doc)
        {
            foreach (XmlNode node in doc.ChildNodes[1].ChildNodes[1].ChildNodes)
            {
                if (node.Name.Equals("Module"))
                {
                    if (node.Attributes[0].Value.Equals(ModuleName))
                    {
                        XmlNode child = node.ChildNodes[0];
                        foreach (XmlNode childNode in child.ChildNodes)
                        {
                            if (childNode.Name.Equals("Module"))
                            {                                
                                XmlNode Programs = childNode.ChildNodes[0];
                                #region 20150109 modified by Dick for 避免重複增加節點問題  #18
                                bool IsExsit = false;
                                foreach (XmlNode PNode in Programs.ChildNodes)
                                {
                                    if (PNode.Name.Equals("Program"))
                                    {
                                       if(PNode.Attributes[0].Value.Equals(pEntityName + "Browse"))
                                       {                                          
                                          IsExsit = true;                                           
                                       }
                                    }
                                }
                                if (!IsExsit)
                                {
                                    XmlElement Program = doc.CreateElement("Program");
                                    Program.SetAttribute("Name", pEntityName + "Browse");
                                    Program.SetAttribute("ActionSetting", pEntityName + "Action");
                                    Program.SetAttribute("BusinessObject", pEntityName);
                                    Program.SetAttribute("Image", "TrainingType_16");
                                    Program.SetAttribute("UriAction", "Browse");
                                    Program.SetAttribute("UriTypeKey", pEntityName);
                                    Programs.AppendChild(Program);
                                }
                                #endregion                               
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 20141225 add by Dick for 檢查節點並補上 #6
        /// </summary>
        /// <param name="pPermissionPath"></param>
        /// <param name="pModuleName"></param>
        /// <param name="pSubModuleName"></param>
        private void CheckModule(string pPermissionPath,string pModuleName, string pSubModuleName)
        {
            bool Has = false;
            XmlDocument doc = FileTool.XmlFile.LoadXml(pPermissionPath);
            foreach (XmlNode node in doc.ChildNodes[1].ChildNodes[1].ChildNodes)
            {
                if (node.Name.Equals("Module"))
                {
                    if (node.Attributes[0].Value.Equals(pModuleName))
                    {
                        Has = true;
                    }
                }
            }
            if (!Has)
            {
                XmlElement Module = doc.CreateElement("Module");
                Module.SetAttribute("Name", pModuleName);
                XmlElement Modules = doc.CreateElement("Modules");
                Module.AppendChild(Modules);
                XmlElement SubModule = doc.CreateElement("Module");
                SubModule.SetAttribute("Name", pSubModuleName);
                Modules.AppendChild(SubModule);
                XmlElement Programs = doc.CreateElement("Programs");
                SubModule.AppendChild(Programs);
                doc.ChildNodes[1].ChildNodes[1].AppendChild(Module);
                doc.Save(pPermissionPath);
            }
        }

        /// <summary>
        /// 20141225 add by Dick for 檢查節點並補上 #6
        /// </summary>
        /// <param name="pPermissionPath"></param>
        /// <param name="pModuleName"></param>
        /// <param name="pSubModuleName"></param>
        private void CheckSubModule(string pPermissionPath, string pModuleName, string pSubModuleName)
        {
            bool Has = false;
            XmlDocument doc = FileTool.XmlFile.LoadXml(pPermissionPath);
            foreach (XmlNode node in doc.ChildNodes[1].ChildNodes[1].ChildNodes)
            {
                if (node.Name.Equals("Module"))
                {
                    if (node.Attributes[0].Value.Equals(pModuleName))
                    {
                        XmlNode Modules = node.ChildNodes[0];
                        foreach (XmlNode child in Modules.ChildNodes)
                        {
                            if (child.Name.Equals("Module"))
                            {
                                if (child.Attributes[0].Value.Equals(pSubModuleName))
                                {
                                    Has = true;
                                }
                            }
                        }
                    }
                }
            }

            if (!Has)
            {
                foreach (XmlNode node in doc.ChildNodes[1].ChildNodes[1].ChildNodes)
                {
                    if (node.Name.Equals("Module"))
                    {
                        if (node.Attributes[0].Value.Equals(pModuleName))
                        {
                            XmlNode Modules = node.ChildNodes[0];
                            XmlElement SubModule = doc.CreateElement("Module");
                            SubModule.SetAttribute("Name", pSubModuleName);
                            Modules.AppendChild(SubModule);
                            XmlElement Programs = doc.CreateElement("Programs");
                            SubModule.AppendChild(Programs);
                            Modules.AppendChild(SubModule);
                            doc.Save(pPermissionPath);
                        }
                    }
                }
            }
        }


    }

    /// <summary>
    /// 20140819 add by Dick for  控件輔助類
    /// </summary>
    public class SubUIControl {
        /// <summary>
        /// 控件排列模式  一行2個
        /// </summary>
        public enum Mode2 {
            Point1x = 170,
            Point1y = 45,
            Point2x = 490,
            Point2y = 45
        }
        /// <summary>
        /// 控件排列模式  一行3個
        /// </summary>
        public enum Mode3{
            Point1x=25,
            Point1y=45,
            Point2x=300,
            Point2y =45,
            Point3x=470,
            Point3y =45
        }
        ///// <summary>
        ///// 控件排列模式  一行3個
        ///// </summary>
        //public enum Mode4 {
        //    Point1x = 25,
        //    Point1y = 45,
        //    Point2x = 300,
        //    Point2y = 45,
        //    Point3x = 470,
        //    Point3y = 45
        //    Point3x = 470,
        //    Point3y = 45
        //}         
        /// <summary>
        /// Label
        /// </summary>
        public string LabelName { set; get; }
        public string LabelContext { set; get; }
        public string LabelDeclare { set; get; }
        public string LabelAdd { set; get; }
        public string LabelNewControl { set; get; }
        /// <summary>
        ///  控件
        /// </summary>
        public string Name { set; get; }
        public string ControlFullName { set; get; }
        public string Declare { set; get; }
        public int Order { set; get; }
        public string NewControl { set; get; }
        public string Context { set; get; }
        public string Layout { set; get; }
        public string GroupAdd { set; get; }
        //public string X { set; get; }
        //public string Y { set; get; }
        /// <summary>
        /// 20140819 設定Label及Control的位置
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public void SetLocation(int X,int Y) {            
            LabelContext += "            " + LabelName + "Label1.Location = new System.Drawing.Point(" + X + ", " + Y + ");";
            Context += "\r\n            " + this.ControlFullName + ".Location =new System.Drawing.Point(" +( X +13)+ ", " + Y + ");";
        }
    }


    /// <summary>
    /// 20140923 add by Dick 
    /// 多個頁簽 QueryProject
    /// </summary>
    public class QueryViewCondition {
        public string BrowseName { set; get; }  //瀏覽頁簽名稱
        public string Description { set; get; } //中文描述
        public string Type { set; get; }        //型態 1.Select   2.Browse
        public List<Condition> ConditionList = new List<Condition>();                
    }

    /// <summary>
    /// 20140923 add by Dick 
    /// QueryView條件
    ///   &lt;QueryItem ItemType="Dcms.Common.UI.QueryItem" ID="1" ParentID="0" ParameterName="" DbType="Boolean" FieldName="Flag" Value1="True" Value2="" Symbol="EqualCondition" /&gt;
    /// </summary>
    public class Condition {
        public string ID { set; get; }             //ID
        public string ParentID { set; get; }
        public string ParameterName { set; get; }
        public string Type { set; get; }           //資料型態
        public string Field { set; get; }          //條件欄位位名稱
        public string Conditon1 { set; get; }      //條件1
        public string Conditon2 { set; get; }      //條件2
        public string sybel { set; get; }          //And OR 等等
    }

    /// <summary>
    /// 明細
    /// </summary>
    public class Detail {
        public string Name { set; get; }
        public string Description { set; get; }
    }
}
