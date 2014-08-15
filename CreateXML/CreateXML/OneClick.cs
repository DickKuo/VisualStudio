using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Data;
using System.Xml.Serialization;

namespace CreateXML {
    class OneClick {       

        public string Expory { get; set; }

        public string Parent { get; set; }

        public OneClick(string parent,string export) {
            Expory = export;
            Parent = parent;
        }

        public void CreateEntitesFile(string pFileName,string pContext) {
            string pPath = Parent + Path.DirectorySeparatorChar + "DigiWin.HR.CustomBusiness" + Path.DirectorySeparatorChar + "DataEntities";
            string FullFileName = pPath + Path.DirectorySeparatorChar + pFileName + ".cs";           
            StreamWriter Sw = new StreamWriter(FullFileName, false);
            Sw.Write(pContext);
            Sw.Close();
        }

        public void CreateInterFaceFile(string pFileName, string pContext) {
            string pPath = Parent + Path.DirectorySeparatorChar + "DigiWin.HR.CustomBusiness" + Path.DirectorySeparatorChar + "Services";
            string FullFileName = pPath + Path.DirectorySeparatorChar + pFileName + ".cs";
            StreamWriter Sw = new StreamWriter(FullFileName, false);
            Sw.Write(pContext);
            Sw.Close();
        }

        public void CreateServiceFile(string pFileName, string pContext) {
            string pPath = Parent + Path.DirectorySeparatorChar + "DigiWin.HR.CustomBusinessImplement" + Path.DirectorySeparatorChar + "Services";
            string FullFileName = pPath + Path.DirectorySeparatorChar + pFileName + ".cs";
            StreamWriter Sw = new StreamWriter(FullFileName, false);
            Sw.Write(pContext);
            Sw.Close();
        }

        
        public void CreateQueryView(string pFileName, string pContext,System.Windows.Forms.DataGridView GridView) {
            string pPath = Parent + Path.DirectorySeparatorChar + "DigiWin.HR.CustomBusinessImplement" + Path.DirectorySeparatorChar + "Configuration"+ Path.DirectorySeparatorChar +"Query"+Path.DirectorySeparatorChar+"Case";
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

            foreach(DataGridViewRow dr in GridView.Rows)
            {
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
                QueryViewColumns.AppendChild(QueryViewColumnSpecil(doc, EntityName,"Remark",count,true));            
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


        private XmlNode QueryViewColumn(DataGridViewRow dr, XmlDocument doc, string Entity)
        {
             XmlElement element =doc.CreateElement("QueryViewColumn");
               XmlElement OrderNumber = doc.CreateElement("OrderNumber");
                 if(dr.Cells["Order"].Value != null) {
                     OrderNumber.InnerXml = dr.Cells["Order"].Value.ToString();
                }
                 element.AppendChild(OrderNumber);
           

                XmlElement Name = doc.CreateElement("Name");
                if(dr.Cells["Parameter"].Value != null) {
                    if(dr.Cells["ReferenceProperty"].Value!=null)                     
                    {
                        if(dr.Cells["ReferenceProperty"].Value.ToString().Equals("CodeInfo")) {
                            Name.InnerXml = dr.Cells["Parameter"].Value.ToString() + ".ScName";
                        }
                        else if(!dr.Cells["ReferenceProperty"].Value.ToString().Equals(string.Empty)) {
                            {
                                Name.InnerXml = dr.Cells["Parameter"].Value.ToString() + ".Name";
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


        private XmlNode QueryViewColumnSpecil( XmlDocument doc, string Entity,string pName,int count,bool Visible) {
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
            IsBrowable.InnerXml =  Visible.ToString().ToLower();
            element.AppendChild(IsBrowable);

            XmlElement Description = doc.CreateElement("Description");
            Description.InnerXml = pName;
            element.AppendChild(Description);



            return element;
        }



        public void AppendDataEntityDisplayInfo(DataTable GridTable, string pFileName) {
            string pPath = Parent + Path.DirectorySeparatorChar + "DigiWin.HR.CustomBusinessImplement" + Path.DirectorySeparatorChar + "Configuration" + Path.DirectorySeparatorChar + "DataEntityDisplay";
            string FullFileName = pPath + Path.DirectorySeparatorChar + "DataEntityDisplayInfoForCase.xml";
            XmlDocument doc = Tools.XmlTool.LoadXml(FullFileName);
            XmlNode root = doc.SelectSingleNode("Root");
            bool IsExistNode = false;
            XmlElement DataEntity = doc.CreateElement("DataEntity");
            foreach(XmlNode node in root.ChildNodes)
            {
                if(node.Attributes != null) {
                    if(node.Attributes["TypeKey"].Value.ToString().Equals("pFileName")) {
                        IsExistNode = true;
                    }
                }
            }
            if(!IsExistNode) {
                DataEntity.SetAttribute("TypeKey", pFileName);
                DataEntity.SetAttribute("name",pFileName);

            }
            root.AppendChild(DataEntity);          
            doc.Save(FullFileName);
        }

    }
}
