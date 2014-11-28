using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Tools {
    public class XmlTool {
        public static XmlDocument LoadXml(string Path) {
            XmlDocument doc = new XmlDocument();
            System.IO.StreamReader sr = new System.IO.StreamReader(Path);
            doc.Load(sr);
            sr.Close();
            sr.Dispose();
            return doc;
        }

        public static String ReOrderMethod(string pInput)
        {
            string ThisPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Old.xml");
            WriteXml(ThisPath, pInput);
            XmlDocument doc = Tools.XmlTool.LoadXml(ThisPath);
            XmlNode node = doc.SelectSingleNode("root/QueryViewColumns");
            object[] arry = new object[300];
            int order = 0;
            int reapet = 199;  
            foreach (XmlNode child in node.ChildNodes)
            {
                foreach (XmlNode grad in child.ChildNodes)
                {
                    if (grad.Name.Equals("OrderNumber"))
                    {                         
                        int.TryParse(grad.InnerText, out order);                        
                        arry[order] = child;                       
                        break;
                    }
                }               
            }
            StringBuilder NewString =new StringBuilder();
            NewString.Append("<QueryViewColumns> \r\n");
            foreach (XmlNode child in arry)
            {
                if (child != null)
                {
                    NewString.Append("  <QueryViewColumn> \r\n");
                    foreach (XmlNode grad in child.ChildNodes)
                    {
                        NewString.Append("    ");
                        NewString.Append(grad.OuterXml);
                        NewString.Append("\r\n");
                    }
                    NewString.Append("  </QueryViewColumn> \r\n");
                }
            }
            NewString.Append("</QueryViewColumns> \r\n");
            return NewString.ToString();           
        }

        public static String NewOrderMethod(string pInput)
        {
            string NewXmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "New.xml");
            WriteXml(NewXmlPath, pInput);
            XmlDocument doc = Tools.XmlTool.LoadXml(NewXmlPath);
            XmlNode node = doc.SelectSingleNode("root/QueryViewColumns");
            int order = 0;
            StringBuilder NodeBuilder = new StringBuilder();
            NodeBuilder.Append("<QueryViewColumns> \r\n");
            foreach (XmlNode child in node.ChildNodes)
            {
                NodeBuilder.Append("  <QueryViewColumn> \r\n");
                foreach (XmlNode grad in child.ChildNodes)
                {
                    if (grad.Name.Equals("OrderNumber"))
                    {
                        grad.InnerXml = order.ToString();
                    }
                    NodeBuilder.Append("    ");
                    NodeBuilder.Append(grad.OuterXml);
                    NodeBuilder.Append("\r\n");                    
                }
                NodeBuilder.Append("  </QueryViewColumn> \r\n");
                order++;
            }
            NodeBuilder.Append("</QueryViewColumns> \r\n");
            return NodeBuilder.ToString();
        }

        /// <summary>
        /// 20141128 add by Dick for ru8
        /// </summary>
        /// <param name="pInput"></param>
        private static void WriteXml(string pSavePath, string pInput)
        {
            using (StreamWriter sw = new StreamWriter(pSavePath,false))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
                sb.Append("<root>\r\n");
                sb.Append(pInput);
                sb.Append("</root>\r\n");
                sw.Write(sb.ToString());
                sw.Close();
            }
        }
    }
}
