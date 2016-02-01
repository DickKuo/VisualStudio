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
    public partial class MenuEdit : Form
    {
        public MenuEdit()
        {
            InitializeComponent();
        }

        private List<string> FoodList = new List<string>();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DataTable dt = new DataTable();
            dt.Columns.Add("Ingridients");
            dt.Columns.Add("Cost");
            dt.Columns.Add("Total Quantity(g/ml)");
            dt.Columns.Add("Quantity used");
            dt.Columns.Add("Quantity waste");
            dt.Columns.Add("Yield");
            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FoodAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectFood food = new SelectFood();
            food.Show();

        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point po = new Point(30, 50);
                contextMenuStrip1.Show(MousePosition);
            } 
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode"))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }  
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode NewNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                if (!string.IsNullOrEmpty(NewNode.ImageKey))
                {
                    if (FoodList.Contains(NewNode.ImageKey))
                    {
                        MessageBox.Show("不可加入重複食材");
                    }
                    else
                    {
                        FoodList.Add(NewNode.ImageKey);
                        DataRow dr = dt.NewRow();
                        dr["Ingridients"] = NewNode.Text;
                        dr["Cost"] = GetCost(NewNode.ImageKey);
                        dt.Rows.Add(dr);
                        dataGridView1.DataSource = dt;
                    }
                }
            }
        }


        private decimal GetCost(string Id)
        {
            DateTime End =DateTime.Now;
            DateTime Begin =DateTime.Now.AddMonths(-6);
            string sql = string.Format(@"
                Select SUM(Price)/ COUNT(DatilyId)  as 'Aveger' from Datily  
                Where FoodId=@FoodId AND Day between @Begin AND @End");
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("FoodId", Id);
            dic.Add("Begin",Begin.ToString("yyyy/MM/dd"));
            dic.Add("End", End.ToString("yyyy/MM/dd"));
            DataTable dt = SQLHelper.SHelper.ExeDataTableUseParameter(sql, dic);
            decimal result = 0;
            if (dt.Rows.Count > 0)
            {
                result = Convert.ToDecimal(dt.Rows[0][0]);
            }
            return result;
        }

    }
}
