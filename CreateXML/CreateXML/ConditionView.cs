using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CreateXML {
    public partial class ConditionView : Form
    {
         public ConditionView() {
            InitializeComponent();
        }

        public ConditionView( QueryViewCondition pOldView) {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add("ParameterName");
            dt.Columns.Add("Type");
            dt.Columns.Add("Field");
            dt.Columns.Add("Conditon1");
            dt.Columns.Add("Conditon2");
            dt.Columns.Add("sybel");
            textBox1.Text = pOldView.BrowseName;
            textBox2.Text = pOldView.Description;
            comboBox1.Text = pOldView.Type;  //20141028 add by Dick for 加入回寫 Type: Browse、Select           
            foreach(Condition condition in pOldView.ConditionList)
            {
                string[] row1 = new string[] { condition.ParameterName, condition.Type, condition.Field, condition.Conditon1, condition.Conditon2, condition.sybel };

                dataGridView1.Rows.Add(row1);

                //DataGridView NewRow = new DataGridView();
                ////DataRow NewRow = dt.NewRow();
                //NewRow["ParameterName"] = condition.ParameterName;
                //NewRow["Type"]=condition.Type;
                //NewRow["Field"] = condition.Field;
                //NewRow["Conditon1"] = condition.Conditon1;
                //NewRow["Conditon2"] = condition.Conditon2;
                //NewRow["sybel"] = condition.sybel;                
                //dt.Rows.Add(NewRow);
            }
            
          
        }

         

        public QueryViewCondition Result { set; get; }



        /// <summary>
        /// 20141002 add by Dick for 加入條件項目。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {
            if(string.IsNullOrEmpty(textBox1.Text)) {
                MessageBox.Show("頁籤名稱不可空白");
                return;
            }
            Result = new QueryViewCondition();
            Result.BrowseName = textBox1.Text;
            Result.Description = textBox2.Text;
            Result.Type = comboBox1.SelectedItem.ToString();
            Result.ConditionList.Clear();
            int count = 1;
             foreach(DataGridViewRow dr in dataGridView1.Rows) {
                 if(dr.Cells["Type"].Value != null) {
                     Condition condition = new Condition();
                     condition.ID = count.ToString();
                     condition.ParentID = "0";
                     count++;
                     if(dr.Cells["ParameterName"].Value != null) {
                         condition.ParameterName = dr.Cells["ParameterName"].Value.ToString();
                     }
                     if(dr.Cells["Type"].Value != null) {
                         condition.Type = dr.Cells["Type"].Value.ToString();
                     }
                     if(dr.Cells["Field"].Value != null) {
                         condition.Field = dr.Cells["Field"].Value.ToString();
                     }
                     if(dr.Cells["Conditon1"].Value != null) {
                         condition.Conditon1 = dr.Cells["Conditon1"].Value.ToString();
                     }
                     if(dr.Cells["Conditon2"].Value != null) {
                         condition.Conditon2 = dr.Cells["Conditon2"].Value.ToString();
                     }
                     if(dr.Cells["sybel"].Value != null) {
                         condition.sybel = dr.Cells["sybel"].Value.ToString();
                     }
                     Result.ConditionList.Add(condition);
                 }
             }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


    }
}
