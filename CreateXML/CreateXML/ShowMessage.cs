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
       
        public ShowMessage(string pMessage) {
            InitializeComponent();
            richTextBox1.Text = pMessage;
        }
    }
}
