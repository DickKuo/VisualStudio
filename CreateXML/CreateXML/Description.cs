using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CreateXML {
    public partial class Description : Form {
        public Description(string version) {
            InitializeComponent();
            labelTime.Text = "2014/09/19";
            LabVersion.Text = "Version ：" + version;

            richTextBox1.AppendText("1.單檔一鍵完成。\r\n");
        }
    }
}
