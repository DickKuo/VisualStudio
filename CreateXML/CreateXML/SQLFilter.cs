using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CreateXML {
    public partial class SQLFilter : Form {
        public SQLFilter() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {

          richTextBox1.Text=  richTextBox1.Text.Replace(@"\r\n","").Replace("\"","");

          try {
              SQL.SQL sq = new SQL.SQL(TBConnectstring.Text);
              sq.CommandString = richTextBox1.Text;
              dataGridView1.DataSource = sq.ExeRetrunDataTable();
          }
          catch(Exception ex) {
              MessageBox.Show(ex.ToString());
          }
        }

        private void SQLFilter_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.F5) {
                button1.PerformClick();
            }
        }

       
    }
}
