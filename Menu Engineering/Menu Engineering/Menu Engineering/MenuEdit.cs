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
        public MenuEdit(OperatingType pType)
        {
            InitializeComponent();
            PType = pType;
        }
        private OperatingType PType;

        private decimal TotalCost = 0;
        public string CollectionId = string.Empty;

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
                        Quantity qu = new Quantity();
                        qu.FoodName = NewNode.Text;
                        if (qu.ShowDialog() == DialogResult.OK)
                        {
                            dr["Cost"] =Math.Round( GetCost(NewNode.ImageKey),2);
                            TotalCost += Math.Round(GetCost(NewNode.ImageKey), 2);
                            dr["Total Quantity(g/ml)"] = qu.Total;
                            dr["Quantity used"] = qu.Used;
                            dr["Quantity waste"] = qu.Total - qu.Used;
                            if (qu.Total > 0)
                            {
                                dr["Yield"] = (Math.Round( (qu.Used / qu.Total) ,2)* 100).ToString() + "%";
                            }
                            dt.Rows.Add(dr);
                            dataGridView1.DataSource = dt;
                            TbCost.Text = TotalCost.ToString();
                        }
                    }
                }
            }
        }


        private decimal GetCost(string Id)
        {
            DateTime End =DateTime.Now;
            DateTime Begin =DateTime.Now.AddMonths(-6);
            string sql = string.Format(@"
                Select Case When  SUM(Price)/ COUNT(DatilyId) =0 then 0 Else SUM(Price)/ COUNT(DatilyId)  End  as 'Aveger' from Datily  
                Where FoodId=@FoodId AND Day between @Begin AND @End");
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("FoodId", Id);
            dic.Add("Begin",Begin.ToString("yyyy/MM/dd"));
            dic.Add("End", End.ToString("yyyy/MM/dd"));
            DataTable dt = SQLHelper.SHelper.ExeDataTableUseParameter(sql, dic);
            decimal result = 0;
            if (dt.Rows.Count > 0 && dt!=null)
            {
                decimal.TryParse(dt.Rows[0][0].ToString(), out result);
            }
            return result;
        }

        private void TbCost_TextChanged(object sender, EventArgs e)
        {
            CaculeCost();
        }

        private void numericUpDownSalePrice_ValueChanged(object sender, EventArgs e)
        {
            CaculeCost();
        }

        private void CaculeCost()
        {
            decimal cost = 0;
            decimal.TryParse(TbCost.Text, out cost);
            textBox2.Text = (numericUpDownSalePrice.Value - cost).ToString();

            TbPerce.Text = Math.Round(numericUpDownSalePrice.Value / Convert.ToDecimal(textBox2.Text), 2).ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            if (PType == OperatingType.Add)
            {
                DatabaseInterFace Info = new Database();
                Menus menus = new Menus();
                menus.MenuId = Guid.NewGuid();
                menus.MenuCollectionId = CollectionId;
                menus.Name = rbName.Text;
                menus.Photo =new byte[1];
                menus.Remark = string.Empty;
                menus.SalePrice = numericUpDownSalePrice.Value;
                menus.NumberSold =   numericUpDownNumberSold.Value;
                DataTable dt = dataGridView1.DataSource as DataTable;
                dt.TableName = "MenuFood";
                dt.Columns.Add("MenuId");
                foreach (DataRow dr in dt.Rows)
                {
                    dr["MenuId"] = menus.MenuId.ToString();
                }

                Info.UpdateMenuFood(menus, dt);
            }
        }

       
       
    }
}
