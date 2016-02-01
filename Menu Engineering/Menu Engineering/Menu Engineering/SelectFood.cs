using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Menu_Engineering
{
    public partial class SelectFood : Form
    {
        public SelectFood()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            treeView1.AllowDrop = true;
            treeView1.Dock = DockStyle.Fill;
            Dictionary<string, List<TreeNode>> Detail = GetFoodNodes();
            DataTable dt = GetCollection();

            foreach(DataRow dr in dt.Rows)
            {
                TreeNode ParNode = new TreeNode();
                ParNode.Text = dr["Name"].ToString();
                string collectionid = dr["CollectionsId"].ToString();
                if (Detail.ContainsKey(collectionid))
                {
                    List<TreeNode> li = Detail[collectionid];
                    foreach(TreeNode node in li)
                    {
                        ParNode.Nodes.Add(node);
                    }
                }
                treeView1.Nodes.Add(ParNode);
            }
            
            this.treeView1.ItemDrag += new ItemDragEventHandler(treeView1_ItemDrag);
            this.treeView1.DragEnter += new DragEventHandler(treeView1_DragEnter);

        }
        void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

        }

        void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private DataTable GetCollection()
        {
            string sql = "Select * from Collections";
            return SQLHelper.SHelper.ExeDataTable(sql);
        }

        /// <summary>
        /// 取得所有食材節點
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, List<TreeNode>> GetFoodNodes()
        {
            Dictionary<string, List<TreeNode>> nodes = new Dictionary<string, List<TreeNode>>();
            string sql = string.Format("Select * from Food");
            DataTable dt = SQLHelper.SHelper.ExeDataTable(sql);            
            foreach(DataRow dr in dt.Rows)
            {
                TreeNode Node = new TreeNode();
                Node.Text = dr["Name"].ToString();
                Node.ImageKey = dr["FoodId"].ToString();
                string collectionId = dr["CollectionsId"].ToString();
                if (!nodes.ContainsKey(collectionId))
                {
                    List<TreeNode> li = new List<TreeNode>();
                    li.Add(Node);
                    nodes.Add(collectionId, li);
                }
                else
                {
                    List<TreeNode> li = nodes[collectionId];
                    li.Add(Node);
                    nodes[collectionId] = li;
                }
            }

            return nodes;
        }
    }
}
