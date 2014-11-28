using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CreateXML {
    public partial class ShowMessage : Form {

        public string Context { set; get; }
        public ShowMessage() {
            InitializeComponent();
            //richTextBox1.Text = pMessage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBox1.Text))
            {
                return;
            }
            richTextBox1.Text = Tools.XmlTool.ReOrderMethod(richTextBox1.Text);
            MessageBox.Show("重新排序完成");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBox1.Text))
            {
                return;
            }
            richTextBox1.Text = Tools.XmlTool.NewOrderMethod(richTextBox1.Text);
            MessageBox.Show("重新產生完成");
        }
    }
}
