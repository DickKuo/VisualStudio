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
    public partial class PagesName : BaseDialog
    {
        public string Name { set; get; }

        public PagesName()
        {
            InitializeComponent();
        }

        public override void button1_Click(object sender, EventArgs e)
        {
            base.button1_Click(sender, e);
            Name = textBox1.Text;
        }
    }
}
