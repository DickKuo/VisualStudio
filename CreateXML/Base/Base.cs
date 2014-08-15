using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Base {
    public class Base {

        public static XmlDocument LoadXml(string Path) {
            XmlDocument doc = new XmlDocument();
            System.IO.StreamReader sr = new System.IO.StreamReader(Path);
            doc.Load(sr);
            sr.Close();
            sr.Dispose();
            return doc;
        }

        //public static void RecurceeNode(TreeNode tree, XmlNode root) {
        //    foreach (XmlNode child in root.ChildNodes) {
        //        tree.Nodes.Add(child.Attributes[0].Value.ToString());
        //        if (child.ChildNodes.Count > 0) {
        //            RecurceeNode(tree.Nodes[tree.Nodes.Count - 1], child);
        //        }
        //    }
        //}

        //public static string TreeViewGetTypeByID(string name) {
        //    XmlDocument doc = LoadXml(Properties.Settings.Default.FileName);
        //    XmlNode node = doc.GetElementById(name);
        //    if (node != null) {
        //        return node.Attributes[1].Value.ToString();
        //    }
        //    else {
        //        return null;
        //    }
        //}

        //public void CreateStockXmlFile(string SaveFilePath) {
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml("<!DOCTYPE root [" +
        //         "<!ELEMENT Masterial ANY>" +
        //       "<!ATTLIST Masterial Name ID #REQUIRED>" +
        //        "]>" +
        //        "<root/>");
        //    doc.Save(SaveFilePath);
        //    MessageBox.Show("建檔完成");
        //}

        //public void CreateTreeFile() {
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml("<!DOCTYPE root [" +
        //        "<!ELEMENT Job ANY>" +
        //         "<!ELEMENT Item ANY>" +
        //         "<!ELEMENT Masterial ANY>" +
        //       "<!ATTLIST Job Name ID #REQUIRED>" +
        //       "<!ATTLIST Item Name ID #REQUIRED>" +
        //       "<!ATTLIST Masterial Name ID #REQUIRED>" +
        //        "]>" +
        //        "<root/>");
        //    string[] arry = { "侍", "僧", "神職", "忍者", "鍛冶師", "陰陽師", "藥師", "傾奇者" };
        //    XmlNode root = doc.SelectSingleNode("root");
        //    foreach (var i in arry) {
        //        string ns = root.GetNamespaceOfPrefix("bk");
        //        XmlNode attr = doc.CreateNode(XmlNodeType.Attribute, "Name", ns);
        //        attr.Value = i.ToString();
        //        XmlElement Job = doc.CreateElement("Job");
        //        Job.Attributes.SetNamedItem(attr);
        //        XmlNode attr2 = doc.CreateNode(XmlNodeType.Attribute, "Type", "");
        //        attr2.Value = "Profession";
        //        Job.Attributes.SetNamedItem(attr2);
        //        root.AppendChild(Job);
        //    }
        //    doc.Save(Properties.Settings.Default.FileName);
        //    MessageBox.Show("建檔完成!!");
        //}
    }
}
