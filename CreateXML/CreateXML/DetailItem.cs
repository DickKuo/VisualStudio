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

namespace CreateXML {
    public partial class DetailItem : Form {
        public DetailItem() {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e) {            
            base.OnLoad(e);
            treeView1.AllowDrop = true;
            treeView1.Dock = DockStyle.Fill;
            this.treeView1.ItemDrag += new ItemDragEventHandler(treeView1_ItemDrag);
            this.treeView1.DragEnter += new DragEventHandler(treeView1_DragEnter);

            XmlDocument doc = Tools.XmlTool.LoadXml(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "SaveFile.xml");
            XmlNode root = doc.SelectSingleNode("root");
             foreach(XmlNode node in root.ChildNodes)
             {
                 treeView1.Nodes.Add(node.Attributes["ClassName"].Value);
             }
            
        }

        void treeView1_DragEnter(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;

        }

        void treeView1_ItemDrag(object sender, ItemDragEventArgs e) {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

      
    }
}
