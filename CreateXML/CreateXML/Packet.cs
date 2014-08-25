using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace CreateXML {
    public partial class Packet : Form {
        public Packet() {
            InitializeComponent();
            XmlDocument doc = Tools.XmlTool.LoadXml(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Path.xml");
            XmlNode root = doc.SelectSingleNode("root");
            foreach(XmlNode node in root.ChildNodes)
            {
                cb_Path.Items.Add(node.Attributes["Xpath"].Value.ToString());
            }
            cb_Path.SelectedIndex = cb_Path.Items.Count - 1;
        }

        private void button1_Click(object sender, EventArgs e) {
           
            MoveFile.Move move = new MoveFile.Move();
            List<string> ClientList = new List<string>();
            List<string> ServerList = new List<string>();

            string temp = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "ListDll.xml";
            XmlDocument doc = Tools.XmlTool.LoadXml(temp);

            XmlNode root = doc.SelectSingleNode("root");

            XmlNode client = root.SelectSingleNode("Client");

            foreach(XmlNode child in client.ChildNodes)
            {
                ClientList.Add(child.Attributes["Name"].Value.ToString());                
            }

            XmlNode Server = root.SelectSingleNode("Server");
            foreach(XmlNode child in Server.ChildNodes) {
                ServerList.Add(child.Attributes["Name"].Value.ToString());
            }


            move.MoveEachFile(cb_Path.SelectedItem.ToString(), tb_Number.Text+@"\Client", ClientList,"client");
            move.MoveEachFile(cb_Path.SelectedItem.ToString(), tb_Number.Text + @"\Server", ServerList,"server");                      
            move.RecurrceMove(cb_Path.SelectedItem.ToString() + @"\Configuration",@"C:\Share\"+ tb_Number.Text + @"\Server", true);
            //MoveFileFileNameStart(cb_Path.SelectedItem.ToString() + @"\Configuration\Query", @"C:\Share\" + tb_Number.Text + @"\Server\Configuration\Query","", true);
            MoveFileFileName(cb_Path.SelectedItem.ToString() + @"\Configuration\Query\Case", @"C:\Share\" + tb_Number.Text + @"\Server\Configuration\Query\Case", true);
            MoveFileFileName(cb_Path.SelectedItem.ToString() + @"\HRShellXml", @"C:\Share\" + tb_Number.Text + @"\Server\HRShellXml",true);

            MessageBox.Show("建置完成");
        }


        private void button2_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK) {
                string path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Path.xml";
                XmlDocument doc = Tools.XmlTool.LoadXml(path);
                XmlNode root = doc.SelectSingleNode("root");
                XmlElement child= doc.CreateElement("Path");
                child.SetAttribute("Xpath",fbd.SelectedPath);
                child.SetAttribute("Set", "true");
                root.AppendChild(child);
                doc.Save(path);
                cb_Path.Items.Add(fbd.SelectedPath);
                cb_Path.SelectedIndex = cb_Path.Items.Count - 1;
            }
        }


        public virtual void MoveFileFileNameStart(string pSourcePath,string pDestinationPath,string pIndexString,bool IsOverWrite)
        {          
          foreach(string str in  Directory.GetFiles(pSourcePath))
          {
              FileInfo fi = new FileInfo(str);
              if(fi.Name.Substring(0, 1).Equals(pIndexString)) {
                  string NewFile = pDestinationPath + @"\" + fi.Name;
                  fi.CopyTo(NewFile, IsOverWrite);
              }
          }
        }


        public virtual void MoveFileFileName(string pSourcePath, string pDestinationPath, bool IsOverWrite) {
            foreach(string str in Directory.GetFiles(pSourcePath)) {
                FileInfo fi = new FileInfo(str);

                if(!Directory.Exists(pDestinationPath)) {
                    Directory.CreateDirectory(pDestinationPath);
                }
                    string NewFile = pDestinationPath + @"\" + fi.Name;
                    fi.CopyTo(NewFile, IsOverWrite);
                    FileInfo newfi = new FileInfo(NewFile);
                    newfi.IsReadOnly = false;
              
            }
        }

    }
}
