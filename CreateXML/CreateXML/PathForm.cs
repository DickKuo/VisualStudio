using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using CommTool;

namespace CreateXML {
    public partial class PathForm : Form {
        public PathForm() {
            InitializeComponent();
            string Xmlpath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Path.xml";
            XmlDocument doc = CommTool.XmlFile.LoadXml(Xmlpath);
            XmlNode root = doc.SelectSingleNode("root");
            int count = 0;
            foreach(XmlNode child in root.ChildNodes) {
                cob_Path.Items.Add(child.Attributes["Xpath"].Value.ToString());
                if(child.Attributes["Set"].Value.ToString() == "true") {
                    cob_Path.SelectedIndex = count;
                }
                count++;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if(FBD.ShowDialog() == DialogResult.OK) {
                cob_Path.Items.Add(FBD.SelectedPath);
                string Xmlpath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Path.xml";
                XmlDocument doc = CommTool.XmlFile.LoadXml(Xmlpath);
                XmlNode root = doc.SelectSingleNode("root");
                XmlElement element = doc.CreateElement("Path");
                element.SetAttribute("Xpath",FBD.SelectedPath);
                element.SetAttribute("Set","false");
                root.AppendChild(element);
                doc.Save(Xmlpath);
                cob_Path.SelectedIndex = cob_Path.Items.Count - 1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Xmlpath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Path.xml";
            XmlDocument doc = CommTool.XmlFile.LoadXml(Xmlpath);
            XmlNode root = doc.SelectSingleNode("root");
            foreach (XmlNode child in root.ChildNodes)
            {
                child.Attributes["Set"].Value = "false";
                if (child.Attributes["Xpath"].Value.ToString() == cob_Path.SelectedItem.ToString())
                {
                    child.Attributes["Set"].Value = "true";
                }
            }
            doc.Save(Xmlpath);
            #region 20150709 add for  懶人工具 #73
            string configPath = Path.Combine(cob_Path.SelectedItem.ToString(), "appSettings.config");
            ConfigManager mage = new ConfigManager(configPath, "CurrentDbSource", true);
            SQLHelper.SHelper._sqlconnection = mage.GetValue("CurrentDbSource");
            string[] sp = SQLHelper.SHelper._sqlconnection.Split(';');
            SQLHelper.SHelper.DBIP = sp[0].Replace("Data Source=", "");
            SQLHelper.SHelper.DBName = sp[1].Replace("Initial Catalog=", "");
            TB_DBIP.Text = SQLHelper.SHelper.DBIP;
            tb_DBName.Text = SQLHelper.SHelper.DBName;
            #endregion
            MessageBox.Show("設定完成!!");
            this.Close();
        }
    }
}
