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
    class OneClick {

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
            string pPath = Parent + Path.DirectorySeparatorChar + pDirectory + Path.DirectorySeparatorChar + pSubDirectory;
            string FullFileName = pPath + Path.DirectorySeparatorChar + pFileName + ".cs";
            using(StreamWriter Sw = new StreamWriter(FullFileName, false)) {
                Sw.Write(pContext);
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
            XmlDocument doc = Tools.XmlTool.LoadXml(FullFileName);
            XmlNode root = doc.SelectSingleNode("QueryConfiguration");
            XmlNode QueryViewXML = root.SelectSingleNode("QueryViewXML/QueryView/QueryViewId");
            QueryViewXML.InnerXml = EntityName + "_Browse";
            XmlNode RefToTypeKey = root.SelectSingleNode("QueryViewXML/QueryView/RefToTypeKey");
            RefToTypeKey.InnerXml = EntityName;
            XmlNode QueryViewColumns = root.SelectSingleNode("QueryViewXML/QueryView/QueryViewColumns");
            int count = 1;
            bool IsRemarkExit = false;
            //加入Id
            QueryViewColumns.AppendChild(QueryViewColumnSpecil(doc, EntityName, EntityName + "Id", 0, false));
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
            XmlNode QueryProjectId = root.SelectSingleNode("QueryProjectXML/QueryProject/QueryProjectId");
            QueryProjectId.InnerXml = EntityName + "_Browse";
            XmlNode QueryProjectViewId = root.SelectSingleNode("QueryProjectXML/QueryProject/QueryViewId");
            QueryProjectViewId.InnerXml = EntityName + "_Browse";
            XmlNode QueryProjectRefToTypeKey = root.SelectSingleNode("QueryProjectXML/QueryProject/RefToTypeKey");
            QueryProjectRefToTypeKey.InnerXml = EntityName;
            XmlNode QueryCollectId = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectId");
            QueryCollectId.InnerXml = EntityName + "_Browse";
            XmlNode QueryCollectRefToTypeKey = root.SelectSingleNode("QueryCollectXML/QueryCollect/RefToTypeKey");
            QueryCollectRefToTypeKey.InnerXml = EntityName;
            XmlNode QueryCollectItemId = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectItems/QueryCollectItem/QueryCollectItemId");
            QueryCollectItemId.InnerXml = EntityName + "_Browse_QueryCollectItem" + Guid.NewGuid().ToString();
            XmlNode QueryCollectQueryProjectId = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectItems/QueryCollectItem/QueryProjectId");
            QueryCollectQueryProjectId.InnerXml = EntityName + "_Browse";
            XmlNode QueryCollectQueryRefToTypeKey = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectItems/QueryCollectItem/RefToTypeKey");
            QueryCollectQueryRefToTypeKey.InnerXml = EntityName;
            XmlNode TextTextQueryCollectQueryText = root.SelectSingleNode("QueryCollectXML/QueryCollect/QueryCollectItems/QueryCollectItem/Text");
            TextTextQueryCollectQueryText.InnerXml = EntityName + "_Browse";
            doc.Save(FullFileName);
        }

        /// <summary>
        /// 20140815 建立單檔UI
        /// </summary>
        /// <param name="pEntityName"></param>
        public void CreateentityNoDetailBrowseEditViewV5(string pEntityName, DataGridView dt,int Mode) {
            string SourcePath = System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "SampleFile\\Sample.txt";
            StreamReader sr = new StreamReader(SourcePath);
            string line = string.Empty;
            #region 加入控件插入
            List<SubUIControl> li = AddControlerInit(dt, pEntityName);
            StringBuilder SBParameter = new StringBuilder();
            StringBuilder SBNewControl = new StringBuilder();
            StringBuilder SBLayout = new StringBuilder();
            StringBuilder SBContext = new StringBuilder();
            StringBuilder SBAdd = new StringBuilder();
            //起始座標
            int x = 50;
            int y = 50;

            ///排列邏輯還在想
            int count = 0;
            foreach(SubUIControl control in li.OrderBy(o => o.Order)) {
                if(count % Mode != 0) {
                    x = 50;
                    y += 50;
                }

                    SBParameter.Append(control.Declare);
                    SBParameter.Append("\r\n");
                    SBParameter.Append(control.LabelDeclare);
                    SBParameter.Append("\r\n");
                    SBNewControl.Append(control.NewControl);
                    SBNewControl.Append("\r\n");
                    SBNewControl.Append(control.LabelNewControl);
                    SBNewControl.Append("\r\n");
                    SBLayout.Append(control.Layout);
                    SBLayout.Append("\r\n");
                    SBContext.Append(control.LabelContext.Replace("$X", x.ToString()).Replace("$Y", y.ToString()));
                    SBContext.Append("\r\n");
                    x += 40;
                    SBContext.Append(control.Context.Replace("$X", (x + 30).ToString()).Replace("$Y", y.ToString()));
                    SBContext.Append("\r\n");
                    SBAdd.Append(control.LabelAdd);
                    SBAdd.Append("\r\n");
                    SBAdd.Append(control.GroupAdd);
                    SBAdd.Append("\r\n");
                    x += 150;
                count++;
            }

            #endregion
            StringBuilder sb = new StringBuilder();
            #region 每行撈取分析
            while((line = sr.ReadLine()) != null) {
                if(line.IndexOf("this.entityEditerView1 = new Dcms.HR.UI.EntityEditerView();") != -1) {
                    sb.Append("\r\n");
                    string temp = "            ((Label)(this.Controls.Find(\"labDoc\",true)[0])).Text = ((Label)(this.Controls.Find(\"labDoc\",true)[0])).Text  + \"*\";";
                    sb.Append(temp);
                    sb.Append("\r\n");
                }
                if(line.IndexOf("//<createDate>date</createDate>") != -1) {
                    line = line.Replace("date", DateTime.Now.ToString("yyyy/MM/dd"));
                }
                if(line.IndexOf("//<description>description</description>") != -1) {
                    line = line.Replace("//<description>description</description>", "//<description>新增單檔作業</description>");
                }
                if(line.IndexOf(" public class EntityEditerView : HREditerView {") != -1) {
                    line = line.Replace("Entity", pEntityName);
                }
                if(line.IndexOf("EntityEditerView()") != -1) {
                    line = line.Replace("Entity", pEntityName);
                }
                if(line.IndexOf("this.entityBindingSource") != -1) {
                    line = line.Replace("entity", pEntityName.ToLower());
                }
                if(line.IndexOf("private BindingSource entityBindingSource;") != -1) {
                    line = line.Replace("entity", pEntityName.ToLower());
                }
                if(line.IndexOf("Dcms.HR.DataEntities.Entity") != -1) {
                    line = line.Replace("Entity", pEntityName);
                }
                if(line.IndexOf("return Factory.GetService<IEntityService>();") != -1) {
                    line = line.Replace("IEntityService", "I" + pEntityName.Remove(0, 1) + "ServiceX");
                }
                if(line.IndexOf("browseWindow.Name = GetBrowseWindowName();") != -1) {
                    sb.Append("\r\n");
                    sb.Append("            browseWindow.UsingExtraText = true;\r\n");
                }
                if(line.IndexOf("Resources.EntityDisplayName;") != -1) {
                    line = line.Replace("Resources.Entity", "ResourcesForCase." + pEntityName);
                }
                if(line.IndexOf("entity") != -1) {
                    line = line.Replace("entity", pEntityName.ToLower());
                }
                if(line.IndexOf("Entity") != -1) {
                    line = line.Replace("Entity", pEntityName);
                }
                if(line.IndexOf("//ResourceExtend") != -1) {
                    line = line.Replace("//ResourceExtend", "System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(" + pEntityName + "EditerView));");
                }
                if(SBParameter.Length > 0) {
                    if(line.IndexOf("//ParameterExtend") != -1) {
                        line = line.Replace("//ParameterExtend", "");
                        sb.Append(SBParameter);
                    }
                }
                if(SBNewControl.Length > 0) {
                    if(line.IndexOf("//NewControlExtend") != -1) {
                        line = line.Replace("//NewControlExtend", "");
                        sb.Append(SBNewControl);
                    }
                }
                if(SBLayout.Length > 0) {
                    if(line.IndexOf("//LayoutExtend") != -1) {
                        line = line.Replace("//LayoutExtend", "");
                        sb.Append(SBLayout);
                    }
                }
                if(SBContext.Length > 0) {
                    if(line.IndexOf("//ContextExtend") != -1) {
                        line = line.Replace("//ContextExtend", "");
                        sb.Append(SBContext);
                    }
                }
                if(SBAdd.Length > 0) {
                    if(line.IndexOf("//groupBoxExtend") != -1) {
                        line = line.Replace("//groupBoxExtend", "");
                        sb.Append(SBAdd);
                    }
                }
                sb.Append(line + "\r\n");
            }
            #endregion
            sr.Close();
            string SaveFile = Parent + Path.DirectorySeparatorChar + "DigiWin.HR.CustomUI" + Path.DirectorySeparatorChar + pEntityName + ".cs";
            StreamWriter sw = new StreamWriter(SaveFile);
            sw.Write(sb.ToString());
            sw.Close();
        }

        /// <summary>
        /// 20140818 add by Dick 加入控件List
        /// </summary>
        /// <param name="dt"></param>
        public List<SubUIControl> AddControlerInit(System.Windows.Forms.DataGridView dv, string EntityName) {
            List<SubUIControl> li = new List<SubUIControl>();
            foreach(DataGridViewRow dr in dv.Rows) {
                if(dr.Cells["UIOrder"] != null) {
                    if(!string.IsNullOrEmpty(dr.Cells["UIOrder"].ToString())) {
                        if(dr.Cells["UIOrder"].Value != null && dr.Cells["UIOrder"].Value.ToString() != string.Empty) {
                            if(!string.IsNullOrEmpty(dr.Cells["Type"].Value.ToString())) {
                                SubUIControl control = null;
                                switch(dr.Cells["Type"].Value.ToString()) {
                                    case "Int":
                                        break;
                                    case "Decmail":
                                        control = ControlsSetting(EntityName, dr, control, "DcmsCalcEdit");
                                        control.Context += "\r\n            this." + control.Name + "DcmsCalcEdit.ImeMode = System.Windows.Forms.ImeMode.Off;";
                                        control.Context+="\r\n            this."+control.Name+"DcmsCalcEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {";
                                        control.Context += "\r\n            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});";
                                        control.Context += "\r\n            this." + control.Name + "DcmsCalcEdit.Properties.Mask.UseMaskAsDisplayFormat = true;";
                                        control.Layout = "             ((System.ComponentModel.ISupportInitialize)(this." + control.Name + "DcmsCalcEdit.Properties)).EndInit();\r\n";
                                        break;
                                    case "Guid":
                                        if(!string.IsNullOrEmpty(dr.Cells["ReferenceProperty"].Value.ToString())) {
                                            control = ControlsSetting(EntityName, dr, control, "HRSelectControl");
                                            control.Context += "            this.hRSelectControlDepartmentId.TypeKey =\"" + dr.Cells["ReferenceProperty"].Value.ToString()+"\";";
                                        }
                                        break;
                                    case "String":
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
                                    case "DateTime":
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
                                    //加入Label
                                    control.LabelName = control.Name + "Label1";
                                    control.LabelDeclare += "\r\n            System.Windows.Forms.Label  " + control.Name + "Label1;";
                                    control.LabelNewControl += "\r\n            " + control.Name + "Label1 = new System.Windows.Forms.Label();";
                                    control.LabelAdd = "\r\n            this.groupBox1.Controls.Add(" + control.Name + "Label1);";
                                    control.LabelContext += "\r\n            //";
                                    control.LabelContext += "\r\n            // " + control.LabelName;
                                    control.LabelContext += "\r\n            // ";
                                    control.LabelContext += "\r\n            " + control.LabelName + ".AutoSize = true;";
                                    control.LabelContext += "\r\n            " + control.LabelName + ".Name = \"" + control.LabelName + "\";";
                                    control.LabelContext += "\r\n            " + control.LabelName + ".Size = new System.Drawing.Size(29, 12);";
                                    control.LabelContext += "\r\n            " + control.LabelName + ".TabIndex = 2;";
                                    control.LabelContext += "\r\n            " + control.LabelName + ".Text = \"" + control.Name+":\";";
                                    control.LabelContext += "\r\n            " + control.LabelName + ".TextAlign = System.Drawing.ContentAlignment.MiddleRight;\r\n";
                                    control.LabelContext += "\r\n            " + control.LabelName + ".Location = new System.Drawing.Point($X, $Y);";

                                    li.Add(control);
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
            control.Order = dr.Cells["UIOrder"].Value.ToString();
            control.Declare = "        private " + ControlType + " "+temp + control.Name + ";";
            if(ControlType.Equals("HRSelectControl")) { ///SelectControl 命名空間不一樣
                control.NewControl = "            this." + temp + control.Name + " = new " + ControlType + "();\r\n";
            } 
            else {
                control.NewControl = "            this." + temp + control.Name + " = new Dcms.Common.UI." + ControlType + "();\r\n";
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
        /// </summary>
        /// <param name="pString"></param>
        /// <param name="FileName"></param>
        public void AddResourceRow(string pDirectory, string pString, string FileName, bool IsAppend) {
            string SaveFile = Parent + Path.DirectorySeparatorChar + pDirectory + Path.DirectorySeparatorChar + "Properties" + Path.DirectorySeparatorChar + FileName + ".resx";
            string[] spl = pString.Split('\t');
            if(spl.Length > 2) {
                AddResource(SaveFile, spl);
                if(IsAppend) {
                    string DesignerFile = Parent + Path.DirectorySeparatorChar + pDirectory + Path.DirectorySeparatorChar + "Properties" + Path.DirectorySeparatorChar + FileName + ".Designer.cs";
                    StringBuilder sb = new StringBuilder();
                    using(StreamReader reader = new StreamReader(DesignerFile)) {
                        string line = string.Empty;
                        while((line = reader.ReadLine()) != null) {
                            sb.Append(line);
                            sb.Append("\r\n");
                            if(line.IndexOf("resourceCulture = value;") != -1) {
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
        private static void AddResource(string SaveFile, string[] spl) {
            XmlDocument doc = Tools.XmlTool.LoadXml(SaveFile);
            XmlNode root = doc.SelectSingleNode("root");
            XmlElement element = doc.CreateElement("data");
            element.SetAttribute("name", spl[0]);
            element.SetAttribute("xml:space", "preserve");
            XmlElement value = doc.CreateElement("value");
            if(spl[1].Substring(0, 1) == "X") {
                value.InnerText = spl[1].Remove(0, 1);
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
        /// 20140815 add by Dick for 加入掛節點
        /// </summary>
        /// <param name="pEntityName"></param>
        public void RegisterEntity(string pEntityName) {
            string FullFileName = Parent + Path.DirectorySeparatorChar + "DigiWin.HR.CustomServerApplication" + Path.DirectorySeparatorChar + "EntityTypeRegisterForCase.config";
            XmlDocument doc = Tools.XmlTool.LoadXml(FullFileName);
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
                if(Data != null) {
                    string basestr = "Dcms.HR.DataEntities." + pEntityName;
                    XmlElement DataEntity = doc.CreateElement("TypeName");
                    DataEntity.SetAttribute("dllFile", "DigiWin.HR.CaseBusiness.dll");
                    XmlElement element = doc.CreateElement("TypeName");
                    element.InnerText = basestr;
                    DataEntity.AppendChild(element);
                    Data.AppendChild(DataEntity);
                    doc.Save(FullFileName);
                }
            }
        }

        /// <summary>
        /// 建立QueryViewXmL結構
        /// </summary>
        /// <returns></returns>
        private string NewQueryView() {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<QueryConfiguration  Version=\"5.1.3.1\" IsCase=\"1\">");
            sb.Append(" <QueryViewXML>");
            sb.Append("   <QueryView>");
            sb.Append("      <QueryViewId></QueryViewId>");
            sb.Append("      <Name>Browse</Name>");
            sb.Append("      <RefToTypeKey></RefToTypeKey>");
            sb.Append("      <IsSystem>true</IsSystem>");
            sb.Append("      <QueryViewColumns></QueryViewColumns>");
            sb.Append("   </QueryView>");
            sb.Append(" </QueryViewXML>");
            sb.Append(" <QueryProjectXML>");
            sb.Append("   <QueryProject>");
            sb.Append("   <Text>Browse</Text>");
            sb.Append("   <QueryProjectId></QueryProjectId>");
            sb.Append("   <ProjectType>2</ProjectType>");
            sb.Append("   <Name>Browse</Name>");
            sb.Append("   <RefToTypeKey></RefToTypeKey>");
            sb.Append("   <QueryViewId></QueryViewId>");
            sb.Append("   <IsSystem>true</IsSystem>");
            sb.Append("   </QueryProject>");
            sb.Append(" </QueryProjectXML>");
            sb.Append(" <QueryCollectXML>");
            sb.Append(" <QueryCollect>");
            sb.Append(" <QueryCollectId></QueryCollectId>");
            sb.Append(" <Name>Browse</Name>");
            sb.Append(" <RefToTypeKey></RefToTypeKey>");
            sb.Append(" <IsSystem>true</IsSystem>");
            sb.Append(" <QueryCollectItems>");
            sb.Append(" <QueryCollectItem>");
            sb.Append(" <QueryCollectItemId> </QueryCollectItemId>");
            sb.Append(" <QueryProjectId> </QueryProjectId>");
            sb.Append(" <QueryName>Browse</QueryName>");
            sb.Append(" <Text>Browse</Text>");
            sb.Append(" <RefToTypeKey> </RefToTypeKey>");
            sb.Append(" <OrderNumber>0</OrderNumber>");
            sb.Append(" </QueryCollectItem>");
            sb.Append(" </QueryCollectItems>");
            sb.Append(" </QueryCollect>");
            sb.Append(" </QueryCollectXML>");
            sb.Append("</QueryConfiguration>");
            return sb.ToString();
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
            XmlDocument doc = Tools.XmlTool.LoadXml(FullFileName);
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
        public string Order { set; get; }
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
}
