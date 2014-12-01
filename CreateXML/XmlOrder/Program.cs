using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.IO;


namespace XmlOrder {
    class Program {
        static void Main(string[] args) {
            NewMethod();
        }
        private static void NewMethod() {
            string ThisPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Old.xml");
            string SavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "New.xml");
            if(!File.Exists(ThisPath)) {
                return;
            }
            XmlDocument doc = 
                FileTool.XmlFile(ThisPath);
            XmlNode node = doc.SelectSingleNode("root/QueryViewColumns");
            object[] arry = new object[300];
            int order = 0;
            int reapet = 199;
            foreach(XmlNode child in node.ChildNodes) {
                foreach(XmlNode grad in child.ChildNodes) {
                    if(grad.Name.Equals("OrderNumber")) {
                        //int order = 0;
                        //int.TryParse(grad.InnerText,out order);
                        grad.InnerText = order.ToString();
                        //if(arry[order] != null) {
                        //    arry[reapet] = child;
                        //    reapet--;
                        //}
                        //else {
                        arry[order] = child;
                        //}
                        break;
                    }
                }
                order++;
            }

            using(StreamWriter sw = new StreamWriter(SavePath, false)) {
                StringBuilder sb = new StringBuilder();
                sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
                sb.Append("<root>\r\n");
                sb.Append("<QueryViewColumns>\r\n");
                sb.Append("</QueryViewColumns>\r\n");
                sb.Append("</root>\r\n");
                sw.Write(sb.ToString());
                sw.Close();
            }
            XmlDocument newdoc = Tools.XmlTool.LoadXml(SavePath);
            XmlNode newnode = newdoc.SelectSingleNode("root/QueryViewColumns");
            foreach(object child in arry) {
                if(child != null) {
                    XmlNode no = child as XmlNode;
                    XmlElement NewNode = newdoc.CreateElement(no.Name);
                    foreach(XmlNode nono in no.ChildNodes) {
                        XmlElement element = newdoc.CreateElement(nono.Name);
                        element.InnerText = nono.InnerText;
                        NewNode.AppendChild(element);
                    }
                    newnode.AppendChild(NewNode);
                }
            }
            newdoc.Save(SavePath);
        }
    }
}
