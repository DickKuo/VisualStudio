using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CreateXML
{
    public partial class InsertEntities : Form
    {
        public InsertEntities()
        {
            InitializeComponent();
        }

        public string context { set; get; }

        
        

        private void button1_Click(object sender, EventArgs e)
        {
            context = textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
